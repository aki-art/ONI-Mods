using Moonlet.Templates;
using Moonlet.Utils;
using UnityEngine;

namespace Moonlet.Scripts.ComponentTypes
{
	public class RadiationEmitterComponent : BaseComponent
	{
		public RadiationEmitterData Data { get; set; }

		public class RadiationEmitterData
		{
			public Vector2I Range { get; set; }

			public FloatNumber Rads { get; set; }

			public FloatNumber Rate { get; set; }

			public FloatNumber Speed { get; set; }

			public FloatNumber Angle { get; set; }

			public FloatNumber Direction { get; set; }

			public string EmitterType { get; set; }

			public Vector3F Offset { get; set; }
		}

		public override void Apply(GameObject prefab)
		{
			if (!DlcManager.FeatureRadiationEnabled())
				return;

			var emitter = prefab.AddOrGet<RadiationEmitter>();
			emitter.emitRadiusX = (short)Mathf.Clamp((short)Data.Range.X, 0, short.MaxValue);
			emitter.emitRadiusY = (short)Mathf.Clamp((short)Data.Range.Y, 0, short.MaxValue);
			emitter.emitRads = Data.Rads.CalculateOrDefault(1000);
			emitter.emissionOffset = Data.Offset;
			emitter.emitRate = Data.Rate.CalculateOrDefault(1);
			emitter.emitSpeed = Data.Speed.CalculateOrDefault(1);
			emitter.emitType = EnumUtils.ParseOrDefault(Data.EmitterType, RadiationEmitter.RadiationEmitterType.Constant);
			emitter.emitAngle = Data.Angle.CalculateOrDefault(0);
			emitter.emitDirection = Data.Direction.CalculateOrDefault(0);
		}
	}
}
