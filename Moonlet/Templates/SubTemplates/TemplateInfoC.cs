using System;

namespace Moonlet.Templates.SubTemplates
{
	[Serializable]
	public class TemplateInfoC : ShadowTypeBase<TemplateContainer.Info>
	{
		public Vector2FC Size { get; set; }
		public Vector2FC Min { get; set; }
		public int Area { get; set; }
		public Tag[] Tags { get; set; }

		public TemplateInfoC()
		{
			Size = new Vector2FC(0, 0);
			Min = new Vector2FC(0, 0);
		}

		public override TemplateContainer.Info Convert(Action<string> log = null)
		{
			return new TemplateContainer.Info()
			{
				area = Area,
				min = Min.ToVector2f(),
				size = Size.ToVector2f(),
				tags = Tags,
			};
		}
	}
}
