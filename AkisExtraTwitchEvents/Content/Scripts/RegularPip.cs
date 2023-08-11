using Database;
using ImGuiNET;
using KSerialization;
using System.Collections.Generic;
using System.Linq;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class RegularPip : KMonoBehaviour
	{
		[Serialize] private bool initialized;
		[Serialize] private HashSet<HashedString> masteredSkills;
		[MyCmpReq] private KPrefabID kPrefabID;

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
		};

		public void OnImguiDebug()
		{
			foreach(var skill in allowedSkills)
			{
				if (!masteredSkills.Contains(skill) && ImGui.Button($"Learn {skill}"))
				{
					LearnSkill(skill);
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
				masteredSkills = new();

				//var skills = Db.Get().Skills;
			}

			base.OnSpawn();
			NameDisplayScreen.Instance.RegisterComponent(gameObject, this);
			Mod.regularPips.Add(this);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.regularPips.Remove(this);
		}

		public void LearnSkill(HashedString skill)
		{
			masteredSkills.Add(skill);
			kPrefabID.AddTag(TagManager.Create(skill.HashValue.ToString()));
		}

		public bool HasPerk(HashedString perkId)
		{
			foreach (var skill in masteredSkills)
			{
				if (Db.Get().Skills.Get(skill).GivesPerk(perkId))
					return true;
			}

			return false;
		}
	}
}
