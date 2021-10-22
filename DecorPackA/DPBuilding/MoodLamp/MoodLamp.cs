using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DecorPackA.DPBuilding.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	class MoodLamp : StateMachineComponent<MoodLamp.SMInstance>
	{
		[Serialize]
		private string currentVariant;

		[MyCmpReq]
		private KBatchedAnimController animController;

		[SerializeField]
		public Dictionary<string, Color> variants;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
			variants = new Dictionary<string, Color>
			{
				{ "unicorn", new Color(2.25f, 0, 2.13f, 1f) },
				{ "morb", new Color(.27f, 2.55f, .08f, 1f) },
				{ "dense", new Color(0.07f, 0.98f, 3.35f, 1f) },
				{ "moon", new Color(1.09f, 1.25f, 1.94f, 1f) },
				{ "brothgar", new Color(2.47f, 1.75f, .62f, 1f) }
			};
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();

			if (currentVariant.IsNullOrWhiteSpace() || !variants.ContainsKey(currentVariant))
			{
				var newIdx = Random.Range(0, variants.Count - 1);
				currentVariant = variants.ElementAt(newIdx).Key;
			}

			smi.StartSM();
		}

		private void OnCopySettings(object obj)
		{
			var lamp = ((GameObject)obj).GetComponent<MoodLamp>();
			if (lamp != null)
            {
                SetVariant(lamp.currentVariant);
            }
        }

		internal void SetVariant(string targetVariant)
		{
			currentVariant = targetVariant;
			string suffix = GetComponent<Operational>().IsOperational ? "_on" : "_off";
			animController.Play(currentVariant + suffix);
			GetComponent<Light2D>().Color = variants[currentVariant];
		}

		public class States : GameStateMachine<States, SMInstance, MoodLamp>
		{
			public State off;
			public State on;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = off;
				off
					.PlayAnim(smi => smi.master.currentVariant + "_off", KAnim.PlayMode.Paused)
					.EventTransition(GameHashes.OperationalChanged, on, smi => smi.GetComponent<Operational>().IsOperational);
				on
					.PlayAnim(smi => smi.master.currentVariant + "_on", KAnim.PlayMode.Paused)
					.Enter("RefreshColor", smi => smi.GetComponent<Light2D>().Color = smi.master.variants[smi.master.currentVariant])
					.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational)
					.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null)
					.Enter("SetActive", smi => smi.GetComponent<Operational>().SetActive(true));
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, MoodLamp, object>.GameInstance
		{
			public SMInstance(MoodLamp master) : base(master) { }
		}
	}
}
