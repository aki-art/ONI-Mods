using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Twitchery.Content.Defs.Critters;
using UnityEngine;

namespace Twitchery.Patches
{
    public class ButcherablePatch
	{
		[HarmonyPatch(typeof(Butcherable), "OnButcherComplete")]
		public class Butcherable_OnButcherComplete_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();
				var m_SpawnPrefab = AccessTools.Method(typeof(Scenario), "SpawnPrefab", new[]
				{
					typeof(int), typeof(int), typeof(int), typeof(string), typeof(Grid.SceneLayer)
				});

				// find injection point
				var index = codes.FindIndex(ci => ci.Calls(m_SpawnPrefab));

				if (index == -1)
					return codes;

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(Butcherable_OnButcherComplete_Patch), nameof(ModifyDrops));

				// inject right after the found index
				codes.InsertRange(index + 1, new[]
				{
					// gameobject put on stack
					new CodeInstruction(OpCodes.Dup),
					new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Call, m_InjectedMethod)
				});

				return codes;
			}

			private static void ModifyDrops(GameObject spawnedPrefab, Butcherable butcherable)
			{
				if (spawnedPrefab == null)
					return;

				if (butcherable.IsPrefabID(GiantCrabConfig.ID) && spawnedPrefab.IsPrefabID(CrabShellConfig.ID))
				{
					spawnedPrefab.TryGetComponent(out KBatchedAnimController kbac);
					kbac.animScale *= 4f;

					spawnedPrefab.TryGetComponent(out KBoxCollider2D collider);
					collider.size *= 4f;
				}
			}
		}
	}
}
