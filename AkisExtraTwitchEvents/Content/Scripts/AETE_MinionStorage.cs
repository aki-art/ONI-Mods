using FUtility;
using ImGuiNET;
using Klei.AI;
using KSerialization;
using System;
using System.Runtime.Serialization;
using Twitchery.Content.Defs.Critters;
using UnityEngine;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AETE_MinionStorage : KMonoBehaviour, IImguiDebug
	{
		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private Effects effects;
		[MyCmpReq] private MinionIdentity identity;

		[Serialize] private bool isDoubleTroubleDupe;
		[Serialize] private bool isWereVole;
		[Serialize] private bool hasHealedHulk;
		[Serialize] public float wereVoleSince;
		private bool debugForceWereVole;

		public bool IsWereVole => isWereVole;

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (effects.HasEffect(TEffects.DOUBLETROUBLE))
				kbac.TintColour = new Color(1, 1, 1, 0.5f);

			Subscribe((int)GameHashes.EffectRemoved, OnEffectRemoved);

#if WIP_EVENTS
			GameClock.Instance.Subscribe((int)GameHashes.Nighttime, OnNight);

			if (GameClock.Instance.IsNighttime())
				OnNight(null);
#endif
			if(!hasHealedHulk)
			{
				var health = GetComponent<Health>();
				health.hitPoints = health.maxHitPoints;

				hasHealedHulk = true;
			}
		}

#if WIP_EVENTS
		public void BecomeWereVole()
		{
			if (isWereVole)
				return;

			wereVoleSince = GameClock.Instance.GetTime();
			isWereVole = true;
			debugForceWereVole = true;

			var trait = Db.Get().traits.Get(TTraits.WEREVOLE);
			TryGetComponent(out Traits traits);

			if (!traits.HasTrait(TTraits.WEREVOLE))
				traits.Add(trait);
		}

		public void CureWereVole()
		{
			if (!isWereVole)
				return;

			isWereVole = false;
			debugForceWereVole = false;

			var trait = Db.Get().traits.Get(TTraits.WEREVOLE);
			TryGetComponent(out Traits traits);

			if (traits.HasTrait(TTraits.WEREVOLE))
				traits.Remove(trait);
		}

		private void OnNight(object obj)
		{
			if (isWereVole)
			{
				var voleGo = FUtility.Utils.Spawn(WereVoleConfig.ID, gameObject);
				voleGo.GetComponent<WereVoleContainer>().FromMinion(this);
			}
		}
#endif

		private void OnEffectRemoved(object obj)
		{
			if (obj is Effect effect && effect.Id == TEffects.DOUBLETROUBLE)
				Die();
		}

		[OnDeserialized]
		private void OnDeserialized()
		{
			if (isDoubleTroubleDupe)
			{
				effects.Add(TEffects.DOUBLETROUBLE, true);
				isDoubleTroubleDupe = false;
			}
		}

		public void MakeItDouble()
		{
			kbac.TintColour = new Color(1, 1, 1, 0.5f);
			effects.Add(TEffects.DOUBLETROUBLE, true);
		}

		private void Die()
		{
			if (Game.IsQuitting())
				return;

			var equipment = identity.GetEquipment();
			if (equipment != null)
				equipment.UnequipAll();

			Game.Instance.SpawnFX(SpawnFXHashes.BuildingFreeze, transform.position, 0);
			Util.KDestroyGameObject(this);
		}
#if WIP_EVENTS
		public override void OnCleanUp()
		{
			base.OnCleanUp();
			GameClock.Instance.Unsubscribe((int)GameHashes.Nighttime, OnNight);
		}
#endif

		public void OnImgui()
		{
#if WIP_EVENTS
			ImGui.Checkbox("Were Vole", ref debugForceWereVole);

			if(debugForceWereVole != isWereVole)
				SetWereVole(debugForceWereVole);

			if(isWereVole && ImGui.Button("WERE VOLE TIME"))
			{
				OnNight(null);
			}
#endif
		}
#if WIP_EVENTS
		private void SetWereVole(bool isWere)
		{
			if (isWere)
				BecomeWereVole();
			else
				CureWereVole();

			isWereVole = isWere;
		}
#endif
	}
}
