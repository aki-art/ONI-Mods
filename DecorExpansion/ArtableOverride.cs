using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Artable;

namespace DecorExpansion
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ArtableOverride : KMonoBehaviour
    {
        [MyCmpReq]
        KBatchedAnimController animController;

        [SerializeField]
        public string animFile;

        [SerializeField]
        public string defaultAnim;

        [SerializeField]
        public KAnimHashedString targetSymbol;

        [Serialize]
        public string currentSymbolOverride;

        [SerializeField]
        public List<Override> overrideSymbols2 = new List<Override>();

        public struct Override
        {
            public string id;
            public string animation;
            public bool symbolOverride;
            public Status quality;
            public string symbol;
            public string animFile;

            public Override(Status quality, string symbol, string animFile, string animation, bool symbolOverride = true)
            {
                this.symbolOverride = symbolOverride;
                this.quality = quality;
                this.symbol = symbol;
                this.animFile = animFile;
                this.animation = animation;
                id = ModAssets.PREFIX + animFile + "_" + symbol;
            }
        }

        public void AddSymbols(Status quality, string[] symbols, string animFile, string animation = null)
        {
            if (symbols == null) return;
            if (animation == null) animation = defaultAnim;

            foreach (string symbol in symbols)
            {
                overrideSymbols2.Add(new Override(quality, symbol, animFile, animation));
            }
        }

        public void AddSymbolsFromArtable(Artable target)
        {
            string animFile = target.GetComponent<KBatchedAnimController>().currentAnimFile;
            foreach (var stage in target.stages)
            {
                overrideSymbols2.Add(new Override(stage.statusItem, stage.anim, animFile, null, false));
            }
        }

        public void EnableOverride(Override symbolOverride)
        {
            if (animController == null)
                return;

            DisableOverride();
            if (symbolOverride.symbolOverride && !string.IsNullOrEmpty(animFile))
            {
                if (symbolOverride.animation != null)
                {
                    animController.Play(symbolOverride.animation);
                }

                KAnimFile anim = Assets.GetAnim(symbolOverride.animFile);
                var soc = animController.GetComponent<SymbolOverrideController>();
                soc.AddSymbolOverride(
                    targetSymbol,
                    anim.GetData().build.GetSymbol(symbolOverride.symbol),
                    6);
            }

            animController.SetSymbolVisiblity(targetSymbol, true);
        }

        private bool TryGetOverrideByID(string id, out Override result)
        {
            result = overrideSymbols2.FirstOrDefault(s => s.id == id);
            return !result.Equals(default(Override));
        }

        public void DisableOverride()
        {
            animController.GetComponent<SymbolOverrideController>().RemoveAllSymbolOverrides();
        }

        public void SetRandomStage(Status quality)
        {
            if (quality == Status.Ready)
                return;

            var potantialOverrides = overrideSymbols2.Where(s => s.quality == quality).ToList();
            if (potantialOverrides.Count == 0)
                return;

            Override newOverride = potantialOverrides.GetRandom();
            currentSymbolOverride = newOverride.id;

            Debug.Log($"we have {potantialOverrides.Count} stages:");
            potantialOverrides.ForEach(s => Debug.Log(s.id));

            currentSymbolOverride = newOverride.id;
            EnableOverride(newOverride);

        }

        public void SetStage(string id)
        {
            Debug.Log($"we chose: {id}");
            if (TryGetOverrideByID(id, out Override currentOverride))
            {
                currentSymbolOverride = id;
                EnableOverride(currentOverride);
            }
        }
    }
}
