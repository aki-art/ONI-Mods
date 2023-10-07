using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using static ElementLoader;

namespace Moonlet.Templates
{
	public class ElementTemplate : ITemplate
	{
		[YamlMember(Alias = "elementId")]
		public string Id { get; set; }

		public string Name { get; set; }

		public string Priority { get; set; }

		public IntNumber TestExpression { get; set; }

		public Dictionary<string, string> PriorityPerCluster { get; set; }

		// ------ default fields -----------------------------------------------------------------------

		public FloatNumber SpecificHeatCapacity { get; set; }

		public FloatNumber ThermalConductivity { get; set; }

		public FloatNumber SolidSurfaceAreaMultiplier { get; set; }

		public FloatNumber LiquidSurfaceAreaMultiplier { get; set; }

		public FloatNumber GasSurfaceAreaMultiplier { get; set; }

		[DefaultValue(100)]
		public FloatNumber DefaultMass { get; set; }

		public TemperatureNumber DefaultTemperature { get; set; }

		[Range(0, float.MaxValue)]
		public FloatNumber DefaultPressure { get; set; }

		public FloatNumber MolarMass { get; set; }

		public FloatNumber LightAbsorptionFactor { get; set; }

		[DefaultValue(1)]
		public FloatNumber RadiationAbsorptionFactor { get; set; }

		public FloatNumber RadiationPer1000Mass { get; set; }

		public string LowTempTransitionTarget { get; set; }

		public TemperatureNumber LowTemp { get; set; }

		public string HighTempTransitionTarget { get; set; }

		public TemperatureNumber HighTemp { get; set; }

		public string LowTempTransitionOreId { get; set; }

		public FloatNumber LowTempTransitionOreMassConversion { get; set; }

		public string HighTempTransitionOreId { get; set; }

		public FloatNumber HighTempTransitionOreMassConversion { get; set; }

		public string SublimateId { get; set; }

		public string SublimateFx { get; set; }

		public FloatNumber SublimateRate { get; set; }

		public FloatNumber SublimateEfficiency { get; set; }

		public FloatNumber SublimateProbability { get; set; }

		public FloatNumber OffGasPercentage { get; set; }

		public string MaterialCategory { get; set; }

		public string[] Tags { get; set; }

		public bool? IsDisabled { get; set; }

		public FloatNumber Strength { get; set; }

		public FloatNumber MaxMass { get; set; }

		public byte? Hardness { get; set; }

		public FloatNumber Toxicity { get; set; }

		public FloatNumber LiquidCompression { get; set; }

		public FloatNumber Speed { get; set; }

		public FloatNumber MinHorizontalFlow { get; set; }

		public FloatNumber MinVerticalFlow { get; set; }

		public string ConvertId { get; set; }

		public FloatNumber Flow { get; set; }

		public int? BuildMenuSort { get; set; }

		public Element.State State { get; set; }

		[DefaultValue(DlcManager.VANILLA_ID)]
		public string DlcId { get; set; }

		public ElementComposition[] Composition { get; set; }

		// ------ additional fields --------------------------------------------------------------------

		public string DescriptionText { get; set; }

		[DefaultValue("FFFFFF")]
		public ColorEntry Color { get; set; }

		public ColorEntry UiColor { get; set; }

		public ColorEntry ConduitColor { get; set; }

		public ColorEntry UnstableColorTint { get; set; }

		public string MaterialReference { get; set; }

		public string MainTexture { get; set; }

		public string MainTextureFromExisting { get; set; }

		public string MainTextureTint { get; set; }

		public string SpecularTexture { get; set; }

		public FloatNumber TextureUVScale { get; set; }

		public string NormalMapTexture { get; set; }

		public ColorEntry SpecularColor { get; set; }

		public string DebrisAnim { get; set; }

		public string RotAtmosphereQuality { get; set; }

		public string GasTextureType { get; set; }

		public string UnstableAnim { get; set; }

		public FloatNumber EyeIrritationStrength { get; set; }

		public ModifierEntry Modifiers { get; set; }

		public EffectsEntry DuplicantEffects { get; set; }

		public AudioConfigEntry Audio { get; set; }

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
			public FloatNumber Decor { get; set; }

			public FloatNumber OverHeat { get; set; }
		}
	}
}
