using DecorPackB.Content;
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
				var fossil = ElementLoader.FindElementByHash(SimHashes.Fossil);

				if (fossil == null)
					return;

				if (fossil.oreTags == null)
				{
					fossil.oreTags = new Tag[]
					{
						DPTags.fossilMaterial
					};
				}
				else
				{
					fossil.oreTags = fossil.oreTags.AddToArray(DPTags.fossilMaterial);
				}
			}

		}
	}
}
