using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP;

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

		public LampVariant CurrentVariant => ModDb.lampVariants.TryGet(currentVariantID);

		public KBatchedAnimController lampKbac;
		public KBatchedAnimController secondaryLampKbac;

		private KAnimLink link;
		private KAnimLink secondaryLink;

		private List<KMonoBehaviour> enabledComponents = new();

		private const string LIGHT_SYMBOL = "light_bloom";

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
			CreateLampController();
		}

		public override void OnSpawn()
		{
			// roll a new one if there is nothing set yet
			var lampVariant = ModDb.lampVariants.TryGet(currentVariantID);

			if (currentVariantID.IsNullOrWhiteSpace() || lampVariant == null)
				lampVariant = SetRandom();

			light2D.IntensityAnimation = 1.5f;
			smi.StartSM();

			SetVariant(currentVariantID);

			Subscribe((int)GameHashes.Rotated, OnRotated);
		}

		private void OnRotated(object obj)
		{
			Log.Debuglog("ON ROTATED" + rotatable.IsRotated);
			lampKbac.flipX = rotatable.IsRotated;

			if (secondaryLampKbac != null)
				secondaryLampKbac.flipX = rotatable.IsRotated;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();

			link.Unregister();
			Util.KDestroyGameObject(lampKbac.gameObject);

			if (secondaryLampKbac != null)
			{
				secondaryLink.Unregister();
				Util.KDestroyGameObject(secondaryLampKbac.gameObject);
			}
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

			if (secondaryLampKbac != null)
				secondaryLampKbac.gameObject.SetActive(false);

			RefreshAnimation();
			RefreshComponents(targetVariant);
			kSelectable.SetName(targetVariant.Name);
			Trigger(ModEvents.OnMoodlampChanged, targetVariant.Id);

			if (kSelectable.IsSelected)
			{
				DetailsScreen.Instance.Refresh(gameObject);
				Game.Instance.userMenu.Refresh(gameObject);
			}
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
			var targetVariant = ModDb.lampVariants.Get("arrow"); // ModDb.lampVariants.GetRandom();
			SetVariant(targetVariant);

			return targetVariant;
		}

		private void OnCopySettings(object obj)
		{
			if (((GameObject)obj).TryGetComponent(out MoodLamp moodLamp))
				SetVariant(moodLamp.currentVariantID);
		}

		public KBatchedAnimController UseSecondaryKbac(KAnim.PlayMode mode = KAnim.PlayMode.Paused)
		{
			if (secondaryLampKbac == null)
				secondaryLampKbac = CreateSecondaryKbac();

			secondaryLampKbac.gameObject.SetActive(true);
			secondaryLampKbac.Play(operational.IsOperational ? "secondary_on" : "secondary_off", mode);

			return secondaryLampKbac;
		}

		private KBatchedAnimController CreateSecondaryKbac()
		{
			var name = "DecorPackI_Moodlamp_Secondary_Overlay";
			var go = new GameObject(name);
			go.SetActive(false);
			go.transform.parent = lampKbac.transform;
			go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);

			var secondaryKbac = go.AddComponent<KBatchedAnimController>();
			secondaryKbac.AnimFiles = new[]
			{
				lampKbac.AnimFiles[0]
			};
			secondaryKbac.initialAnim = "secondary_off";
			secondaryKbac.isMovable = true;
			secondaryKbac.sceneLayer = Grid.SceneLayer.BuildingFront;

			go.SetActive(true);

			secondaryLink = new KAnimLink(kbac, secondaryKbac);
			return secondaryKbac;
		}

		public void RefreshAnimation()
		{
			if (lampKbac == null)
				return;

			var variant = ModDb.lampVariants.TryGet(currentVariantID);

			if (variant == null)
				return;

			if (Assets.TryGetAnim(variant.kAnimFile, out var animFile))
			{
				var anims = new[] { animFile };
				lampKbac.SwapAnims(anims);

				if (secondaryLampKbac != null)
					secondaryLampKbac.SwapAnims(anims);
			}

			var isOn = operational.IsOperational;

			SetLightColor(variant.color);

			if (isOn)
			{
				lampKbac.Play("on", variant.mode);

				if (secondaryLampKbac != null)
					secondaryLampKbac.Play("secondary_on");
			}
			else
			{
				lampKbac.Play("off");

				if (secondaryLampKbac != null)
					secondaryLampKbac.Play("secondary_off");
			}

			kbac.SetSymbolVisiblity(LIGHT_SYMBOL, isOn);
			lampKbac.SetSymbolVisiblity("ui_placeholder", false);
			lampKbac.SetSymbolVisiblity("rotation_marker", false);

			link ??= new KAnimLink(kbac, lampKbac);

			lampKbac.flipX = rotatable.IsRotated;

			Trigger(ModEvents.OnLampRefresh);
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

		internal void SetLightColor(Color color)
		{
			kbac.SetSymbolTint(LIGHT_SYMBOL, color);
			light2D.Color = color;
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
