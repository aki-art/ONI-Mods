using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Cmps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace PrintingPodRecharge.Patches
{
    public class CharacterSelectionControllerPatch
    {
        [HarmonyPatch(typeof(CharacterSelectionController), "InitializeContainers")]
        public static class CharacterSelectionController_InitializeContainers_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var f_numberOfDuplicantOptions = AccessTools.Field(typeof(CharacterSelectionController), "numberOfDuplicantOptions");
                var f_numberOfCarePackageOptions = AccessTools.Field(typeof(CharacterSelectionController), "numberOfCarePackageOptions");

                var m_GetDupeCount = AccessTools.Method(typeof(CharacterSelectionController_InitializeContainers_Patch), "GetDupeCount", new Type[] { typeof(int) });
                var m_GetItemCount = AccessTools.Method(typeof(CharacterSelectionController_InitializeContainers_Patch), "GetItemCount", new Type[] { typeof(int) });

                var codes = orig.ToList();

                int dupeIndex = codes.FindLastIndex(c => c.opcode == OpCodes.Ldfld && c.operand is FieldInfo f && f == f_numberOfDuplicantOptions);
                int itemIndex = codes.FindLastIndex(c => c.opcode == OpCodes.Ldfld && c.operand is FieldInfo f && f == f_numberOfCarePackageOptions);

                if (dupeIndex == -1 || itemIndex == -1)
                {
                    Log.Warning("Could not patch CharacterSelectionController.");
                    return codes;
                }

                codes.Insert(dupeIndex + 1, new CodeInstruction(OpCodes.Call, m_GetDupeCount));
                codes.Insert(itemIndex + 2, new CodeInstruction(OpCodes.Call, m_GetItemCount));

                Log.PrintInstructions(codes);

                return codes;
            }

            public static int GetDupeCount(int count)
            {
                if(ImmigrationModifier.Instance == null)
                {
                    return count;
                }

                return ImmigrationModifier.Instance.GetDupeCount(count);
            }

            public static int GetItemCount(int count)
            {
                if (ImmigrationModifier.Instance == null)
                {
                    return count;
                }

                return ImmigrationModifier.Instance.GetItemCount(count);
            }
        }
    }
}
