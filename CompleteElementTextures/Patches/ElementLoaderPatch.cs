using FUtility;
using HarmonyLib;
using System.IO;
using UnityEngine;

namespace CompleteElementTextures.Patches
{
	public class ElementLoaderPatch
	{
		private static string texturePath;

		[HarmonyPatch(typeof(ElementLoader), "Load")]
		public static class Patch_ElementLoader_Load
		{
			public static void Postfix()
			{
				texturePath = Path.Combine(Utils.ModPath, "textures");
				var metalMaterial = ElementLoader.GetElement(SimHashes.Steel.CreateTag()).substance.material;
				var oreMaterial = ElementLoader.GetElement(SimHashes.Cuprite.CreateTag()).substance.material;

				SetTexture(SimHashes.Aerogel, "aerogel_kanim");
				SetTexture(SimHashes.RefinedCarbon, "refinedcarbon_kanim", true, metalMaterial);
				SetTexture(SimHashes.Creature, "creature_kanim");
				SetTexture(SimHashes.CarbonFibre, "carbonfibre_kanim");
				SetTexture(SimHashes.Bitumen, "bitumen_kanim");

				var oilMaterial = SetTexture(SimHashes.SolidNaphtha, "solidnaphtha_kanim", true, oreMaterial).material;
				oilMaterial.SetColor("_ShineColour", Util.ColorFromHex("2b92e0"));

				var petrolMaterial = SetTexture(SimHashes.SolidPetroleum, "solidpetroleum_kanim", false, oreMaterial).material;
				petrolMaterial.SetTexture("_ShineMask", oilMaterial.GetTexture("_ShineMask")); oilMaterial.SetColor("_ShineColour", new Color(1.1f, 1.1f, 1.1f, 1f));

				var phytoOil = ElementLoader.FindElementByHash(SimHashes.FrozenPhytoOil);
				if (phytoOil != null)
					phytoOil.substance.anim = Assets.GetAnim("met_frozenphytooil_kanim");

				var cobaltMaterial = SetTexture(SimHashes.Cobalt, null, true, metalMaterial).material;
				cobaltMaterial.SetColor("_ShineColour", new Color32(0, 168, 255, 255));
			}

			private static Substance SetTexture(SimHashes elementId, string anim = null, bool shiny = false, Material reference = null)
			{
				var element = ElementLoader.FindElementByHash(elementId);

				if (element == null)
					return null;

				var id = elementId.ToString().ToLower();

				var texture = FAssets.LoadTexture(id, texturePath);

				element.substance.material = new Material(reference ?? element.substance.material)
				{
					mainTexture = texture
				};

				if (shiny)
					element.substance.material.SetTexture("_ShineMask", FUtility.FAssets.LoadTexture(id + "_mask", texturePath));

				if (anim != null)
					element.substance.anim = Assets.GetAnim(anim);

				return element.substance;
			}
		}
	}
}
