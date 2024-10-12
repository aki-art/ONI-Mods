extern alias YamlDotNetButNew;
using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using static ElementLoader;

namespace Moonlet.Templates
{
	public class ElementTemplate : BaseTemplate
	{
		public string ElementId { get; set; }

		public FloatNumber SpecificHeatCapacity { get; set; }

		public FloatNumber ThermalConductivity { get; set; }

		public FloatNumber SolidSurfaceAreaMultiplier { get; set; }

		public FloatNumber LiquidSurfaceAreaMultiplier { get; set; }

		public FloatNumber GasSurfaceAreaMultiplier { get; set; }

		[Doc("The mass in kg-s this element spawns in at worldgen or from sandbox tools.")]
		public FloatNumber DefaultMass { get; set; } = 100;

		[Doc("The temperature this element spawns in at worldgen or from sandbox tools.")]
		public TemperatureNumber DefaultTemperature { get; set; }

		[Range(0, float.MaxValue)]
		public FloatNumber DefaultPressure { get; set; }

		public FloatNumber MolarMass { get; set; }

		public FloatNumber LightAbsorptionFactor { get; set; }

		public FloatNumber RadiationAbsorptionFactor { get; set; } = 1;

		public FloatNumber RadiationPer1000Mass { get; set; }

		[Doc("Transform into this element when the low temperature point is reached.")]
		public string LowTempTransitionTarget { get; set; }

		public TemperatureNumber LowTemp { get; set; }

		[Doc("Transform into this element when the high temperature point is reached.")]
		public string HighTempTransitionTarget { get; set; }

		public TemperatureNumber HighTemp { get; set; }

		[Doc("Secondary element to provide when the low temperature point is reached. This will be dropped as an ore/bottle item.")]
		public string LowTempTransitionOreId { get; set; }

		public FloatNumber LowTempTransitionOreMassConversion { get; set; }

		[Doc("Secondary element to provide when the high temperature point is reached. This will be dropped as an ore/bottle item.")]
		public string HighTempTransitionOreId { get; set; }

		public FloatNumber HighTempTransitionOreMassConversion { get; set; }

		[Doc("Periodically emit this element. Must be a gas or liquid.")]
		public string SublimateId { get; set; }

		[Doc("Visual FX to show when sublimation happens.", typeof(SpawnFXHashes))]
		public string SublimateFx { get; set; }

		public FloatNumber SublimateRate { get; set; }

		public FloatNumber SublimateEfficiency { get; set; }

		public FloatNumber SublimateProbability { get; set; }

		public FloatNumber OffGasPercentage { get; set; }

		[Doc("The category in storages under which this element will be listed.")]
		public string MaterialCategory { get; set; }

		public string[] Tags { get; set; }

		[Doc("When overriding an existing element, exlcude these tags")]
		public string[] RemoveTags { get; set; }

		public bool? IsDisabled { get; set; }

		public FloatNumber Strength { get; set; }

		public FloatNumber MaxMass { get; set; }

		[Doc("Affects digging ability, dig speed, meteor damage, pressure damage, etc.")]
		[Range(0, 255)]
		public IntNumber Hardness { get; set; }

		[Doc("Unused")]
		public FloatNumber Toxicity { get; set; }

		public FloatNumber LiquidCompression { get; set; }

		public FloatNumber Speed { get; set; }

		public FloatNumber MinHorizontalFlow { get; set; }

		public FloatNumber MinVerticalFlow { get; set; }

		[Doc("Unused")]
		public string ConvertId { get; set; }

		public FloatNumber Flow { get; set; }

		public IntNumber BuildMenuSort { get; set; }

		public Element.State State { get; set; }

		[Obsolete("Use DlcIds instead")]
		public string DlcId { get; set; }

		public string[] DlcIds { get; set; }

		public ElementComposition[] Composition { get; set; }

		// ------ additional fields --------------------------------------------------------------------

		public string DescriptionText { get; set; }

		public ColorEntry Color { get; set; } = new ColorEntry("FFFFFF");

		public ColorEntry UiColor { get; set; }

		public ColorEntry ConduitColor { get; set; }

		[Doc("Color of the falling block when affected by gravity (like sand).")]
		public ColorEntry UnstableColorTint { get; set; }

		[Doc("Copy the effects of this block. Like shininess from ores, or sparkly shine from glass.")]
		public string MaterialReference { get; set; }

		[Doc("Override the main texture png. Still expected in the same location.")]
		public string MainTexture { get; set; }

		[Doc("Set the main texture based on this vanilla material. The default UV scale will also be set to copy.")]
		public string MainTextureFromExisting { get; set; }

		[Doc("Texture to use for shinyness.")]
		public string SpecularTexture { get; set; }

		[Doc("Scale of the texture.")]
		public FloatNumber TextureUVScale { get; set; }

		[Doc("Texture to use with Specular. Not really a normal map, more like a mask which moves with the camera motion.")]
		public string NormalMapTexture { get; set; }

		[Doc("Color of the specular. Clamped to 0.0-1.0 in shader.")]
		public ColorEntry SpecularColor { get; set; }

		public string DebrisAnim { get; set; }

		[Doc("", typeof(Rottable.RotAtmosphereQuality))]
		public string RotAtmosphereQuality { get; set; }

		public string UnstableAnim { get; set; }

		public FloatNumber EyeIrritationStrength { get; set; }

		public List<ModifierEntry> Modifiers { get; set; }

		public EffectsEntry DuplicantEffects { get; set; }

		public AudioConfigEntry Audio { get; set; }

		public ElementTemplate()
		{
			SpecularColor = ColorEntry.WHITE;
			Color = ColorEntry.MISSING;
		}

		public class AudioConfigEntry
		{
			[Doc("If defined, copy this existing elements sound effects. By default, metal ores will copy Cuprite, and refined metals Copper. Defining any other property for the audio will overwrite this, so you can use this as a base to start off from, and override what you need.")]
			public string CopyElement { get; set; }

			[Doc("The sound fx when the camera is near a large pile of this element. Only for solids.")]
			public string AmbienceType { get; set; }

			public string SolidAmbienceType { get; set; }

			[Doc("The sound fx while a laser is mining this element.")]
			public string MiningSound { get; set; }

			public string MiningBreakSound { get; set; }

			[Doc("The sound fx when an item falls on this surface.")]
			public string OreBumpSound { get; set; }

			[Doc("The sound fx of walking on this surface.")]
			public string FloorEventAudioCategory { get; set; }

			public string CreatureChewSound { get; set; }
		}

		public class EffectsEntry
		{
			[Doc("Apply this effect on a duplicant when their head is submerged in this element.")]
			public EffectEntry SubmergedIn { get; set; }

			[Doc("Apply this effect when a duplicants feet are in this element, but but not their heads.")]
			public EffectEntry SteppedIn { get; set; }

			public EffectEntry BreathedIn { get; set; }

			public EffectEntry WalkedOn { get; set; }
		}
	}
}
