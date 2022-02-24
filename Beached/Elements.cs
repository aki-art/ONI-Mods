using Beached.Patches;
using System.Collections;
using System.IO;
using System.Reflection;
using static Element;

namespace Beached
{
    internal class Elements
    {
        public static SimHashes Basalt = EnumPatch.RegisterSimHash("Basalt");
        public static SimHashes Bismuth = EnumPatch.RegisterSimHash("Bismuth");
        public static SimHashes BismuthOre = EnumPatch.RegisterSimHash("BismuthOre");
        public static SimHashes BismuthGas = EnumPatch.RegisterSimHash("BismuthGas");
        public static SimHashes MoltenBismuth = EnumPatch.RegisterSimHash("MoltenBismuth");
        public static SimHashes Mucus = EnumPatch.RegisterSimHash("Mucus");
        public static SimHashes SaltyOxygen = EnumPatch.RegisterSimHash("SaltyOxygen");

        public static void RegisterSubstances(Hashtable substanceList)
        {
            // Liquids
            substanceList.Add(Mucus, ElementUtil.CreateSubstance(Mucus, "liquid_tank_kanim", State.Liquid, ModAssets.Colors.mucus, ModAssets.Colors.mucusUi, ModAssets.Colors.mucusConduit));
            substanceList.Add(MoltenBismuth, ElementUtil.CreateSubstance(MoltenBismuth, "liquid_tank_kanim", State.Liquid, ModAssets.Colors.moltenBismuth));

            // Gases
            substanceList.Add(SaltyOxygen, ElementUtil.CreateSubstance(SaltyOxygen, "gas_tank_kanim", State.Gas, ModAssets.Colors.saltyOxygen));
            substanceList.Add(BismuthGas, ElementUtil.CreateSubstance(BismuthGas, "gas_tank_kanim", State.Gas, ModAssets.Colors.bismuthGas));

            // Solids
            substanceList.Add(BismuthOre, ElementUtil.CreateSubstance(BismuthOre, "glass_kanim", State.Solid, ModAssets.Colors.bismuthOre));
            substanceList.Add(Bismuth, ElementUtil.CreateSubstance(Bismuth, "glass_kanim", State.Solid, ModAssets.Colors.bismuth));
            substanceList.Add(Basalt, ElementUtil.CreateSubstance(Basalt, "glass_kanim", State.Solid, ModAssets.Colors.basalt));
        }

        public static void SetSolidMaterials()
        {
            var folder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", "elements", "textures");

            var oreMaterial = Assets.instance.substanceTable.GetSubstance(SimHashes.Cuprite).material;
            var metalMaterial = Assets.instance.substanceTable.GetSubstance(SimHashes.Copper).material;

            ElementUtil.SetTextures(BismuthOre, oreMaterial, folder, "bismuth_ore", "bismuth_ore_specular");
            ElementUtil.SetTextures(Bismuth, metalMaterial, folder, "bismuth_ore", "bismuth_ore_specular");
            ElementUtil.SetTextures(Basalt, null, folder, "bismuth_ore");
        }
    }
}
