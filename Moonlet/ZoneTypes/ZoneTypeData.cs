using UnityEngine;
using YamlDotNet.Serialization;
using static ProcGen.SubWorld;

namespace Moonlet.ZoneTypes
{
	public class ZoneTypeData
	{
		public string id {  get; set; }
		public string color { get; set; }
		public string border { get; set; }
		public string background { get; set; }

		[YamlIgnore] public ZoneType type;
		[YamlIgnore] public ZoneType borderType;
		[YamlIgnore] public Color32 color32;
		[YamlIgnore] public Texture2DArray texture;
	}
}
