using Klei.AI;
using Slag.Content;
using UnityEngine;

namespace Slag
{
    public class ModAssets
    {
        public static ComplexRecipe.RecipeNameDisplay slagNameDisplay = (ComplexRecipe.RecipeNameDisplay)451;

        public class Amounts
        {
            public static Amount ShellGrowth;
            public static Amount ShellIntegrity;
        }

        public class Colors
        {
            public static readonly Color slag = new Color32(185, 255, 255, 255);
            public static readonly Color slagGlass = new Color32(185, 255, 255, 255);
            public static readonly Color moltenSlagGlass = new Color32(3, 115, 116, 128);
        }

        public class Tags
        {
            public static readonly Tag slagWool = TagManager.Create("Slag_SlagWool");
            public static readonly Tag slag = Elements.Slag.CreateTag(); 
            public static readonly Tag beingMined = TagManager.Create("Slag_BeingMined");
            public static readonly Tag grownShell = TagManager.Create("Slag_GrownShell");

            public static class Species
            {
                public static readonly Tag slagmite = TagManager.Create("SlagmiteSpecies");
            }
        }
    }
}
