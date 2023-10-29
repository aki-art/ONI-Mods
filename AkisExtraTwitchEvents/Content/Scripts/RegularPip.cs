using Database;
using FUtility;
using ImGuiNET;
using Klei.AI;
using KSerialization;
using ONITwitchLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class RegularPip : KMonoBehaviour, IImguiDebug
	{
		[Serialize] private bool initialized;
		[Serialize] public HashSet<HashedString> masteredSkills;
		[Serialize] public HashSet<HashedString> masteredSkillPerks;
		[Serialize] public int level;

		[MyCmpReq] private KPrefabID kPrefabID;
		[MyCmpReq] private Traits traits;

		private static Vector4 grey = new(1, 1, 1, 0.4f);
		private static Vector4 white = new(1, 1, 1, 1);

		public HashedString nextSkill;
		public HashSet<HashedString> potentialNextSkills;

		public static Dictionary<int, RegularPip> regularPipCache = new();

		private static HashSet<string> allowedSkills = new()
		{
			nameof(Skills.Mining1),
			nameof(Skills.Mining2),
			nameof(Skills.Mining3),
			nameof(Skills.Mining4),
			nameof(Skills.Building1),
			nameof(Skills.Building2),
			nameof(Skills.Building3),
			nameof(Skills.Basekeeping1),
			nameof(Skills.Basekeeping2),
			nameof(Skills.Hauling1),
			nameof(Skills.Hauling2),
			nameof(Skills.Cooking1),
			nameof(Skills.Cooking2),
			nameof(Skills.Farming1),
			nameof(Skills.Farming2),
			nameof(Skills.Farming3),
			nameof(Skills.Pyrotechnics),
			nameof(Skills.Engineering1),
		};

		public void OnImgui()
		{
			if (potentialNextSkills == null)
				return;

			ImGui.Text($"Level: {level}");

			if (ImGui.Button("Level Up"))
				LearnNextSkill();

			var nextSkillStr = nextSkill == null
				? "none"
				: Db.Get().Skills.Get(nextSkill).Id;

			ImGui.Text("Next Skill: " + nextSkillStr);
			foreach (var skill in allowedSkills)
			{
				if (!masteredSkills.Contains(skill))
				{
					var color = potentialNextSkills.Contains(skill)
						? white
						: grey;

					ImGui.TextColored(color, "•");
					ImGui.SameLine();

					if (ImGui.Button($"Learn {skill}"))
						LearnSkill(skill);
				}
				else
				{
					ImGui.TextColored(grey, $"{skill} learnt");
				}
			}
		}


		public override void OnSpawn()
		{
			if (!initialized)
			{
				var names = STRINGS.DUPLICANTS.REGULAR_PIP_NAMES.NAMES.ToString().Split('/');
				if (names.Length > 0)
					GetComponent<UserNameable>().SetName(names.GetRandom());

				var dbTraits = Db.Get().traits;
				traits.Add(dbTraits.Get(TTraits.PIP_ROOKIE1));
				traits.Add(dbTraits.Get(TTraits.PIP_ROOKIE2));

				initialized = true;
			}

			masteredSkills ??= new();
			masteredSkillPerks ??= new();
			potentialNextSkills ??= new();

			base.OnSpawn();
			NameDisplayScreen.Instance.RegisterComponent(gameObject, this);
			Mod.regularPips.Add(this);
			regularPipCache.Add(kPrefabID.InstanceID, this);

			if (masteredSkills.Count == allowedSkills.Count)
				return;

			UpdateNextSkills();

			AkisTwitchEvents.UpdateRegularPipWeight();
		}

		public void LevelUp(bool showToast)
		{
			var traitId = GetNextTraitId();

			if (traitId == null)
				return;

			level++;

			Trait trait = Db.Get().traits.Get(traitId);
			traits.Remove(trait);

			if (showToast)
			{
				ToastManager.InstantiateToastWithGoTarget(
					STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.TOAST,
					STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.DESC_FIRST,
					gameObject);
			}

			new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0.0f, 0.0f, -0.1f)).StartSM();

			var yOffset = 0.5f;

			foreach (var skill in trait.disabledChoreGroups)
			{
				PopFXManager.Instance.SpawnFX(
					PopFXManager.Instance.sprite_Plus,
					skill.Name,
					transform,
					new Vector3(0, yOffset));

				yOffset += 0.5f;
			}

			Game.Instance.Trigger((int)GameHashes.RolesUpdated, null);
		}

		private HashedString GetNextTraitId()
		{
			return level switch
			{
				LEVELS.ROOKIE => (HashedString)TTraits.PIP_ROOKIE1,
				LEVELS.LEARNED => (HashedString)TTraits.PIP_ROOKIE2,
				_ => null,
			};
		}

		private void UpdateNextSkills()
		{
			if (level < LEVELS.MASTER)
				return;

			var skills = Db.Get().Skills;
			potentialNextSkills = allowedSkills
				.Where(skillId =>
				{
					var hasAlreadyLearned = masteredSkills.Contains((HashedString)skillId);
					if (hasAlreadyLearned)
						return false;

					var skill = skills.TryGet(skillId);

					if (skill == null)
						return false;

					var hasNoPrerequisites = skill.priorSkills.Count == 0;
					if (hasNoPrerequisites)
						return true;

					foreach (var priorSkill in skill.priorSkills)
					{
						var hasLearntRequisite = masteredSkills.Contains(priorSkill);
						if (!hasLearntRequisite)
							return false;
					}

					return true;
				})
				.Select(str => (HashedString)str)
				.ToHashSet();

			nextSkill = potentialNextSkills.Count > 0 ? potentialNextSkills.GetRandom() : null;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.regularPips.Remove(this);
		}

		public void LearnSkill(HashedString skillId)
		{
			var skill = Db.Get().Skills.TryGet(skillId);

			if (skill == null)
			{
				Log.Warning("Invalid skill " + skillId);
				return;
			}

			masteredSkills.Add(skillId);

			foreach (var perk in skill.perks)
				masteredSkillPerks.Add(perk.IdHash);

			new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0, 0, -0.1f)).StartSM();

			UpdateNextSkills();

			Game.Instance.Trigger((int)GameHashes.RolesUpdated, null);
		}

		public bool HasPerk(HashedString perkId) => masteredSkillPerks.Contains(perkId);

		public string LearnNextSkill()
		{

			if (level < LEVELS.MASTER)
			{
				LevelUp(true);
				return null;
			}

			if (nextSkill == null)
				UpdateNextSkills();

			if (nextSkill == null)
				return null;

			var str = Db.Get().Skills.Get(nextSkill).Name;

			LearnSkill(nextSkill);

			return str;
		}

		public static class LEVELS
		{
			public const int
				ROOKIE = 0,
				LEARNED = 1,
				MASTER = 2;
		}
	}
}
