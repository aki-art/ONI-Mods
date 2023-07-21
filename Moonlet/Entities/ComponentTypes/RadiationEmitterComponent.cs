using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	public class RadiationEmitterComponent : BaseComponent
	{
		public Vector2I Range { get; set; }

		public float Rads { get; set; }

		public float Rate { get; set; }

		public float Speed { get; set; }

		public float Angle { get; set; }

		public float Direction { get; set; }

		public RadiationEmitter.RadiationEmitterType EmitterType { get; set; }

		public Vector3F Offset { get; set; }

		public override void Apply(GameObject prefab)
		{
			if (!DlcManager.FeatureRadiationEnabled())
				return;

			var emitter = prefab.AddOrGet<RadiationEmitter>();
			emitter.emitRadiusX = (short)Mathf.Clamp((short)Range.X, 0, short.MaxValue);
			emitter.emitRadiusY = (short)Mathf.Clamp((short)Range.Y, 0, short.MaxValue);
			emitter.emitRads = Rads;
			emitter.emissionOffset = Offset;
			emitter.emitRate = Rate;
			emitter.emitSpeed = Speed;
			emitter.emitType = EmitterType;
			emitter.emitAngle = Angle;
			emitter.emitDirection = Direction;
		}
	}
}
