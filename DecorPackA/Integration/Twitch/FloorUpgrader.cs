using Database;
using DecorPackA.Buildings.StainedGlassTile;
using ONITwitchLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace DecorPackA.Integration.Twitch
{
	public class FloorUpgrader : KMonoBehaviour, ISim1000ms
	{
		public bool finished;
		private Room room;
		public const int MIN_TILE_COUNT = 5;
		private List<int> tilesToUpgrade;
		private BuildingDef glassTileDef1;
		private BuildingDef glassTileDef2;
		public bool running;
		public Tag element1;
		public Tag element2;
		public bool checkerBoard;

		public static HashSet<RoomType> preferredRoomTypes;
		public static Room cachedLastEligible;

		public static HashSet<Tag> discoveryBarredElements = new()
		{
			SimHashes.Milk.CreateTag(),
			SimHashes.TempConductorSolid.CreateTag(),
			SimHashes.SuperInsulator.CreateTag(),
			SimHashes.HardPolypropylene.CreateTag()
		};

		public static void OnDbInit()
		{
			var roomTypes = Db.Get().RoomTypes;
			preferredRoomTypes = new HashSet<RoomType>()
			{
				roomTypes.MessHall,
				roomTypes.GreatHall,
				roomTypes.Barracks,
				roomTypes.Bedroom,
				roomTypes.PrivateBedroom,
				roomTypes.Hospital,
				roomTypes.Kitchen,
				roomTypes.Latrine,
				roomTypes.Laboratory,
				roomTypes.MassageClinic,
				roomTypes.RecRoom,
				roomTypes.PlumbedBathroom
			};
		}

		public static Room FindEligibleRoom()
		{
			var watch = new Stopwatch();
			watch.Start();

			if (cachedLastEligible != null
				&& preferredRoomTypes.Contains(cachedLastEligible.roomType)
				&& IsRoomEligible(cachedLastEligible))
			{
				return cachedLastEligible;
			}

			var roomTypes = Db.Get().RoomTypes;
			var rooms = new List<Room>(Game.Instance.roomProber.rooms);

			var result = rooms
				.Where(r => IsRoomValid(r, roomTypes))
				.OrderBy(room => preferredRoomTypes.Contains(room.roomType) ? 0 : 1)
				.ToList()
				.Find(IsRoomEligible);

			watch.Stop();
			Log.Debuglog($"Found room in {watch.ElapsedMilliseconds} ms");

			cachedLastEligible = result;

			return result;
		}

		private static bool IsRoomValid(Room r, RoomTypes roomTypes)
		{
			if (r.roomType == roomTypes.Neutral)
				return false;

			var cell = Grid.PosToCell(r.cavity.GetCenter());

			if (!Grid.IsVisible(cell))
				return false;

			return true;
		}

		public override void OnSpawn()
		{
			var room = FindEligibleRoom();

			if (room == null)
			{
				ToastManager.InstantiateToast("Oh no", "Tried to upgrade room but something went wrong.");
				return;
			}

			checkerBoard = Random.value < 0.5f;
			this.room = room;

			tilesToUpgrade?.Clear();
			tilesToUpgrade = GetFloor(room, 128);

			var buildingDefs = new List<BuildingDef>(Assets.BuildingDefs)
				.Where(IsBuildingDefAvailable)
				.ToList();

			if (buildingDefs == null || buildingDefs.Count < 2)
			{
				Log.Warning("No glass tiles found");
				return;
			}

			buildingDefs.Shuffle();

			glassTileDef1 = buildingDefs[0];
			element1 = GetElementFromGlassTileDef(glassTileDef1);

			if (checkerBoard)
			{
				glassTileDef2 = buildingDefs[1];
				element2 = GetElementFromGlassTileDef(glassTileDef2);
			}

			Upgrade();
		}

		private bool IsBuildingDefAvailable(BuildingDef def)
		{
			if (!def.BuildingComplete.HasTag(ModAssets.Tags.stainedGlass))
				return false;

			if (TwitchMod.forbiddenUpgradeElements != null && TwitchMod.forbiddenUpgradeElements.Contains(def.PrefabID))
				return false;

			var element = GetElementFromGlassTileDef(def);
			if (discoveryBarredElements.Contains(element))
				return DiscoveredResources.Instance.IsDiscovered(element);

			return true;
		}

		private Tag GetElementFromGlassTileDef(BuildingDef def)
		{
			return StainedGlassTiles.reverseTileTagDict.TryGetValue(def.PrefabID, out var result)
				? result
				: SimHashes.Diamond.CreateTag();
		}

		private static bool IsUpgradeableFloor(int cell)
		{
			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
				return go != null && go.PrefabID() == TileConfig.ID;

			if (Grid.IsSolidCell(cell))
			{
				// dont yoink out natural tiles under plants
				// (plants are on the building layer, not the plants layer)
				if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(Grid.CellAbove(cell), out var goAbove))
				{
					var isPlant = goAbove != null && (goAbove.GetComponent<Uprootable>() != null || goAbove.GetComponent<BasicForagePlantPlanted>() != null);
					return !isPlant;
				}

				return true;
			}

			return false;
		}

		// TODO: this can be a lot more efficient
		private static HashSet<int> GetRoomCells(Room room)
		{
			var result = new HashSet<int>();

			for (int x = room.cavity.minX; x <= room.cavity.maxX; x++)
			{
				for (int y = room.cavity.minY; y <= room.cavity.maxY; y++)
				{
					var cell = Grid.XYToCell(x, y);
					if (Game.Instance.roomProber.GetCavityForCell(cell) == room.cavity)
						result.Add(cell);
				}
			}

			return result;
		}

		public static List<int> GetFloor(Room room, int maxTiles)
		{
			var roomCells = GetRoomCells(room); //GridUtil.FloodCollectCells(center, cell => IsInRoom(cell, room), 128);

			if (roomCells.Count == 0)
				return null;

			var tiles = new List<int>();

			foreach (var cell in roomCells)
			{
				var cellBelow = Grid.CellBelow(cell);
				if (IsUpgradeableFloor(cellBelow))
				{
					tiles.Add(cellBelow);

					if (tiles.Count >= maxTiles)
						return tiles;
				}
			}

			return tiles;
		}

		private static bool IsRoomEligible(Room room)
		{
			if (room.roomType == Db.Get().RoomTypes.Neutral || room.roomType == Db.Get().RoomTypes.NatureReserve)
				return false;

			var tiles = GetFloor(room, 20);
			return tiles != null && tiles.Count >= 5;
		}

		void Upgrade()
		{
			tilesToUpgrade.Shuffle();
			running = true;

			var message = room.roomType == Db.Get().RoomTypes.CreaturePen ?
				(string)STRINGS.TWITCH.FLOOR_UPGRADE.TOAST_STABLE
				: STRINGS.TWITCH.FLOOR_UPGRADE.TOAST_GENERIC.Replace("{RoomType}", room.roomType.Name);

			ToastManager.InstantiateToastWithPosTarget(STRINGS.TWITCH.FLOOR_UPGRADE.NAME, message, room.cavity.GetCenter());

			cachedLastEligible = null;
		}

		private void SpawnGlassTile(int cell)
		{
			var element = element1;
			var def = glassTileDef1;

			if (checkerBoard)
			{
				Grid.CellToXY(cell, out int x, out int y);
				if (x % 2 != y % 2)
				{
					element = element2;
					def = glassTileDef2;
				}
			}

			var elems = new List<Tag>(glassTileDef1.DefaultElements())
			{
				[1] = element
			};

			def.Build(cell, Orientation.Neutral, null, elems, 300f, false, GameClock.Instance.GetTime() + 1);
			World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, cell);

			SpawnSparkles(Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront));
		}

		private void SpawnSparkles(Vector3 position)
		{
			var sparkles = Instantiate(ModAssets.Prefabs.sparklesParticles);
			sparkles.transform.position = position;
			sparkles.SetActive(true);
			sparkles.GetComponent<ParticleSystem>().Play();
		}

		public void Sim1000ms(float _)
		{
			if (!running)
				return;

			var tileCount = Random.Range(0, 3);

			for (var i = 0; i < tileCount; i++)
			{
				if (tilesToUpgrade != null && tilesToUpgrade.Count > 0)
				{
					var cell = tilesToUpgrade.First();
					UpgradeSingleTile(cell);
					tilesToUpgrade.Remove(cell);
				}
				else
					Util.KDestroyGameObject(gameObject);
			}
		}

		private void UpgradeSingleTile(int cell)
		{
			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
			{
				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					deconstructable.ForceDestroyAndGetMaterials();
					GameScheduler.Instance.ScheduleNextFrame("", _ => SpawnGlassTile(cell));
				}
			}
			else
			{
				WorldDamage.Instance.ApplyDamage(cell, 1f, -1);
				SpawnGlassTile(cell);
			}
		}
	}
}
