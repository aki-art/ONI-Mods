using System;

namespace Moonlet.Templates.CodexTemplates
{
	public class ImageEntry : BaseWidgetTemplate
	{
		public string Sprite { get; set; }

		public ColorEntry Color { get; set; }

		public string BatchedAnimPrefabSourceID { get; set; }

		public int PreferredWidth { get; set; }
		public int PreferredHeight { get; set; }

		public override ICodexWidget Convert(Action<string> log = null)
		{
			var result = new CodexImage
			{
				preferredWidth = PreferredWidth,
				preferredHeight = PreferredHeight,
				spriteName = Sprite
			};

			if (BatchedAnimPrefabSourceID != null)
				result.batchedAnimPrefabSourceID = BatchedAnimPrefabSourceID;

			result.color = Color.value;

			return result;
		}
	}
}
