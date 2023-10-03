using Moonlet.Templates.SubTemplates;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using static ElementLoader;

namespace Moonlet.Templates
{
	public class ElementTemplate : ITemplate
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public int Priority { get; set; }

		public Dictionary<string, int> PriorityPerCluster { get; set; }

		// ------ default fields -----------------------------------------------------------------------

		public float? SpecificHeatCapacity { get; set; }

		public float? ThermalConductivity { get; set; }

		public float? SolidSurfaceAreaMultiplier { get; set; }

		public float? LiquidSurfaceAreaMultiplier { get; set; }

		public float? GasSurfaceAreaMultiplier { get; set; }

		public float? DefaultMass { get; set; }

		[YamlMember(Alias = "defaultTemperature")]
		public float? DefaultTemperatureKelvin { get; set; }

		public float? DefaultPressure { get; set; }

		public float? MolarMass { get; set; }

		public float? LightAbsorptionFactor { get; set; }

		public float? RadiationAbsorptionFactor { get; set; }

		public float? RadiationPer1000Mass { get; set; }

		public string LowTempTransitionTarget { get; set; }

		[YamlMember(Alias = "lowTemp")]
		public float? LowTempKelvin { get; set; }

		public string HighTempTransitionTarget { get; set; }

		[YamlMember(Alias = "highTemp")]
		public float? HighTempKelvin { get; set; }

		public string LowTempTransitionOreId { get; set; }

		public float? LowTempTransitionOreMassConversion { get; set; }

		public string HighTempTransitionOreId { get; set; }

		public float? HighTempTransitionOreMassConversion { get; set; }

		public string SublimateId { get; set; }

		public string SublimateFx { get; set; }

		public float? SublimateRate { get; set; }

		public float? SublimateEfficiency { get; set; }

		public float? SublimateProbability { get; set; }

		public float? OffGasPercentage { get; set; }

		public string MaterialCategory { get; set; }

		public string[] Tags { get; set; }

		public bool? IsDisabled { get; set; }

		public float? Strength { get; set; }

		public float? MaxMass { get; set; }

		public byte? Hardness { get; set; }

		public float? Toxicity { get; set; }

		public float? LiquidCompression { get; set; }

		public float? Speed { get; set; }

		public float? MinHorizontalFlow { get; set; }

		public float? MinVerticalFlow { get; set; }

		public string ConvertId { get; set; }

		public float? Flow { get; set; }

		public int? BuildMenuSort { get; set; }

		public Element.State State { get; set; }

		public string LocalizationID { get; set; }

		public string DlcId { get; set; }

		public ElementComposition[] Composition { get; set; }

		// ------ additional fields --------------------------------------------------------------------

		public string DescriptionText { get; set; }

		public string Color { get; set; }

		public string UiColor { get; set; }

		public string ConduitColor { get; set; }

		public string UnstableColorTint { get; set; }

		public string MaterialReference { get; set; }

		public string MainTexture { get; set; }

		public string MainTextureFromExisting { get; set; }

		public string MainTextureTint { get; set; }

		public string SpecularTexture { get; set; }

		public float? TextureUVScale { get; set; }

		public string NormalMapTexture { get; set; }

		public string SpecularColor { get; set; }

		public string DebrisAnim { get; set; }

		public string RotAtmosphereQuality { get; set; }

		public string GasTextureType { get; set; }

		public string UnstableAnim { get; set; }

		public float? EyeIrritationStrength { get; set; }

		public ModifierEntry Modifiers { get; set; }

		public EffectsEntry DuplicantEffects { get; set; }

		public AudioConfigEntry Audio { get; set; }

		public float? DefaultTemperatureCelsius { get; set; }

		public float? HighTempCelsius { get; set; }

		public float? LowTempCelsius { get; set; }

		public string WaterCoolerEffect { get; set; }

		public string WaterCoolerTooltip { get; set; }


		public class AudioConfigEntry
		{
			public string CopyElement { get; set; }

			public string AmbienceType { get; set; }

			public string SolidAmbienceType { get; set; }

			public string MiningSound { get; set; }

			public string MiningBreakSound { get; set; }

			public string OreBumpSound { get; set; }

			public string FloorEventAudioCategory { get; set; }

			public string CreatureChewSound { get; set; }
		}

		public class EffectsEntry
		{
			public EffectEntry SubmergedIn { get; set; }

			public EffectEntry SteppedIn { get; set; }

			public EffectEntry BreathedIn { get; set; }

			public EffectEntry WalkedOn { get; set; }
		}

		public class ModifierEntry
		{
			public float? Decor { get; set; }

			public float? OverHeat { get; set; }
		}
	}
}
