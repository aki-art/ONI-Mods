namespace DecorPackA.DPBuilding.StainedGlassTile
{
    public class DyeInsulator : KMonoBehaviour
	{
		[MyCmpReq]
		private Deconstructable deconstructable;

        [MyCmpReq]
        private Building building;

        public float Modifier { get; private set; } = 1f;

        protected override void OnSpawn()
        {
            float TCTransparent = GetThermalConductivity(0);
            float TCDye = GetThermalConductivity(1);

            Modifier = TCDye / TCTransparent;
            SetInsulation(Modifier);
        }

        private float GetThermalConductivity(int index)
        {
            return ElementLoader.GetElement(deconstructable.constructionElements[index]).thermalConductivity;
        }

        protected override void OnCleanUp()
		{
			SetInsulation(1f);
		}

		private void SetInsulation(float value)
        {
			SimMessages.SetInsulation(building.GetCell(), value);
		}
	}
}
