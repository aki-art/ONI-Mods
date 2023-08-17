using Database;
using FUtility;
using ImGuiNET;
using Klei.AI;
using KSerialization;
using ONITwitchLib;
using System;
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
		[MyCmpReq] private KPrefabID kPrefabID;
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
		};

		public void OnImgui()
		{
			if (potentialNextSkills == null)
				return;

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

		private void UpdateNextSkills()
		{
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

			new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0.0f, 0.0f, -0.1f)).StartSM();

			UpdateNextSkills();
		}

		public bool HasPerk(HashedString perkId) => masteredSkillPerks.Contains(perkId);

		public string LearnNextSkill()
		{
			if (nextSkill == null)
				UpdateNextSkills();

			if (nextSkill == null)
				return null;

			var str = Db.Get().Skills.Get(nextSkill).Name;

			LearnSkill(nextSkill);

			return str;
		}
	}
}
