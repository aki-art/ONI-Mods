using Harmony;
using System;

namespace StripDoor
{
	partial class StripDoor : StateMachineComponent<StripDoor.SMInstance>
	{
		private const float ANIMATION_COOLDOWN = .5f;

		private Door door;

		private StripDoorReactable reactable;
		private KBatchedAnimController overlay;
		private string swooshSound;

		protected override void OnSpawn()
		{
			door = GetComponent<Door>();
			overlay = CreateOverlayAnim();
			swooshSound = GlobalAssets.GetSound("drecko_ruffle_scales_short");

			base.OnSpawn();
			smi.StartSM();

			RefreshReactable(true);

		}
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			ClearReactable();
		}

		protected KBatchedAnimController CreateOverlayAnim()
		{
			Grid.SceneLayer overlayLayer = Grid.SceneLayer.Ground;
			KBatchedAnimController effect = FXHelpers.CreateEffect("stripdooroverlay_kanim", transform.position, transform);
			effect.destroyOnAnimComplete = false;
			effect.fgLayer = overlayLayer;
			effect.SetSceneLayer(overlayLayer);
			effect.defaultAnim = "closed";

			return effect;
		}
		private void FlutterStrips(string anim)
		{
			Debug.Log("Fluttering strips: " + anim);
			PlaySwooshSound();
			overlay.Play(anim);
			overlay.Queue("closed");
		}

		private void RefreshReactable(bool doorAutoState)
		{
			if (doorAutoState)
			{
				CreateNewReactable();
				return;
			}
			ClearReactable();
		}

		private void CreateNewReactable()
		{
			if (reactable == null) reactable = new StripDoorReactable(this);
		}

		private void OrphanReactable() => reactable = null;

		private void ClearReactable()
		{
			if (reactable != null)
			{
				reactable.Cleanup();
				reactable = null;
			}
		}

		private void PlaySwooshSound() =>
			SoundEvent.EndOneShot(
				SoundEvent.BeginOneShot(swooshSound, transform.position, 2f, SoundEvent.ObjectIsSelectedAndVisible(gameObject)));

		public class States : GameStateMachine<States, SMInstance, StripDoor>
		{
#pragma warning disable 649
			public State locked;
			public State open;
			public State auto;
#pragma warning restore 649

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = auto;

				root
					.Update((smi, dt) => { smi.master.RefreshState(); }, UpdateRate.SIM_200ms);
				auto
					.Enter(smi => smi.master.ResetDoor());
				open
					.Enter(smi => smi.master.OpenDoor())
					.Exit(smi => smi.master.overlay.Play("permanentOpenPst"));
				locked
					.Enter(smi => smi.master.LockDoor())
					.Exit(smi => smi.master.overlay.Play("lockedPst"));
			}
		}

		private void ResetDoor()
		{
			smi.master.RefreshReactable(true);
			smi.master.overlay.Queue("closed");
		}

		private void OpenDoor()
		{
			smi.master.RefreshReactable(false);
			smi.master.overlay.Play("permanentOpenPre");
			smi.master.overlay.Queue("permanentOpen");
		}

		private void LockDoor()
		{
			smi.master.RefreshReactable(false);
			smi.master.overlay.Play("lockedPre");
			smi.master.overlay.Queue("locked");
		}

		private void RefreshState()
		{
			switch (smi.master.door.CurrentState)
			{
				case Door.ControlState.Opened:
					smi.GoTo(smi.sm.open);
					break;
				case Door.ControlState.Locked:
					smi.GoTo(smi.sm.locked);
					break;
				case Door.ControlState.Auto:
				default:
					smi.GoTo(smi.sm.auto);
					break;
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, StripDoor, object>.GameInstance
		{
			public SMInstance(StripDoor master) : base(master) { }
		}
	}
}
