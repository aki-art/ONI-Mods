/*using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
	internal class CrewJobsScreenPatch
	{

		[HarmonyPatch(typeof(CrewJobsScreen), "SpawnEntries")]
		public class CrewJobsScreen_SpawnEntries_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_SpawnEntries = AccessTools.Method(typeof(CrewJobsScreen).BaseType, "SpawnEntries");

				// find injection point
				var index = codes.FindIndex(ci => ci.Calls(m_SpawnEntries));

				if (index == -1)
				{
					Log.Warning("Could not patch CrewJobsScreen.SpawnEntries.");
					return codes;
				}

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(CrewJobsScreen_SpawnEntries_Patch), nameof(InjectedMethod));

				// inject right after the found index
				codes.InsertRange(index + 1, new[]
				{
					new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Call, m_InjectedMethod)
				});

				return codes;
			}

			private static void InjectedMethod(CrewJobsScreen instance)
			{
				foreach (var _identity in Mod.regularPips.Items)
				{
					var go = Util.KInstantiateUI(instance.Prefab_CrewEntry, instance.EntriesPanelTransform.gameObject);
					var jobsEntry = go.GetComponent<CrewJobsEntry>();
					var entry = go.AddOrGet<CrewPipEntry>();
;
				}
			}

*//*
			public static void Populate(CrewJobsEntry entry, RegularPip _identity)
			{
				entry.identity = _identity;
				if ((Object)entry.portrait == (Object)null)
				{
					entry.portrait = Util.KInstantiateUI<CrewPortrait>(entry.PortraitPrefab.gameObject, (Object)entry.crewPortraitParent != (Object)null ? entry.crewPortraitParent : entry.gameObject);
					if ((Object)entry.crewPortraitParent == (Object)null)
						entry.portrait.transform.SetSiblingIndex(2);
				}
				entry.portrait.SetIdentityObject((IAssignableIdentity)_identity);

				entry.consumer = _identity.GetComponent<ChoreConsumer>();
				entry.consumer.choreRulesChanged += new System.Action(entry.Dirty);
				foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
					entry.CreateChoreButton(resource);
				entry.CreateAllTaskButton();
				entry.dirty = true;
			}*//*
		}
	}
}
*/