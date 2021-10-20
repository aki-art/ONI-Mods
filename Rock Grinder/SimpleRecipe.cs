using System.Collections.Generic;

namespace RockGrinder
{
    public class SimpleRecipe
    {
        public float OreToMetalRatio { get; set; } = 0.75f;
        public float MineralToSandRatio { get; set; } = 1;
        public bool AllowCreatureGrinding { get; set; } = true;
        public bool HurtFallenDuplicants { get; set; } = true;
        public Dictionary<Tag, SimpleRecipeOutput> AdditionalRecipes { get; set; }

        public class SimpleRecipeOutput
        {
            public float In { get; set; }
            public Dictionary<Tag, float> Out;
        }
    }
}

