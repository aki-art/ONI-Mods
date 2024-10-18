using Moonlet.Templates;
using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using UnityEngine;

namespace Moonlet.Scripts.ComponentTypes
{
	public class LightEmitterComponent : BaseComponent
	{
		public LightEmitterData Data { get; set; }

		public class LightEmitterData
		{
			public IntNumber Lux { get; set; }

			public IntNumber Range { get; set; }

			public ColorEntry Color { get; set; }

			public bool ShowOverlay { get; set; }

			public Vector2FC Offset { get; set; }

			public IntNumber Width { get; set; }

			public string Shape { get; set; }

			public Vector2FC Direction { get; set; }

			public bool WhenOperational { get; set; }

			public bool WhenHappy { get; set; }

			public LightEmitterData()
			{
				WhenOperational = true;
				Shape = LightShape.Circle.ToString();
				Direction = Vector2FC.zero;
				Offset = Vector2FC.zero;
				ShowOverlay = false;
			}
		}

		public override void OnConfigureBuildingComplete(GameObject prefab)
		{
			base.OnConfigureBuildingComplete(prefab);
			Apply(prefab);
		}

		public override void OnConfigureBuildingTemplate(GameObject prefab)
		{
			prefab.AddTag(RoomConstraints.ConstraintTags.LightSource);
		}

		public override void OnConfigureBuildingPreview(GameObject prefab)
		{
			var lightShapePreview = prefab.AddOrGet<Moonlet_LightShapePreview>();
			var shape = EnumUtils.ParseOrDefault(Data.Shape, LightShape.Circle);
			var width = Data.Width.CalculateOrDefault(0);
			var direction = Data.Direction.ToDirection();
			var offset = new CellOffset((int)Data.Offset.Get("X"), (int)Data.Offset.Get("Y"));

			prefab.GetComponent<KPrefabID>().prefabSpawnFn += go =>
			{
				go.AddOrGet<Moonlet_LightShapePreview>().AddSource(Data.Range, Data.Lux, width, shape, direction, offset);
			};
		}

		public override void Apply(GameObject prefab)
		{
			var light = prefab.AddComponent<Light2D>();
			light.Offset = Data.Offset.ToVector2();
			light.Color = Data.Color;
			light.drawOverlay = Data.ShowOverlay;
			light.overlayColour = Data.Color;
			light.shape = EnumUtils.ParseOrDefault(Data.Shape, LightShape.Circle);
			light.Direction = Data.Direction.ToVector2();
			light.Range = Data.Range.CalculateOrDefault(1);
			light.Lux = Data.Lux.CalculateOrDefault(1000);
			light.Width = Data.Width.CalculateOrDefault(0);

			if (Data.WhenOperational && prefab.TryGetComponent(out Operational _))
				prefab.AddOrGetDef<Moonlet_LightController.Def>();

			if (Data.WhenHappy && prefab.HasTag(GameTags.CreatureBrain))
				prefab.AddOrGetDef<CreatureLightToggleController.Def>();
		}
	}
}
