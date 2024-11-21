using KSerialization;
using System.Collections;
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

		public LampVariant CurrentVariant => ModDb.lampVariants.TryGet(currentVariantID);

		public KBatchedAnimController lampKbac;

		private KAnimLink link;

		private const string LIGHT_SYMBOL = "light_bloom";

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
			CreateLampController();
		}

		public bool HasTag(HashedString tag)
		{
			var data = CurrentVariant?.data;
			return data != null && LampVariant.HasTag(data, tag);
		}

		public override void OnSpawn()
		{
			lampKbac.transform.SetParent(transform);

			// roll a new one if there is nothing set yet
			var lampVariant = ModDb.lampVariants.TryGet(currentVariantID);

			if (currentVariantID.IsNullOrWhiteSpace() || lampVariant == null)
				SetRandom();

			light2D.IntensityAnimation = 1.5f;
			smi.StartSM();

			SetVariant(currentVariantID);
			UpdateFlip();

			StartCoroutine(UpdateNextFrame());
		}


		public IEnumerator UpdateNextFrame()
		{
			yield return SequenceUtil.WaitForNextFrame;

			SetVariant(currentVariantID);
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

			NotifyComponents(targetVariant, ModEvents.OnMoodlampChangedEarly);

			currentVariantID = targetVariant.Id;

			RefreshAnimation();
			kSelectable.SetName(targetVariant.Name);

			NotifyComponents(targetVariant, ModEvents.OnMoodlampChanged);

			if (kSelectable.IsSelected)
			{
				DetailsScreen.Instance.Refresh(gameObject);
				Game.Instance.userMenu.Refresh(gameObject);
			}
		}

		private void NotifyComponents(LampVariant targetVariant, int hash)
		{
			Log.Debug("notifying components");

			Trigger(hash, new Dictionary<HashedString, object>()
			{
				{"LampId", currentVariantID },
				{"Color", targetVariant.color},
				{"Tags", targetVariant.tags},
				{"Data", targetVariant.data},
			});
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
			{
				var anims = new[] { animFile };
				lampKbac.SwapAnims(anims);
			}

			var isOn = operational.IsOperational;

			if (!LampVariant.HasTag(variant.data, LampVariants.TAGS.TINTABLE))
				SetLightColor(variant.color);

			if (isOn)
			{
				lampKbac.Play("on", variant.mode);
			}
			else
			{
				lampKbac.Play("off", variant.mode);
			}

			kbac.SetSymbolVisiblity(LIGHT_SYMBOL, isOn);
			lampKbac.SetSymbolVisiblity("ui_placeholder", false);
			lampKbac.SetSymbolVisiblity("rotation_marker", false);

			link ??= new KAnimLink(kbac, lampKbac);

			lampKbac.flipX = rotatable.IsRotated;

			Trigger(ModEvents.OnLampRefreshedAnimation);
		}

		public void Rotate()
		{
			rotatable.Rotate();
			UpdateFlip();
		}

		private void UpdateFlip()
		{
			lampKbac.flipX = rotatable.IsRotated;
			lampKbac.SetDirty();
		}

		private void CreateLampController()
		{
			var go = new GameObject("DecorPackA_LampTop");

			go.SetActive(false);

			lampKbac = go.AddComponent<KBatchedAnimController>();
			lampKbac.animFiles = new[]
			{
				Assets.GetAnim("dpi_moodlamp_unicorn_kanim")
			};
			lampKbac.initialAnim = "off";

			go.transform.position = transform.position + lampOffset;
			go.transform.SetParent(transform.parent);

			go.SetActive(true);

			link = new KAnimLink(kbac, lampKbac);
		}

		public void SetLightColor(Color color)
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
