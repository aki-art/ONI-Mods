using Database;
using ImGuiNET;
using Klei.AI;
using KSerialization;
using System;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AETE_MinionStorage : KMonoBehaviour, IImguiDebug, ISim200ms, ISim33ms
	{
		private const string FOOT_SYMBOL = "foot";

		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private Effects effects;
		[MyCmpReq] private MinionIdentity identity;
		[MyCmpReq] private Accessorizer accessorizer;
		[MyCmpReq] private KPrefabID kPrefabId;

		[Serialize] private bool isDoubleTroubleDupe;
		[Serialize] public Color hairColor;
		[Serialize] public bool dyeHair;
		[Serialize] private int hairId;
		[Serialize] private bool isWereVole;
		[Serialize] private bool hasHealedHulk;
		[Serialize] private bool toiletPaperStuck;
		[Serialize] public float wereVoleSince;

		private bool debugForceWereVole;
		private bool _twitchLookUpdated;
		private bool _isOiledUp;

		private Transform _superTrail;
		private Transform _toiletPaperTrail;
		private Transform _oilFx;
		private Transform _sweatFx;
		private SimHashes _sweatElement;

		private Guid _sweatyStatusItem;

		public const float SUPER_DURATION_SECONDS = 600 * 30;

		public static readonly Tag[] eventForbiddenTags = [
			TTags.jailed,
			GameTags.Dead,
			TTags.midased,
			TTags.midasSafe,
			GameTags.Stored ];

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

			_sweatElement = identity.model == GameTags.Minions.Models.Bionic ? SimHashes.LiquidGunk : SimHashes.SaltWater;

			if (effects.HasEffect(TEffects.DOUBLETROUBLE))
				kbac.TintColour = new Color(1, 1, 1, 0.5f);
			else if (effects.HasEffect(TEffects.SUPERDUPE))
				InitSuperEffect();
			else if (effects.HasEffect(TEffects.TWITCH_GUEST))
				InitTwitchLook();
			else if (effects.HasEffect(TEffects.OILED_UP))
				InitOil();
			else if (effects.HasEffect(TEffects.TOILER_PAPER_STUCK))
				InitToiletPaper();
			else if (effects.HasEffect(TEffects.SWEATY))
				InitSweaty();

			Subscribe((int)GameHashes.EffectRemoved, OnEffectRemoved);
			Subscribe((int)GameHashes.EffectAdded, OnEffectAdded);
			Subscribe((int)GameHashes.MinionMigration, UpdateLoner);
			Subscribe((int)GameHashes.TagsChanged, UpdateLoner);

			UpdateLoner(null);

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

		private void InitToiletPaper()
		{
			Log.Debug("initting toilet paper trail");
			Log.Assert("ModAssets.Prefabs.toiletPaperTrail", ModAssets.Prefabs.toiletPaperTrail);
			if (_toiletPaperTrail == null)
				_toiletPaperTrail = Instantiate(ModAssets.Prefabs.toiletPaperTrail).transform;

			_toiletPaperTrail.parent = transform;
			_toiletPaperTrail.SetLocalPosition(new Vector3(0, 0.1f, 0.05f));
			_toiletPaperTrail.gameObject.SetActive(true);

			toiletPaperStuck = true;
		}

		private void RemoveToiletPaper()
		{
			if (_toiletPaperTrail != null)
				Destroy(_toiletPaperTrail.gameObject);

			toiletPaperStuck = false;
		}


		private void UpdateToiletPaper()
		{
			if (_toiletPaperTrail.IsNullOrDestroyed())
				return;


			var dupePosition = (Vector3)kbac
				.GetSymbolTransform(FOOT_SYMBOL, out var _)
				.GetColumn(3) with
			{
				z = transform.position.z + 0.01f
			};


			var f = GameClock.Instance.frame;
			var offset = Mathf.Sin(0.2f * f) + Mathf.Cos(0.14f * f);
			offset *= 0.2f;
			offset += 0.2f;

			_toiletPaperTrail.position = dupePosition with { y = dupePosition.y + offset };
		}

		private void UpdateLoner(object data)
		{
			if (data is TagChangedEventData tagChanged)
			{
				if (!tagChanged.tag.IsValid
					|| !eventForbiddenTags.Contains(tagChanged.tag)
					|| tagChanged.tag == TTags.loneDupe)
					return;
			}

			var worldId = this.GetMyWorldId();
			var minionsOnMyWorld = Components.LiveMinionIdentities.GetWorldItems(worldId);

			if (minionsOnMyWorld.Count <= 1)
				kPrefabId.AddTag(TTags.loneDupe);
			else
			{
				foreach (var minion in Components.LiveMinionIdentities.GetWorldItems(worldId))
				{
					if (minion != identity && minion.GetComponent<AETE_MinionStorage>().IsTargetableByEvents())
					{
						kPrefabId.RemoveTag(TTags.loneDupe);
						return;
					}
				}
			}

			kPrefabId.AddTag(TTags.loneDupe);
		}

		private void InitSweaty()
		{
			if (_sweatFx != null)
				Destroy(_sweatFx.gameObject);

			_sweatFx = Instantiate(ModAssets.Prefabs.oilDripFx).transform;
			Destroy(_sweatFx.transform.GetChild(0).gameObject); // sparkles

			_sweatFx.transform.position = (transform.position + new Vector3(0, 1.4f, 0)) with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };
			_sweatFx.transform.parent = transform;

			var particles = _sweatFx.GetComponent<ParticleSystem>();
			var main = particles.main;
			main.startColor = (Color)ElementLoader.FindElementByHash(_sweatElement).substance.uiColour;

			_sweatFx.gameObject.SetActive(true);
		}

		private void RemoveSweaty()
		{
			if (_sweatFx != null)
				Destroy(_sweatFx.gameObject);
		}

		private void InitOil()
		{
			kPrefabId.AddTag(TTags.oiledUp);

			if (_oilFx != null)
				Destroy(_oilFx.gameObject);

			_oilFx = Instantiate(ModAssets.Prefabs.oilDripFx).transform;
			_oilFx.transform.position = (transform.position + new Vector3(0, 1.4f, 0)) with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };
			_oilFx.transform.parent = transform;

			_oilFx.gameObject.SetActive(true);
		}

		private void RemoveOil()
		{
			kPrefabId.RemoveTag(TTags.oiledUp);
			if (_oilFx != null)
				Destroy(_oilFx.gameObject);
		}

		private void InitSuperEffect()
		{
			if (!Mod.Settings.SuperDupe_RenderTrail)
				return;

			if (_superTrail == null)
				_superTrail = Instantiate(ModAssets.Prefabs.superDupeTrail).transform;

			_superTrail.parent = transform;
			_superTrail.SetLocalPosition(new Vector3(0, 0.75f, 0.05f));
			_superTrail.gameObject.SetActive(true);
		}

		private void EndSuperEffect()
		{
			if (_superTrail != null)
				Destroy(_superTrail.gameObject);

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

		private void OnNight(object _)
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
					case TEffects.OILED_UP:
						InitOil();
						break;
					case TEffects.TOILER_PAPER_STUCK:
						InitToiletPaper();
						break;
					case TEffects.SWEATY:
						InitSweaty();
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
					case TEffects.TOILER_PAPER_STUCK:
						RemoveToiletPaper();
						break;
					case TEffects.OILED_UP:
						RemoveOil();
						break;
					case TEffects.SWEATY:
						RemoveSweaty();
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

		public bool IsTargetableByEvents() => !this.HasAnyTags(eventForbiddenTags);

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

		public void Sim200ms(float dt)
		{
			if (effects.HasEffect(TEffects.SWEATY))
			{
				var roll = UnityEngine.Random.value < 0.05f;
				if (roll)
				{
					var mass = 1f;
					var temperature = Db.Get().Amounts.Temperature.Lookup(gameObject).value;

					var equippable = GetComponent<SuitEquipper>().IsWearingAirtightSuit();

					if (equippable != null)
						equippable.GetComponent<Storage>().AddLiquid(
							_sweatElement,
							mass,
							temperature,
							byte.MaxValue,
							0);
					else
						SimMessages.AddRemoveSubstance(
							Grid.CellAbove(Grid.PosToCell(this)),
							_sweatElement,
							CellEventLogger.Instance.Vomit,
							mass,
							temperature,
							byte.MaxValue,
							0);
				}
			}
		}

		public void Sim33ms(float dt)
		{
			if (toiletPaperStuck)
				UpdateToiletPaper();
		}
	}
}
