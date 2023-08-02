using System;
using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	public class GeneratorComponent : BaseComponent
	{
		public string Tag { get; set; }

		public bool Tinkerable { get; set; }

		public bool IgnoreBatteryRefillPercent { get; set; }

		public FormulaConfig Formula { get; set; }

		public class FormulaConfig
		{
			public Input[] Inputs { get; set; }

			public Output[] Outputs { get; set; }

			public float MinOutputTemperature { get; set; }
		}

		public class Input
		{
			public string Tag { get; set;}
			public float MassRate { get; set;}
			public float StoredMass { get; set; }
		}

		public class Output
		{
			public string Tag { get; set; }

			public float MassRate { get; set; }

			public bool StoreMass { get; set; }

			public Vector2I Offset { get; set; }
		}

		public GeneratorComponent()
		{

		}

		public override void Apply(GameObject prefab)
		{
			throw new NotImplementedException();
		}
	}
}
