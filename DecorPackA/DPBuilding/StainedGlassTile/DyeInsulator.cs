using System.Collections.Generic;
using UnityEngine;
using static DecorPackA.STRINGS.UI.BUILDINGEFFECTS;

namespace DecorPackA.DPBuilding.StainedGlassTile
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
            string name = Strings.Get("DecorPackA.STRINGS.BUILDINGS.PREFABS.DP_DEFAULTSTAINEDGLASSTILE.STAINED_NAME");
            kSelectable.SetName(name.Replace("{Element}", GetElement(1)));
        }

        private float GetThermalConductivity(int index)
        {
            return ElementLoader.GetElement(deconstructable.constructionElements[index]).thermalConductivity;
        }

        private string GetElement(int index)
        {
            return deconstructable.constructionElements[index].ProperNameStripLink();
        }

        protected override void OnCleanUp()
		{
			SetInsulation(1f);
		}

		private void SetInsulation(float value)
        {
			SimMessages.SetInsulation(building.GetCell(), value);
		}

        public List<Descriptor> GetDescriptors(GameObject go)
        {
            List<Descriptor> list = null;
            if (Modifier != 1f)
            {
                list = new List<Descriptor>();
                Descriptor item = new Descriptor();
                string percent = GameUtil.GetFormattedPercent(Modifier * 100f - 100f, GameUtil.TimeSlice.None);
                string comparator = Modifier > 0 ? TOOLTIP.HIGHER : TOOLTIP.LOWER;

                item.SetupDescriptor(
                    string.Format(THERMALCONDUCTIVITYCHANGE, percent), 
                    string.Format(TOOLTIP.THERMALCONDUCTIVITYCHANGE, GetElement(0), comparator, GetElement(1), percent), 
                    Descriptor.DescriptorType.Effect);
                list.Add(item);
            }
            return list;
        }
    }
}
