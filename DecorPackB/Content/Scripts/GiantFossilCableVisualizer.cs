using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackB.Content.Scripts
{
	public class GiantFossilCableVisualizer : KMonoBehaviour, ISim4000ms, IRender200ms
	{
		public const int MAX_CABLE_LENGTH = 16;
		public const int MAX_CABLE_COUNT = 8;
		public static readonly HashedString cableOriginMarker = "cableorigin_marker";

		[SerializeField] public GameObject linePrefab;
		[SerializeField] public bool updatePositionEveryFrame;
		[SerializeField] public Color color;
		[SerializeField] public string anim;

		[MyCmpGet] public FoundationOrAnchorMonitor monitor;

		private Building building;
		private KBatchedAnimController kbac;

		public List<Vector3> startPoints;
		public List<Cable> cables;
		public int width;
		public Vector3 previousPosition;

		public float z;
		public bool isHangable;
		public static bool isActivePreviewHangable;
		public Text[] debugTexts = new Text[7];
		public float[] ceilingDistances = new float[7];

		public override void OnSpawn()
		{
			base.OnSpawn();

			kbac = transform.parent.GetComponent<KBatchedAnimController>();
			building = transform.parent.GetComponent<Building>();

			z = Grid.GetLayerZ(Grid.SceneLayer.Front);

			transform.parent.gameObject.Subscribe((int)DPIIHashes.FossilStageSet, _ => ArtStageChanged());
			ArtStageChanged();

			if (!updatePositionEveryFrame)
				SimAndRenderScheduler.instance.render200ms.Remove(this);

			if (updatePositionEveryFrame || !isHangable)
				SimAndRenderScheduler.instance.sim4000ms.Remove(this);

			SetCableColor(color);
			cables = new();
		}

		public bool IsGrounded()
		{
			return BuildingDef.CheckFoundation(Grid.PosToCell(this), building.Orientation, BuildLocationRule.OnFloor, building.Def.WidthInCells, building.Def.HeightInCells);
		}

		public bool IsHangable()
		{
			if (!isHangable)
				return false;

			for (int i = 0; i < startPoints.Count; i++)
			{
				var ceilingDistance = GetCeilingDistance(Mathf.FloorToInt(startPoints[i].x));

				if (ceilingDistance < 0 || ceilingDistance > MAX_CABLE_LENGTH)
					return false;
			}

			return true;
		}

		private void SetCableColor(Color color)
		{
			if (cables == null)
				return;

			foreach (var cable in cables)
			{
				var renderer = cable.lineRenderer;
				if (renderer != null)
					renderer.startColor = renderer.endColor = color;
			}
		}

		private float GetCeilingDistance(int x)
		{
			Grid.PosToXY(transform.position, out var xo, out var yo);
			for (var y = building.Def.HeightInCells; y < MAX_CABLE_LENGTH; y++)
			{
				var cell = Grid.XYToCell(x + xo, y + yo);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell))
					return y;
			}

			return -1;
		}

		public void CollectMarkers()
		{
			startPoints = new List<Vector3>();

			var batch = kbac.GetBatch();

			if (batch != null)
			{
				var frame = batch.group.data.GetAnimFrames()[0];
				for (var frameElementIdx = 0; frameElementIdx < frame.numElements; ++frameElementIdx)
				{
					var offsetIdx = frame.firstElementIdx + frameElementIdx;
					if (offsetIdx < batch.group.data.frameElements.Count)
					{
						var frameElement = batch.group.data.frameElements[offsetIdx];
						if (frameElement.symbol == cableOriginMarker)
						{
							var vector3 = (kbac.GetTransformMatrix() * frameElement.transform)
								.MultiplyPoint(Vector3.zero) - transform.GetPosition();

							startPoints.Add(vector3 with { z = z });
						}
					}
				}
			}
		}

		private void SetupLines()
		{
			cables = new List<Cable>();

			for (int i = 0; i < startPoints.Count; i++)
			{
				var lineRenderer = Instantiate(linePrefab).GetComponent<LineRenderer>();
				lineRenderer.transform.position = transform.position;
				lineRenderer.transform.parent = transform;
				lineRenderer.name = $"cable {i}";
				lineRenderer.startColor = lineRenderer.endColor = updatePositionEveryFrame ? Color.white : Color.black;
				lineRenderer.gameObject.SetActive(true);
				cables.Add(new Cable(lineRenderer, startPoints[i]));
			}
		}

		public void Draw()
		{
			if (previousPosition == transform.position)
				return;

			isActivePreviewHangable = true;

			for (int i = 0; i < ceilingDistances.Length; i++)
			{
				var ceilingDistance = GetCeilingDistance(i);
				var previousDistance = ceilingDistances[i];

				if (!Mathf.Approximately(previousDistance, ceilingDistance))
					UpdateColumn(i, ceilingDistance);

				if (ceilingDistance < 0 || ceilingDistance > MAX_CABLE_LENGTH)
					isActivePreviewHangable = false;
			}

			ToggleLines(isActivePreviewHangable);
			previousPosition = transform.position;
		}

		private void UpdateColumn(int i, float distance)
		{
			foreach (var cable in cables)
			{
				if (cable == null || cable.column != i)
					continue;

				var lineRenderer = cable.lineRenderer;
				var position = new Vector3(cable.x, distance, z);
				lineRenderer.SetPosition(1, position);

				ceilingDistances[i] = distance;

				if (monitor != null)
					monitor.RecalculateCeilingCells(ceilingDistances);

				if (Mod.DebugMode)
				{
					if (debugTexts[i] != null)
						Destroy(debugTexts[i].gameObject);

					debugTexts[i] = ModAssets.AddText(transform.position + position, Color.yellow, distance.ToString());
				}
			}
		}

		private void ToggleLines(bool enabled)
		{
			foreach (var cable in cables)
			{
				if (cable.lineRenderer != null)
					cable.lineRenderer.enabled = enabled;
			}
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			DestroyCables();
		}

		private void DestroyCables()
		{
			if (cables == null)
				return;

			for (int i = 0; i < cables.Count; i++)
				cables[i]?.Destroy();
		}

		public void ArtStageChanged()
		{
			DestroyCables();
			CollectMarkers();
			kbac.SetSymbolVisiblity(cableOriginMarker, false);

			// not hangable
			isHangable = startPoints != null && startPoints.Count > 0;
			var go = transform.parent.gameObject;

			if (isHangable && GameComps.RequiresFoundations.Has(go))
				GameComps.RequiresFoundations.Remove(go);

			if (!isHangable)
			{
				if (!GameComps.RequiresFoundations.Has(go))
					GameComps.RequiresFoundations.Add(go);

				return;
			}

			width = building.Def.WidthInCells;
			SetupLines();
			Draw();
		}

		public void Sim4000ms(float dt)
		{
			if (!updatePositionEveryFrame)
				return;

			Draw();
		}

		public void Render200ms(float dt)
		{
			Draw();
		}

		public class Cable
		{
			public LineRenderer lineRenderer;
			public Vector3 startPoint;
			public int column;
			public float x;

			public Cable(LineRenderer lineRenderer, Vector3 startPoint)
			{
				this.lineRenderer = lineRenderer;
				this.startPoint = startPoint;

				column = (int)(startPoint.x / 100f);
				column = Mathf.Clamp(column, 0, 6);
				x = startPoint.x;
			}

			public void Destroy()
			{
				if (lineRenderer != null)
					UnityEngine.Object.Destroy(lineRenderer);
			}
		}
	}
}