using Buildings.MoodLamp;
using KSerialization;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class MoodLamp : StateMachineComponent<MoodLamp.SMInstance>
	{
		[MyCmpReq]
		private readonly KBatchedAnimController kbac;

		[MyCmpReq]
		private readonly Operational operational;

		[MyCmpReq] private readonly Light2D light2D;
		[MyCmpReq] private readonly Hamis hamis;

		[Serialize] public string currentVariantID;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
		}

		public override void OnSpawn()
		{
			// roll a new one if there is nothing set yet
			if (currentVariantID.IsNullOrWhiteSpace() || ModDb.lampVariants.TryGet(currentVariantID) == null)
				SetRandom();

			light2D.IntensityAnimation = 1.5f;
			smi.StartSM();
		}

		// gives a randomly selected key of a variant
		public void SetRandom()
		{
			SetVariant(ModDb.lampVariants.GetRandom());
		}

		// copy the selected lamp type when the user copies building settings
		private void OnCopySettings(object obj)
		{
			if (((GameObject)obj).TryGetComponent(out MoodLamp moodLamp))
				SetVariant(moodLamp.currentVariantID);
		}

		internal void SetVariant(LampVariant targetVariant)
		{
			currentVariantID = targetVariant.Id;
			RefreshAnimation();
		}

		internal void SetVariant(string targetVariant)
		{
			var variant = ModDb.lampVariants.TryGet(targetVariant);
			if (variant != null)
			{
				SetVariant(variant);
			}
		}

		public void RefreshAnimation()
		{
			var variant = ModDb.lampVariants.TryGet(currentVariantID);
			if (variant == null)
				return;

			if (operational.IsOperational)
			{
				if (currentVariantID == Hamis.HAMIS_ID)
					kbac.Play(hamis.GetAnimOverride(true), variant.mode);
				else
					kbac.Play(variant.on, variant.mode);

				light2D.Color = variant.color;
			}
			else
			{
				if (currentVariantID == Hamis.HAMIS_ID)
					kbac.Play(hamis.GetAnimOverride(false), variant.mode);
				else
					kbac.Play(variant.off);
			}

			gameObject.AddOrGet<GlitterLight2D>().enabled = variant.rainbowLights;

			var shifty = gameObject.AddOrGet<ShiftyLight2D>();
			shifty.enabled = variant.shifty;
			if (variant.shifty)
			{
				shifty.color1 = variant.color;
				shifty.color2 = variant.color2;
				shifty.duration = variant.shiftDuration;
			}
		}

		public class States : GameStateMachine<States, SMInstance, MoodLamp>
		{
			public State off;
			public State on;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = off;

				off
					.Enter("SetInactive", smi => smi.master.RefreshAnimation())
					.EventTransition(GameHashes.OperationalChanged, on, smi => smi.GetComponent<Operational>().IsOperational);
				on
					.Enter("SetActive", smi =>
					{
						smi.GetComponent<Operational>().SetActive(true);
						smi.master.RefreshAnimation();
					})
					.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational)
					.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null);
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, MoodLamp, object>.GameInstance
		{
			public SMInstance(MoodLamp master) : base(master) { }
		}
	}
}
