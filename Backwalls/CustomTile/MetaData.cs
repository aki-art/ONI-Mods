using System;
using System.Collections.Generic;

namespace Backwalls.CustomTile
{
	public class MetaData
	{
		public string Name { get; set; } = "Backwalls.STRINGS.MISC.UNNAMED_BACKWALL_VARIANT";

		public string BorderColorHex { get; set; } = "888888";

		public string SpecularColor { get; set; } = "FFFFFF"; 

		//public ShaderSettings Shader { get; set; }

		public string BorderTag { get; set; }
/*
		[Serializable]
		public class ShaderSettings
		{
			public string Name { get; set; }

			public Dictionary<string, object> Data { get; set; }
		}*/
	}
}
