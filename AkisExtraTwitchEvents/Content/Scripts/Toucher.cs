using FUtility;
using ONITwitchLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public abstract class Toucher : KMonoBehaviour, ISim33ms
	{
		[SerializeField] public float lifeTime;
		[SerializeField] public float radius;
		[SerializeField] public int cellsPerUpdate;
		[SerializeField] public Color markerColor;

		public static CellElementEvent toucherEvent = new("AETE_ToucherCellEvent", "Touch event spawn", false);

		private HashSet<int> alreadyVisitedCells;
		private float elapsedLifeTime = 0;
		private Transform circleMarker;
		private HashSet<int> cells;

		public abstract bool UpdateCell(int cell);

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			alreadyVisitedCells = new HashSet<int>();
			cells = new HashSet<int>();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (ModAssets.Prefabs.sparkleCircle == null)
			{
				Debug.LogWarning("marker is null");
				return;
			}

			var markerGo = Instantiate(ModAssets.Prefabs.sparkleCircle);
			markerGo.transform.SetParent(transform);
			markerGo.gameObject.SetActive(true);

			circleMarker = markerGo.transform;

			ModAssets.ConfigureSparkleCircle(markerGo, radius, markerColor);

			Mod.touchers.Add(this);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.touchers.Remove(this);
		}


		private void Update()
		{
			if (circleMarker != null)
			{
				var position = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
				position.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
				circleMarker.position = position;
			}
		}

		public void IgnoreCell(int cell)
		{
			alreadyVisitedCells?.Add(cell);
		}

		public void ReplaceElement(int cell, Element elementFrom, SimHashes elementId, bool useMassRatio = true, float massMultiplier = 1f, bool force = false)
		{
			if (!force && !GridUtil.IsCellFoundationEmpty(cell))
				return;

			var mass = Grid.Mass[cell];

			if (useMassRatio)
			{
				var elementTo = ElementLoader.FindElementByHash(elementId);

				var maxMassFrom = elementFrom.maxMass;
				var maxMassTo = elementTo.maxMass;

				mass = (mass / maxMassFrom) * maxMassTo;
				mass *= massMultiplier;
			}

			SimMessages.ReplaceElement(
				cell,
				elementId,
				toucherEvent,
				mass,
				Grid.Temperature[cell],
				Grid.DiseaseIdx[cell],
				Grid.DiseaseCount[cell]);
		}

		public void Sim33ms(float dt)
		{
			elapsedLifeTime += dt;

			if (elapsedLifeTime > lifeTime)
			{
				circleMarker.GetComponent<ParticleSystem>().Stop();
				Util.KDestroyGameObject(gameObject);
				return;
			}

			var position = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
			var cells = GetTilesInRadius(position, radius);
			transform.position = position;

			var worldIdx = this.GetMyWorldId();

			foreach (var cell in cells)
			{
				if (Grid.IsValidCellInWorld(cell, worldIdx))
				{
					if (alreadyVisitedCells.Contains(cell))
						return;

					if(UpdateCell(cell))
						alreadyVisitedCells.Add(cell);
				}
			}
		}

		private HashSet<int> GetTilesInRadius(Vector3 position, float radius)
		{
			cells.Clear();
			for (int i = 0; i < cellsPerUpdate; i++)
			{
				var offset = position + (Vector3)(UnityEngine.Random.insideUnitCircle * radius);
				cells.Add(Grid.PosToCell(offset));
			}

			return cells;
		}

		public void SpawnTile(int cell, string prefabId, Tag[] elements, float temperature)
		{
			var def = Assets.GetBuildingDef(prefabId);

			if (def == null)
				return;

			def.Build(cell, Orientation.Neutral, null, elements, temperature, false, GameClock.Instance.GetTime() + 1);
			World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, cell);
		}
	}
}
