using Database;
using ImGuiNET;
using Klei.AI;
using KSerialization;
using System.Linq;
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
		[MyCmpReq] private Accessorizer accessorizer;

		[Serialize] private bool isDoubleTroubleDupe;
		[Serialize] public Color hairColor;
		[Serialize] public bool dyeHair;
		[Serialize] private int hairId;
		[Serialize] private bool isWereVole;
		[Serialize] private bool hasHealedHulk;
		[Serialize] public float wereVoleSince;

		private bool debugForceWereVole;
		private bool _twitchLookUpdated;

		private Transform superTrail;

		public const float SUPER_DURATION_SECONDS = 600 * 30;

		public static readonly int[] allowedHairIds =
[
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			15,
			16,
			17,
			18,
			19,
			30,
			36,
			37,
			43,
			44
		];

		public bool IsWereVole => isWereVole;

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (effects.HasEffect(TEffects.DOUBLETROUBLE))
				kbac.TintColour = new Color(1, 1, 1, 0.5f);
			else if (effects.HasEffect(TEffects.SUPERDUPE))
				InitSuperEffect();
			else if (effects.HasEffect(TEffects.TWITCH_GUEST))
				InitTwitchLook();

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
					case TEffects.TWITCH_GUEST:
						InitTwitchLook();
						break;
					case TEffects.SUPERDUPE:
						InitSuperEffect();
						break;
				}
			}
		}

		private void InitTwitchLook()
		{
			if (_twitchLookUpdated)
				return;

			_twitchLookUpdated = true;

			if (hairId == 0)
			{
				var personality = Db.Get().Personalities.TryGet(identity.personalityResourceId);
				if (personality == null)
					return;

				hairId = personality.hair;

				if (!allowedHairIds.Contains(personality.hair))
					hairId = allowedHairIds.GetRandom();
			}

			ChangeAccessorySlot(kbac);
		}

		public void ApplyTwitchLook(KBatchedAnimController kbac)
		{
			ChangeAccessorySlot(kbac);
		}

		// make sure a vanilla hair is saved as the body data, so if this mod is removed, these dupes can still load and exist
		private void ChangeAccessorySlot(KBatchedAnimController kbac)
		{
			if (!dyeHair)
				return;

			if (!allowedHairIds.Contains(hairId))
				return;

			var kanim = Assets.GetAnim("aete_bleachedhair_kanim");

			var hair = string.Format("hair_bleached_{0:000}", hairId);
			var hat = string.Format("hat_hair_bleached_{0:000}", hairId);
			var controller = kbac.GetComponent<SymbolOverrideController>();

			var hairSymbol = kanim.GetData().build.GetSymbol(hair);
			var hatSymbol = kanim.GetData().build.GetSymbol(hat);

			var hairSymbolId = Db.Get().AccessorySlots.Hair.targetSymbolId;
			controller.AddSymbolOverride(hairSymbolId, hairSymbol, 99);
			var hatHairSymbolId = Db.Get().AccessorySlots.HatHair.targetSymbolId;
			controller.AddSymbolOverride(hatHairSymbolId, hatSymbol, 99);

			kbac.SetSymbolTint(hairSymbolId, hairColor);
			kbac.SetSymbolTint(hatHairSymbolId, hairColor);
		}

		private void OnEffectRemoved(object obj)
		{
			if (obj is Effect effect)
			{
				switch (effect.Id)
				{
					case TEffects.DOUBLETROUBLE:
					case TEffects.TWITCH_GUEST:
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

		public void TwitchBorn(Color32? color)
		{
			if (color.HasValue)
			{
				hairColor = (Color)color;
				dyeHair = true;
			}

			effects.Add(TEffects.TWITCH_GUEST, true);
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
