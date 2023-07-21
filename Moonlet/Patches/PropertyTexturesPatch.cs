using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Moonlet.Patches
{

	public class PropertyTexturesPatch
	{
		// Makes elements with Melty tag look melty
		//[HarmonyPatch(typeof(PropertyTextures), nameof(PropertyTextures.UpdateFallingSolidChange))]
		public static class PropertyTextures_UpdateFallingSolidChange_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var f_id = AccessTools.Field(typeof(Element), nameof(Element.id));

				var index = codes.FindIndex(ci => ci.LoadsField(f_id)); /// 34 ldc.i4 SimHashes.Oxygen

				var valueIndex = codes.FindLastIndex(ci => ci.LoadsConstant(0.65f));

				if (index == -1)
					return codes;

				if(valueIndex == -1)
				{
					Log.Warning("Nothing loads 0.65f");
					return codes;
				}

				Log.Debuglog("valueIndex " + valueIndex);
				Log.Debuglog("index " + index);

				var m_HasTag = AccessTools.Method(typeof(Element), nameof(Element.HasTag), new[] { typeof(Tag) });
				var f_tag = AccessTools.Field(typeof(MTags), nameof(MTags.Melty));

				var m_isMelty = AccessTools.Method(typeof(PropertyTextures_UpdateFallingSolidChange_Patch), nameof(IsMelty), new[]
				{
					typeof(Element)
				});


				var meltyLabel = generator.DefineLabel(); 
				codes[valueIndex].labels.Add(meltyLabel); // 33

				var continueLabel = generator.DefineLabel();
				codes[index].labels.Add(continueLabel); // 26

				codes.InsertRange(index, new[]
				{
					new CodeInstruction(OpCodes.Dup), // duplicate element entry
					new CodeInstruction(OpCodes.Call, m_isMelty),
					new CodeInstruction(OpCodes.Brtrue_S, meltyLabel),
				});

				Log.PrintInstructions(codes);

				return codes;
			}

			private static bool IsMelty(Element element)
			{
				return element.HasTag(MTags.Melty);
			}
		}

		// Makes the Salty Oxygen texture the lighter texture Oxygen uses
		[HarmonyPatch(typeof(PropertyTextures), nameof(PropertyTextures.UpdateDanger))]
		public static class PropertyTextures_UpdateDanger_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var index = codes.FindIndex(ci => ci.LoadsConstant((int)SimHashes.Oxygen)); /// 34 ldc.i4 SimHashes.Oxygen

				if (index == -1)
					return codes;

				var m_GetDangerForElement = AccessTools.Method(typeof(PropertyTextures_UpdateDanger_Patch), nameof(GetMeltiness), new[] { typeof(int), typeof(int) });

				codes.InsertRange(index + 3, new[]
				{
					// byte.maxValue is loaded to the stack
					new CodeInstruction(OpCodes.Ldloc_2), // load num to the stack
					new CodeInstruction(OpCodes.Call, m_GetDangerForElement)
				});

				return codes;
			}

			// Calling with existing value so there is a possibility for other mods to also add their own values
			private static byte GetMeltiness(int existingValue, int cell)
			{
				return (Grid.Element[cell].HasTag(GameTags.Breathable))
					? (byte)0
					: (byte)existingValue;
			}
		}
	}
}