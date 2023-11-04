using System.Collections.Generic;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public abstract class Toucher : KMonoBehaviour, ISim33ms
	{
		[SerializeField] public float lifeTime;
		[SerializeField] public float radius;
		[SerializeField] public int cellsPerUpdate;
		[SerializeField] public Color markerColor;
		private GameObject sparklePoof;

		public static CellElementEvent toucherEvent = new("AETE_ToucherCellEvent", "Touch event spawn", false);

		private HashSet<int> alreadyVisitedCells;
		private float elapsedLifeTime = 0;
		private Transform circleMarker;
		private HashSet<int> cells;
		public bool paused;

		public abstract bool UpdateCell(int cell, float dt);

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

			var markerGo = CreateMarker();
			markerGo.transform.SetParent(transform);
			markerGo.gameObject.SetActive(true);

			circleMarker = markerGo.transform;

			sparklePoof = Instantiate(ModAssets.Prefabs.sparklePoof);
			var particles = sparklePoof.GetComponent<ParticleSystem>().main;
			particles.startColor = markerColor;


			Mod.touchers.Add(this);
		}

		protected virtual GameObject CreateMarker()
		{
			var marker = Instantiate(ModAssets.Prefabs.sparkleCircle);
			ModAssets.ConfigureSparkleCircle(marker, radius, markerColor);

			return marker;
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

		public virtual void SpawnFeedbackAnimation(int cell)
		{
			var pos = Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront);
			var go = Instantiate(ModAssets.Prefabs.sparklePoof);
			go.transform.position = pos;
			go.SetActive(true);
		}

		public void ReplaceElement(int cell, Element elementFrom, SimHashes elementId, bool useMassRatio = true, float massMultiplier = 1f, bool force = false, float? tempOverride = null)
		{
			if (AGridUtil.ReplaceElement(cell, elementFrom, elementId, useMassRatio, massMultiplier, force, tempOverride))
				SpawnFeedbackAnimation(cell);
		}

		public void Sim33ms(float dt)
		{
			if (paused) return;

			elapsedLifeTime += dt;

			if (elapsedLifeTime > lifeTime)
			{
				Destroy(circleMarker.gameObject);
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

					if (UpdateCell(cell, dt))
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
