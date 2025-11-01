using Database;
using HarmonyLib;
using ONITwitchLib;
using System.Collections.Generic;
using System.Linq;

namespace Twitchery.Content.Events.EventTypes
{
	internal class EducationalEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Educational";

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var skillGroups = new List<SkillGroup>(Db.Get().SkillGroups.resources);
			skillGroups.Shuffle();
			var success = false;

			foreach (var skillGroup in skillGroups)
			{
				success = TryUpgrading(skillGroup);
				if (success)
					break;
			}

			if (!success)
			{
				ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.EDUCATIONAL.TOAST, STRINGS.AETE_EVENTS.EDUCATIONAL.FAIL_NO_DUPES);
			}
		}

		private bool TryUpgrading(SkillGroup skillGroup)
		{

			var dupes = Components.MinionResumes.Where(resume => CanLearnSkill(resume, skillGroup));

			if (dupes.Count() == 0)
				return false;

			var dupesList = "";

			if (dupes.Count() > 5)
			{
				dupes = dupes
					.Shuffle()
					.Take(5);
			}

			foreach (var dupe in dupes)
			{
				dupe.GetComponent<MinionResume>().SetAptitude(skillGroup.IdHash, 10);

				if (!dupe.HasTag(GameTags.Stored))
				{
					new EmoteChore(dupe.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_cheer_kanim",
					[
						"cheer_pre",
						"cheer_loop",
						"cheer_pst"
					]);
				}
			}

			dupesList += dupes.Join(d => d.GetProperName());

			var body = STRINGS.AETE_EVENTS.EDUCATIONAL.DESC_DUPES.Replace("{Skill}", skillGroup.Name);
			body += "\n";
			body += dupesList;

			var toastGo = ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.EDUCATIONAL.TOAST, body);

			var texts = toastGo.GetComponentsInChildren<LocText>();



			return true;
		}

		private bool CanLearnSkill(MinionResume resume, SkillGroup skillGroup)
		{
			if (resume.identity.model != GameTags.Minions.Models.Standard)
				return false;

			if (resume.HasTag(GameTags.Dead))
				return false;

			if (resume.AptitudeBySkillGroup.ContainsKey(skillGroup.IdHash) && resume.AptitudeBySkillGroup[skillGroup.IdHash] > 0f)
				return false;

			return true;
		}
	}
}
