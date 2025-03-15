using Database;
using ImGuiNET;
using Klei.AI;
using KSerialization;
using System.Runtime.Serialization;
using UnityEngine;

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

		private Transform superTrail;

		public const float SUPER_DURATION_SECONDS = 600 * 30;

		public bool IsWereVole => isWereVole;

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (effects.HasEffect(TEffects.DOUBLETROUBLE))
				kbac.TintColour = new Color(1, 1, 1, 0.5f);

			if (effects.HasEffect(TEffects.SUPERDUPE))
				InitSuperEffect();

			Subscribe((int)GameHashes.EffectRemoved, OnEffectRemoved);
			Subscribe((int)GameHashes.EffectAdded, OnEffectAdded);

			GameClock.Instance.Subscribe((int)GameHashes.Nighttime, OnNight);

			if (GameClock.Instance.IsNighttime())
				OnNight(null);

#if HULK
			if (!hasHealedHulk)
			{
				var health = GetComponent<Health>();
				health.hitPoints = health.maxHitPoints;

				hasHealedHulk = true;
			}
#endif
		}


		private void InitSuperEffect()
		{
			if (!Mod.Settings.SuperDupe_RenderTrail)
				return;

			if (superTrail == null)
				superTrail = Instantiate(ModAssets.Prefabs.superDupeTrail).transform;

			superTrail.parent = transform;
			superTrail.SetLocalPosition(new Vector3(0, 0.75f, 0.05f));
			superTrail.gameObject.SetActive(true);
		}

		private void EndSuperEffect()
		{
			if (superTrail != null)
				Destroy(superTrail.gameObject);

			AkisTwitchEvents.Instance.onDupeSuperEnded?.Invoke();
		}

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
#if WEREVOLE
			if (isWereVole)
			{
				var voleGo = FUtility.Utils.Spawn(WereVoleConfig.ID, gameObject);
				voleGo.GetComponent<WereVoleContainer>().FromMinion(this);
			}
#endif
		}

		private void OnEffectAdded(object obj)
		{
			if (obj is Effect effect)
			{
				switch (effect.Id)
				{
					case TEffects.DOUBLETROUBLE:
						kbac.TintColour = new Color(1, 1, 1, 0.5f);
						break;
					case TEffects.SUPERDUPE:
						InitSuperEffect();
						break;
				}
			}
		}

		private void OnEffectRemoved(object obj)
		{
			if (obj is Effect effect)
			{
				switch (effect.Id)
				{
					case TEffects.DOUBLETROUBLE:
						Die();
						break;
					case TEffects.SUPERDUPE:
						EndSuperEffect();
						break;
				}
			}
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
			Mod.doubledDupe.Add(identity);
		}

		private static NumberOfDupes GetMin20AchievementRequirement()
		{
			var min20Dupes = Db.Get().ColonyAchievements.Minimum20LivingDupes;
			return min20Dupes.requirementChecklist.Find(req => req is NumberOfDupes) as NumberOfDupes;
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
		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.doubledDupe.Remove(identity);
			GameClock.Instance.Unsubscribe((int)GameHashes.Nighttime, OnNight);
		}

		public void OnImgui()
		{
			ImGui.Checkbox("Were Vole", ref debugForceWereVole);

			if (debugForceWereVole != isWereVole)
				SetWereVole(debugForceWereVole);

			if (isWereVole && ImGui.Button("WERE VOLE TIME"))
			{
				OnNight(null);
			}
		}

		private void SetWereVole(bool isWere)
		{
			if (isWere)
				BecomeWereVole();
			else
				CureWereVole();

			isWereVole = isWere;
		}
	}
}
