using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	internal class LightEmitterComponent : BaseComponent
	{
		public int Lux { get; set; }

		public int Range { get; set; }

		public string Color { get; set; }

		public bool ShowOverlay { get; set; }

		public Vector2I Offset { get; set; }

		public LightShape Shape { get; set; }

		public Vector2f Direction { get; set; }

		public bool WhenOperational { get; set; }

		public bool WhenHappy { get; set; }

		public LightEmitterComponent()
		{
			WhenOperational = true;
			Shape = LightShape.Circle;
			Offset = Vector2I.zero;
			ShowOverlay = false;
		}

		public override void Apply(GameObject prefab)
		{
			var light = prefab.AddComponent<Light2D>();
			light.Offset = Offset;
			light.Color = Color == null ? UnityEngine.Color.white : MiscUtil.ParseColor(Color);
			light.drawOverlay = ShowOverlay;
			light.shape = Shape;
			light.Direction = Direction;
			light.Range = Range;
			light.Lux = Lux;

			if (WhenOperational)
				prefab.AddOrGetDef<LightController.Def>();

			if (WhenHappy)
				prefab.AddOrGetDef<CreatureLightToggleController.Def>();
		}
	}
}
