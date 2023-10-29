/*using FUtility;
using ImGuiNET;
using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class PuzzleDoor : KMonoBehaviour, IImguiDebug
	{
		[MyCmpReq] private KSelectable kSelectable;

		[MySmiGet] private Door.Controller.Instance door;

		[Serialize] public bool isUnSealed;
		[Serialize] public Ref<Switch> targetRef;

		public static Switch debugTarget;
		public static PuzzleDoor debugDoor;

		private Switch target;
		private LineRenderer lineRenderer;
		private bool highlighted;

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (!isUnSealed)
			{
				if (targetRef != null)
				{
					target = targetRef.Get();
					SetTarget(target);
				}

				door.master.Seal();
			}


			Subscribe(ModEvents.OnHighlightApplied, OnHover);
			Subscribe(ModEvents.OnHighlightCleared, OnUnHover);

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
			Debug.Log($"setting target to {target?.name}");
			Debug.Log($"target null? {target == null}");
			Debug.Log($"isUnSealed? {isUnSealed}");

			if (target == null || isUnSealed)
				return;

			Debug.Log($"connection active");

			this.target = target;
			this.targetRef = new Ref<Switch>(target);

			target.OnToggle += OnTargetToggled;
			lineRenderer = ModAssets.DrawLine(transform.position, target.transform.position, new Color(0, 1, 0, 0.7f));

			if (!highlighted)
				lineRenderer.gameObject.SetActive(false);
		}

		private void OnTargetToggled(bool value)
		{
			if (value)
			{
				UnSeal();

				if (target != null)
					target.OnToggle -= OnTargetToggled;
			}
		}

		public void UnSeal()
		{
			Log.Debug("unsealed");

			door.GetComponent<Unsealable>().unsealed = true;

			door.sm.isLocked.Set(false, door);
			door.master.GetComponent<AccessControl>().controlEnabled = true;
			door.master.controlState = Door.ControlState.Opened;
			door.master.RefreshControlState();
			door.sm.isOpen.Set(true, door);
			door.sm.isLocked.Set(false, door);
			door.sm.isSealed.Set(false, door);

			door.GoTo(door.sm.Sealed.chore_pst);
			isUnSealed = true;

			target = null;
			targetRef = null;
			DestroyLineRenderer();
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
	}
}
*/