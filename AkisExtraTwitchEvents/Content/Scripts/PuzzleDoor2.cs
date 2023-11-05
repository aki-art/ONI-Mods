using FUtility;
using ImGuiNET;
using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class PuzzleDoor2 : KMonoBehaviour, IImguiDebug
	{
		[MyCmpReq] private Building building;
		[MyCmpReq] private PrimaryElement primaryElement;
		[MyCmpReq] private KSelectable kSelectable;
		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private Deconstructable deconstructable;

		[Serialize] public Ref<Switch> targetRef;
		[Serialize] public bool isSolved;

		private static readonly byte simProperties =
			(byte)(Sim.Cell.Properties.GasImpermeable
			| Sim.Cell.Properties.LiquidImpermeable
			| Sim.Cell.Properties.SolidImpermeable
			| Sim.Cell.Properties.Unbreakable
			| Sim.Cell.Properties.Opaque
			| Sim.Cell.Properties.NotifyOnMelt);

		public static Switch debugTarget;
		public static PuzzleDoor2 debugDoor;

		private Switch target;
		private LineRenderer lineRenderer;
		private bool highlighted;

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (!isSolved)
			{
				if (targetRef != null)
				{
					target = targetRef.Get();
					SetTarget(target);
				}
			}
			else
			{
				kbac.Play("open");
			}

			kSelectable.SetStatusItem(Db.Get().StatusItemCategories.Main, TStatusItems.PuzzleDoorStatus, this);
			Subscribe(ModEvents.OnHighlightApplied, OnHover);
			Subscribe(ModEvents.OnHighlightCleared, OnUnHover);

			deconstructable.allowDeconstruction = deconstructable.constructionElements != null
				&& deconstructable.constructionElements.Length > 0
				&& deconstructable.constructionElements[0] != SimHashes.Unobtanium.CreateTag();
		}

		private void OnUnHover(object _)
		{
			highlighted = false;
			if (lineRenderer != null)
				lineRenderer.gameObject.SetActive(false);
		}

		private void OnHover(object _)
		{
			highlighted = true;
			if (lineRenderer != null)
				lineRenderer.gameObject.SetActive(true);
		}

		public void SetTarget(Switch target)
		{
			if (target == null || isSolved)
				return;

			this.target = target;
			targetRef = new Ref<Switch>(target);

			target.OnToggle += OnTargetToggled;
			lineRenderer = ModAssets.DrawLine(
				transform.position + new Vector3(0.5f, 1f),
				target.transform.position + new Vector3(0.5f, 0.5f),
				new Color(0, 1, 0, 0.7f));

			if (!highlighted)
				lineRenderer.gameObject.SetActive(false);

			target.GetComponent<KPrefabID>().AddTag(TTags.disableUserScreen, false);

			Close();
		}

		private void OnTargetToggled(bool value)
		{
			if (value)
			{
				if (target != null)
					target.OnToggle -= OnTargetToggled;

				Solve();
			}
		}

		public void Close()
		{
			ConvertToSolid();
			deconstructable.allowDeconstruction = false;
			Game.Instance.userMenu.Refresh(gameObject);
		}

		public void Solve()
		{
			Open();
			DestroyLineRenderer();

			target = null;
			targetRef = null;

			if (isSolved)
				return;

			isSolved = true;

			AudioUtil.PlaySound(ModAssets.Sounds.VICTORY, transform.position, ModAssets.GetSFXVolume());
		}

		public void Open()
		{
			ConvertToVacuum();
			kbac.Queue("open_pre", KAnim.PlayMode.Once);
			kbac.Queue("open", KAnim.PlayMode.Paused);

			deconstructable.allowDeconstruction = deconstructable.constructionElements != null
				&& deconstructable.constructionElements.Length > 0
				&& deconstructable.constructionElements[0] != SimHashes.Unobtanium.CreateTag();

			Game.Instance.userMenu.Refresh(gameObject);
		}

		private void DestroyLineRenderer()
		{
			if (lineRenderer != null)
				Util.KDestroyGameObject(lineRenderer);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			DestroyLineRenderer();
		}

		public void OnImgui()
		{
			if (debugDoor == null)
			{
				if (ImGui.Button("connect"))
				{
					if (debugTarget != null)
					{
						SetTarget(debugTarget);
						debugTarget = null;
						debugDoor = null;
					}
					else
					{
						debugDoor = this;
					}
				}
			}
			else if (debugDoor == this && ImGui.Button("disconnect"))
			{
				debugDoor = null;
			}
		}

		private void ConvertToSolid()
		{
			if (building == null)
				Log.Debug("building is null");

			building.RunOnArea(num =>
			{
				SimMessages.ReplaceAndDisplaceElement(
					num,
					primaryElement.ElementID,
					CellEventLogger.Instance.SimCellOccupierOnSpawn,
					primaryElement.Mass,
					primaryElement.Temperature);

				Grid.Objects[num, (int)ObjectLayer.FoundationTile] = gameObject;
				Grid.Foundation[num] = true;
				Grid.SetSolid(num, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
				SimMessages.SetCellProperties(num, simProperties);
				Grid.RenderedByWorld[num] = false;
				World.Instance.OnSolidChanged(num);
				GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
			});
		}

		private void ConvertToVacuum()
		{
			building.RunOnArea(cell =>
			{
				SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0.0f);
				Grid.Objects[cell, (int)ObjectLayer.FoundationTile] = null;
				Grid.Foundation[cell] = false;
				Grid.SetSolid(cell, false, CellEventLogger.Instance.SimCellOccupierDestroy);
				SimMessages.ClearCellProperties(cell, simProperties);

				Grid.RenderedByWorld[cell] = true;
				World.Instance.OnSolidChanged(cell);
				GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.solidChangedLayer, null);
			});
		}
	}
}
