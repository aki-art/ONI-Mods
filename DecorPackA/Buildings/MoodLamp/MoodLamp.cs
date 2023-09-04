using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class MoodLamp : StateMachineComponent<MoodLamp.SMInstance>
	{
		[MyCmpReq] private readonly KBatchedAnimController kbac;
		[MyCmpReq] private readonly Operational operational;
		[MyCmpReq] private readonly Light2D light2D;
		[MyCmpReq] private readonly Rotatable rotatable;
		[MyCmpReq] private readonly KSelectable kSelectable;

		[Serialize] public string currentVariantID;

		[SerializeField] public Vector3 lampOffset;

		public KBatchedAnimController lampKbac;
		private KAnimLink link;
		private List<KMonoBehaviour> enabledComponents = new();

		private const string LIGHT_SYMBOL = "light_bloom";

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
		}

		public override void OnSpawn()
		{
			// roll a new one if there is nothing set yet
			var lampVariant = ModDb.lampVariants.TryGet(currentVariantID);

			if (currentVariantID.IsNullOrWhiteSpace() || lampVariant == null)
				lampVariant = SetRandom();

			CreateLampController();

			light2D.IntensityAnimation = 1.5f;
			smi.StartSM();

			SetVariant(currentVariantID);

			Subscribe((int)GameHashes.Rotated, OnRotated);
		}

		private void OnRotated(object obj)
		{
			lampKbac.flipX = rotatable.IsRotated;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			link.Unregister();
			Util.KDestroyGameObject(lampKbac.gameObject);
		}

		public void SetVariant(string targetVariant)
		{
			SetVariant(ModDb.lampVariants.TryGet(targetVariant));
		}

		public void SetVariant(LampVariant targetVariant)
		{
			if (targetVariant == null)
			{
				Log.Warning("Invalid lamp variant");
				return;
			}

			currentVariantID = targetVariant.Id;

			RefreshAnimation();
			RefreshComponents(targetVariant);
			kSelectable.SetName(targetVariant.Name);

			Trigger(ModEvents.OnMoodlampChanged, targetVariant.Id);

			if (kSelectable.IsSelected)
				Trigger((int)GameHashes.RefreshUserMenu);
		}

		private void RefreshComponents(LampVariant targetVariant)
		{
			foreach (var cmp in enabledComponents)
			{
				if (cmp != null)
					cmp.enabled = false;
			}

			enabledComponents.Clear();

			if (targetVariant.componentTypes != null)
			{
				foreach (var componentType in targetVariant.componentTypes)
				{
					var cmp = gameObject.GetComponent(componentType) as KMonoBehaviour;
					if (cmp != null)
					{
						cmp.enabled = true;
						enabledComponents.Add(cmp);
					}
					else
						enabledComponents.Add(gameObject.AddComponent(componentType) as KMonoBehaviour);
				}
			}
		}

		public LampVariant SetRandom()
		{
			var targetVariant = ModDb.lampVariants.GetRandom();
			SetVariant(targetVariant);

			return targetVariant;
		}

		private void OnCopySettings(object obj)
		{
			if (((GameObject)obj).TryGetComponent(out MoodLamp moodLamp))
				SetVariant(moodLamp.currentVariantID);
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

			link ??= new KAnimLink(kbac, lampKbac);

			lampKbac.flipX = rotatable.IsRotated;
		}

		public void Rotate()
		{
			rotatable.Rotate();
			lampKbac.flipX = rotatable.IsRotated;
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
