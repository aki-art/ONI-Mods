using UnityEngine;
using YamlDotNet.Serialization;
using static ProcGen.SubWorld;

namespace Moonlet.ZoneTypes
{
	public class ZoneTypeData
	{
		public string Id {  get; set; }

		public string Color { get; set; }

		public string Border { get; set; }

		public string Background { get; set; }

		[YamlIgnore] public ZoneType type;
		[YamlIgnore] public ZoneType borderType;
		[YamlIgnore] public Color32 color32;
		[YamlIgnore] public Texture2DArray texture;
		[YamlIgnore] public string texturesFolder;
		[YamlIgnore] public int runTimeIndex;
	}
}
