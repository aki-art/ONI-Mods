using Moonlet.Templates.SubTemplates;

namespace Moonlet.Templates
{
	public class TileTemplate : BuildingTemplate
	{
		public string TopsLayout { get; set; }

		public FloatNumber WalkSpeedMultiplier { get; set; }

		public EffectEntry WalkOnEffect { get; set; }

		public FloatNumber Strength { get; set; }

		public FloatNumber InsulationMultiplier { get; set; }

		public string Texture { get; set; }

		public string SpecularTexture { get; set; }

		public string PlaceTexture { get; set; }

		public string TopsTexture { get; set; }

		public string TopsSpecularTexture { get; set; }

		public string TopsPlaceTexture { get; set; }

		public bool AllowAir { get; set; }

		public bool AllowLiquid { get; set; }

		public bool Shiny { get; set; }

		public ColorEntry ShineColor { get; set; }

		public bool Transparent { get; set; }

		public bool ReplaceSimCellOccupier { get; internal set; }

		public TileTemplate() : base()
		{
			TopsLayout = "tiles_glass_tops_decor_info";
			ReplaceSimCellOccupier = true;
		}
	}
}
