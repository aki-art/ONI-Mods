using FMODUnity;
using ONITwitchLib.Utils;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class DeathLaser : KMonoBehaviour, ISim33ms, ISim200ms
	{
		private float height;
		private int worldMinY;
		private int worldMaxY;

		private const int RADIUS = 2;
		private const float BG_CHANCE = 0.4f;
		private const int Y_CHUNK_SIZE = 16;

		private Animator animator;

		private bool hasPlayedSound;

		private static FMOD.VECTOR ZERO = new();

		private int soundTracker1;
		private int soundTracker2;

		private float elapsed;

		private int currentYChunk;

		private int worldIdx;
		private float temp, carbonTemp;
		private bool started;

		private int _cameraShakeID;

		public override void OnSpawn()
		{
			_cameraShakeID = GetInstanceID();
			Appear();
		}

		public void Appear()
		{
			var position = transform.position;
			var cell = Grid.PosToCell(position);
			worldIdx = Grid.WorldIdx[cell];
			var world = ClusterManager.Instance.GetWorld(worldIdx);

			if (world == null)
			{
				Log.Warning("Something went wrong, invalid world ID for DeathLaserEvent");
				return;
			}

			Grid.CellToXY(cell, out startX, out _);
			Grid.PosToXY(world.minimumBounds, out _, out worldMinY);
			Grid.PosToXY(world.maximumBounds, out _, out worldMaxY);

			height = worldMaxY - worldMinY;

			transform.position = position with { y = (height / 2f) + worldMinY, z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };

			temp = GameUtil.GetTemperatureConvertedToKelvin(70, GameUtil.TemperatureUnit.Celsius);
			carbonTemp = GameUtil.GetTemperatureConvertedToKelvin(120, GameUtil.TemperatureUnit.Celsius);

			var smallBeam = transform.Find("SmallBeam").GetComponent<MeshFilter>();
			var midBeam = transform.Find("MidBeam").GetComponent<MeshFilter>();
			var mainBeam = transform.Find("MainBeam").GetComponent<MeshFilter>();
			var ringFront = transform.Find("RingFront").GetComponent<MeshFilter>();
			var ringBack = transform.Find("RingBack").GetComponent<MeshFilter>();
			var glowA = transform.Find("GlowA").GetComponent<MeshFilter>();
			var glowB = transform.Find("GlowB").GetComponent<MeshFilter>();
			var glowC = transform.Find("GlowC").GetComponent<MeshFilter>();

			GenerateMesh(smallBeam, 2.17f);
			glowA.mesh = Instantiate(smallBeam.mesh);
			glowB.mesh = Instantiate(smallBeam.mesh);
			glowC.mesh = Instantiate(smallBeam.mesh);
			GenerateMesh(midBeam, 2.75f);
			GenerateMesh(mainBeam, 3.5f);
			GenerateMesh(ringFront, 3f);
			ringBack.mesh = Instantiate(ringFront.mesh);

			var repeatY = new Vector2(1f, (int)(height / 2));

			smallBeam.GetComponent<MeshRenderer>().material.mainTextureScale = repeatY;
			midBeam.GetComponent<MeshRenderer>().material.mainTextureScale = repeatY;
			mainBeam.GetComponent<MeshRenderer>().material.mainTextureScale = repeatY;
			ringFront.GetComponent<MeshRenderer>().material.mainTextureScale = repeatY;
			ringBack.GetComponent<MeshRenderer>().material.mainTextureScale = repeatY;

			animator = GetComponent<Animator>();

			soundTracker1 = AudioUtil.PlaySound(ModAssets.Sounds.LASER_CHARGE, transform.position, ModAssets.GetSFXVolume() * 1.2f);
			started = true;
		}

		public float totalCameraShakeTimer = 4f;
		internal int startX;

		float CameraShakePower(float elapsedTime)
		{
			float p = elapsedTime / totalCameraShakeTimer;
			float l = 1f;
			var result = (elapsedTime / l * 0.5f) - 1f;
			result *= result;
			result = 1f - result;

			return result * p;
		}

		public void Sim200ms(float dt)
		{
			int startY = currentYChunk * Y_CHUNK_SIZE;

			if (started && startY < height)
			{
				DestroyTiles(startY);
				CharEntities(startY);
				BreakBuildings(startY);

				currentYChunk++;
			}
		}

		public void Sim33ms(float dt)
		{
			if (animator == null)
				return;

			elapsed += dt;
			if (elapsed < totalCameraShakeTimer)
			{
				AkisTwitchEvents.Instance.SetCameraShake(_cameraShakeID, CameraShakePower(elapsed));
			}

			bool shouldUpdateSound = soundTracker1 != 0 || soundTracker2 != 0;
			if (shouldUpdateSound)
			{
				var pos = CameraController.Instance.GetVerticallyScaledPosition(PosUtil.ClampedMouseWorldPos(), false) with { x = transform.position.x };

				var position = pos.ToFMODVector();

				if (soundTracker1 != 0)
				{
					RuntimeManager.CoreSystem.getChannel(soundTracker1, out var channel);
					channel.set3DAttributes(ref position, ref ZERO);
				}

				if (soundTracker2 != 0)
				{
					RuntimeManager.CoreSystem.getChannel(soundTracker2, out var channel);
					channel.set3DAttributes(ref position, ref ZERO);
				}
			}

			if (!hasPlayedSound && elapsed >= 0.4f)
			{
				soundTracker2 = AudioUtil.PlaySound(ModAssets.Sounds.LASER, transform.position, ModAssets.GetSFXVolume() * 2.5f);
				hasPlayedSound = true;
			}

			if (elapsed >= 8f)
			{
				Destroy(gameObject);
				AkisTwitchEvents.Instance.SetCameraShake(_cameraShakeID, 0f);
				AkisTwitchEvents.Instance.SetCameraShake(_cameraShakeID, 0f);
			}
		}

		private void CharEntities(int startY)
		{
			var extents = new Extents(startX - RADIUS, startY, RADIUS * 2 + 1, Y_CHUNK_SIZE);

			var entries = ListPool<ScenePartitionerEntry, MidasToucher>.Allocate();

			GameScenePartitioner.Instance.GatherEntries(
				extents,
				GameScenePartitioner.Instance.pickupablesLayer,
				entries);

			foreach (var entry in entries)
			{
				if (entry.obj is Pickupable pickupable)
				{

					if (pickupable.TryGetComponent(out Butcherable butcherable) && butcherable.drops != null)
					{
						foreach (var drop in butcherable.drops)
						{
							if (ModAPI.cookedDropsLookup.TryGetValue(drop.Key, out var cookedDrop))
							{
								var cooked = FUtility.Utils.Spawn(cookedDrop.tag, butcherable.gameObject);
								if (cooked.TryGetComponent(out PrimaryElement primaryElement))
								{
									primaryElement.SetTemperature(400);
									primaryElement.Mass *= drop.Value;
								}
							}

							// don't double drop
							butcherable.SetDrops(new string[] { });
						}

					}

					if (pickupable.TryGetComponent(out MinionBrain _) && pickupable.TryGetComponent(out KBatchedAnimController kbac))
						kbac.TintColour = new Color(0.03f, 0.03f, 0.03f, 1f);

					var deathMonitor = pickupable.GetSMI<DeathMonitor.Instance>();
					if (deathMonitor != null)
						deathMonitor.Kill(TDeaths.lasered);

					/*	if (pickupable.TryGetComponent(out MinionIdentity _))
							CharredEntity.CreateAndChar(pickupable.gameObject);*/
				}
			}
		}

		private void BreakBuildings(int startY)
		{
			var extents = new Extents(startX - RADIUS, startY, RADIUS * 2 + 1, Y_CHUNK_SIZE);

			var entries = ListPool<ScenePartitionerEntry, MidasToucher>.Allocate();

			GameScenePartitioner.Instance.GatherEntries(
				extents,
				GameScenePartitioner.Instance.completeBuildings,
				entries);

			foreach (var entry in entries)
			{
				if (entry.obj is BuildingComplete building)
				{
					if (building.Def.Breakable
						&& building.Def.ShowInBuildMenu // don't kill off one of a kind stuff, lets not be THAT evil
						&& building.TryGetComponent(out BuildingHP hp))
					{
						hp.DoDamage(hp.MaxHitPoints);
					}
				}
			}
		}

		private void DestroyTiles(int startY)
		{
			for (int xo = -RADIUS; xo <= RADIUS; xo++)
			{
				var x = xo + startX;
				var distance = Mathf.Abs(xo);

				for (int y = startY; y <= startY + Y_CHUNK_SIZE; y++)
				{
					var targetCell = Grid.XYToCell(x, y);

					if (!Grid.IsValidCellInWorld(targetCell, worldIdx))
						continue;

					if (AGridUtil.protectedCells.Contains(targetCell))
						continue;

					if (Grid.Element[targetCell].id == SimHashes.Unobtanium)
						continue;

					var element = Grid.Element[targetCell];

					if (element.hardness == byte.MaxValue && element.molarMass > 5000)
						continue;

					if (distance == 0 || (distance == 1 && Random.value < 0.4f))
					{
						// too laggy
						//WorldDamage.Instance.ApplyDamage(targetCell, 100f, -1);

						if (Grid.Foundation[targetCell])
							WorldDamage.Instance.ApplyDamage(targetCell, 100f, -1);

						if (Random.value < BG_CHANCE)
							AkisTwitchEvents.Instance.AddZoneTypeOverride(targetCell, ProcGen.SubWorld.ZoneType.Space);

						SimMessages.ReplaceAndDisplaceElement(targetCell, SimHashes.CarbonDioxide, AGridUtil.cellEvent, 1f, temp);
					}
					else if (distance == 1 || (distance == 2 && Random.value < 0.5f))
					{
						var meltedElement = element.highTempTransitionTarget;

						if (meltedElement == SimHashes.Void)
							continue;

						if (Random.value < 0.1f)
							meltedElement = SimHashes.Carbon;

						var meltedAndThenCooledTemp = Mathf.Max(Grid.Temperature[targetCell], carbonTemp);

						AGridUtil.ReplaceElement(targetCell, Grid.Element[targetCell], meltedElement, tempOverride: meltedAndThenCooledTemp);
					}
					else
					{
						WorldDamage.Instance.ApplyDamage(targetCell, Random.Range(0.1f, 0.8f), -1);
					}
				}
			}
		}

		private void GenerateMesh(MeshFilter meshFilter, float width)
		{
			Mesh mesh = new()
			{
				vertices =
				[
					new(-width / 2f, -height / 2f, 0), // Bottom-left
					new(width / 2f, -height / 2f, 0),  // Bottom-right
					new(-width / 2f, height / 2f, 0),  // Top-left
					new(width / 2f, height / 2f, 0)    // Top-right
				],

				triangles =
				[
					0, 2, 1, // First triangle
					1, 2, 3  // Second triangle
				],

				uv =
				[
					new(0, 0), // Bottom-left
					new(1, 0), // Bottom-right
					new(0, 1), // Top-left
					new(1, 1)  // Top-right
				]
			};

			mesh.RecalculateNormals();
			mesh.RecalculateBounds();

			meshFilter.mesh = mesh;
		}
	}
}
