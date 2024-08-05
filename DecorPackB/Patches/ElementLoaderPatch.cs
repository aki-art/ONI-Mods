using DecorPackB.Content;
using DecorPackB.Content.Db;
using DecorPackB.Content.Scripts;
using HarmonyLib;

namespace DecorPackB.Patches
{
	public class ElementLoaderPatch
	{
		[HarmonyPatch(typeof(ElementLoader), "Load")]
		public static class Patch_ElementLoader_Load
		{
			public static void Postfix()
			{
				SetFossilTag();
				SetFloorLampFrameTags();
				SetFloorLampPaneTags();
			}

			private static void SetFossilTag()
			{
				var fossil = ElementLoader.FindElementByHash(SimHashes.Fossil);

				if (fossil == null)
					return;

				if (fossil.oreTags == null)
				{
					fossil.oreTags =
					[
						DPTags.fossilMaterial
					];
				}
				else
				{
					fossil.oreTags = fossil.oreTags.AddToArray(DPTags.fossilMaterial);
				}
			}

			private static void SetFloorLampFrameTags()
			{
				foreach (var entry in FloorLampFrames.tileInfos)
				{
					var element = ElementLoader.GetElement(entry.ElementTag);

					if (element != null)
					{
						element.oreTags ??= [];
						element.oreTags = element.oreTags.AddToArray(ModTags.floorLampFrameMaterial);
					}
				}
			}

			private static void SetFloorLampPaneTags()
			{
				foreach (var elementName in FloorLampPanes.entries.Keys)
				{
					var element = ElementLoader.FindElementByName(elementName);

					if (element != null)
					{
						element.oreTags ??= [];
						element.oreTags = element.oreTags.AddToArray(ModTags.floorLampPaneMaterial);
					}
				}
			}
		}
	}
}
