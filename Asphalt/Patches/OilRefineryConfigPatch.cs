using HarmonyLib;
using UnityEngine;

namespace Asphalt.Patches
{
	public class OilRefineryConfigPatch
	{
		[HarmonyPatch(typeof(OilRefineryConfig), nameof(OilRefineryConfig.ConfigureBuildingTemplate))]
		[HarmonyPriority(Priority.LowerThanNormal)]
		public static class OilRefineryConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go)
			{
				var elementDropper = go.AddComponent<ElementDropper>();
				elementDropper.emitMass = 100f;
				elementDropper.emitTag = SimHashes.Bitumen.CreateTag();

				AddElementConverter(go);
			}

			private static void AddElementConverter(GameObject go)
			{
				if (go.TryGetComponent(out ElementConverter elementConverter))
				{
					var output = new ElementConverter.OutputElement(5f, SimHashes.Bitumen, 348.15f, true, outputElementOffsety: 1f);
					elementConverter.outputElements = elementConverter.outputElements.Append(output);
				}
			}
		}
	}
}
