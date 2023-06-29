using Buildings.MoodLamp;
using KSerialization;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class MoodLamp : StateMachineComponent<MoodLamp.SMInstance>
	{
		[MyCmpReq] private readonly KBatchedAnimController kbac;
		[MyCmpReq] private readonly Operational operational;

		[MyCmpReq] private readonly Light2D light2D;
		[MyCmpReq] private readonly Hamis hamis;

		[Serialize] public string currentVariantID;

		[SerializeField] public Vector3 lampOffset;

		private KBatchedAnimController lampKbac;
		private KAnimLink link;

		private const string LIGHT_SYMBOL = "light_bloom";

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

			CreateLampController();

			light2D.IntensityAnimation = 1.5f;
			smi.StartSM();
		}

		private void CreateLampController()
		{
			var go = new GameObject("lamp top");

			go.SetActive(false);

			lampKbac = go.AddComponent<KBatchedAnimController>();
			lampKbac.animFiles = new[] 
			{ 
				Assets.GetAnim("dpi_moodlamp_unicorn_kanim") 
			};
			lampKbac.initialAnim = "off";

			go.transform.position = transform.position + lampOffset;
			go.transform.parent = transform.parent;

			go.SetActive(true);

			link = new KAnimLink(kbac, lampKbac);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			link.Unregister();
			Util.KDestroyGameObject(lampKbac.gameObject);
		}

		public void SetRandom() => SetVariant(ModDb.lampVariants.GetRandom());

		private void OnCopySettings(object obj)
		{
			if (((GameObject)obj).TryGetComponent(out MoodLamp moodLamp))
				SetVariant(moodLamp.currentVariantID);
		}

		public void SetVariant(string targetVariant)
		{
			var variant = ModDb.lampVariants.TryGet(targetVariant);
			if (variant != null)
				SetVariant(variant);
		}

		public void SetVariant(LampVariant targetVariant)
		{
			currentVariantID = targetVariant.Id;
			RefreshAnimation();

			Trigger(ModEvents.OnMoodlampChanged, targetVariant.Id);
		}

		public void RefreshAnimation()
		{
			if (lampKbac == null)
				return;

			var variant = ModDb.lampVariants.TryGet(currentVariantID);

			if (variant == null)
				return;

			if (Assets.TryGetAnim(variant.kAnimFile, out var animFile))
				lampKbac.SwapAnims(new[] { animFile });

			var isOn = operational.IsOperational;

			if (isOn)
			{
				lampKbac.Play("on", variant.mode);
				kbac.SetSymbolTint(LIGHT_SYMBOL, variant.color);
				light2D.Color = variant.color;
			}
			else
				lampKbac.Play("off");

			kbac.SetSymbolVisiblity(LIGHT_SYMBOL, isOn);

			UpdateHamis();
			//UpdateGlitterpuft(variant);
			//UpdateShiftyLights(variant);

			link ??= new KAnimLink(kbac, lampKbac);
		}

/*		private void UpdateShiftyLights(LampVariant variant)
		{
			var shifty = gameObject.AddOrGet<ShiftyLight2D>();
			shifty.enabled = variant.shifty;

			if (variant.shifty)
			{
				shifty.color1 = variant.color;
				shifty.color2 = variant.color2;
				shifty.duration = variant.shiftDuration;
			}
		}*/
/*
		private void UpdateGlitterpuft(LampVariant variant)
		{
			gameObject.AddOrGet<GlitterLight2D>().enabled = variant.rainbowLights;
		}
*/
		private void UpdateHamis()
		{
			if (currentVariantID == Hamis.HAMIS_ID)
				hamis.RefreshSymbols();
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
