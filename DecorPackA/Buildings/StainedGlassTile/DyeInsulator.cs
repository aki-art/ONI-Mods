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

            Modifier = Mathf.Sqrt(TCDye * TCTransparent) / TCTransparent;
            SetInsulation(Modifier);

            SetName();
        }

        private void SetName()
        {
            kSelectable.SetName(string.Format(STRINGS.BUILDINGS.PREFABS.DP_DEFAULTSTAINEDGLASSTILE.STAINED_NAME, GetElementName(1)));
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

            string percent = GameUtil.GetFormattedPercent(Modifier * 100f - 100f, GameUtil.TimeSlice.None);
            string comparator = Modifier > 0 ? TOOLTIP.HIGHER : TOOLTIP.LOWER;

            Descriptor item = new Descriptor();

            item.SetupDescriptor(
                string.Format(THERMALCONDUCTIVITYCHANGE, percent),
                string.Format(TOOLTIP.THERMALCONDUCTIVITYCHANGE, GetElementName(0), comparator, GetElementName(1), percent),
                Descriptor.DescriptorType.Effect);

            list.Add(item);

            return list;
        }
    }
}
