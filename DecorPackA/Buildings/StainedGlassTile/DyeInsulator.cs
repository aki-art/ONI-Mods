
using System.Collections.Generic;
using UnityEngine;
using static DecorPackA.STRINGS.UI.BUILDINGEFFECTS;

namespace DecorPackA.Buildings.StainedGlassTile
{
    public class DyeInsulator : KMonoBehaviour, IGameObjectEffectDescriptor
    {
        [MyCmpReq]
        private Deconstructable deconstructable;

        [MyCmpReq]
        private Building building;

        [MyCmpReq]
        KSelectable kSelectable;

        public float Modifier { get; private set; } = 1f;

        protected override void OnSpawn()
        {
            float TCTransparent = GetThermalConductivity(0);
            float TCDye = GetThermalConductivity(1);
            float ratio = Mod.Settings.GlassTile.DyeRatio;

            Modifier = Mathf.Pow(TCDye, ratio) * Mathf.Pow(TCTransparent, 1f - ratio) / TCTransparent;
            SetInsulation(Modifier);

            SetName();
        }

        private void SetName()
        {
            kSelectable.SetName(STRINGS.BUILDINGS.PREFABS.DECORPACKA_DEFAULTSTAINEDGLASSTILE.STAINED_NAME.Replace("{element}", GetElementName(1)));
        }

        private float GetThermalConductivity(int index)
        {
            return ElementLoader.GetElement(deconstructable.constructionElements[index]).thermalConductivity;
        }

        private string GetElementName(int index)
        {
            return deconstructable.constructionElements[index].ProperNameStripLink();
        }

        // reset insulation over this tile
        protected override void OnCleanUp()
        {
            SetInsulation(1f);
        }

        // sets insulation for the sim, because this tile is no using the standard thermal conduction rules
        private void SetInsulation(float value)
        {
            SimMessages.SetInsulation(building.GetCell(), value);
        }

        public List<Descriptor> GetDescriptors(GameObject go)
        {
            if (Modifier == 1f) return null;

            List<Descriptor> list = new List<Descriptor>();

            Descriptor item = new Descriptor();

            string percent = GameUtil.GetFormattedPercent(Modifier * 100f - 100f, GameUtil.TimeSlice.None);
            string comparator = Modifier < 1f ? TOOLTIP.LOWER : TOOLTIP.HIGHER;

            item.SetupDescriptor(
                string.Format(THERMALCONDUCTIVITYCHANGE, percent),
                TOOLTIP.THERMALCONDUCTIVITYCHANGE
                    .Replace("{dyeElement}", GetElementName(1))
                    .Replace("{higherOrLower}", comparator)
                    .Replace("{baseElement}", GetElementName(0))
                    .Replace("{percent}", percent),
                Descriptor.DescriptorType.Effect);

            list.Add(item);

            return list;
        }
    }
}
