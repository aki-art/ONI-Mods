using UnityEngine;
using UnityEngine.UI;

namespace FUtility
{
	public class ModDebug
	{
		public static LineRenderer AddSimpleLineRenderer(Transform transform, Color start, Color end, float width = 0.05f)
		{
			var gameObject = new GameObject("Beached_DebugLineRenderer");
			gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.SceneMAX));
			transform.SetParent(transform);

			gameObject.SetActive(true);

			var debugLineRenderer = gameObject.AddComponent<LineRenderer>();

			debugLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
			debugLineRenderer.material.renderQueue = 3501;
			debugLineRenderer.startColor = start;
			debugLineRenderer.endColor = end;
			debugLineRenderer.startWidth = debugLineRenderer.endWidth = width;
			debugLineRenderer.positionCount = 2;

			debugLineRenderer.GetComponent<LineRenderer>().material.renderQueue = RenderQueues.Liquid;

			return debugLineRenderer;
		}

		public static void Square(LineRenderer debugLineRenderer, Vector3 position, float radius)
		{
			debugLineRenderer.positionCount = 4;
			debugLineRenderer.loop = true;
			var z = Grid.GetLayerZ(Grid.SceneLayer.SceneMAX);

			debugLineRenderer.SetPositions(new[] {
				new Vector3(position.x - radius, position.y - radius, z),
				new Vector3(position.x + radius, position.y - radius, z),
				new Vector3(position.x + radius, position.y + radius, z),
				new Vector3(position.x - radius, position.y + radius, z)
			});
		}

		public static Text AddText(Vector3 position, Color color, string msg)
		{
			var gameObject = new GameObject();

			var rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.GetComponent<RectTransform>());
			gameObject.transform.SetPosition(position);
			rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);

			var text2 = gameObject.AddComponent<Text>();
			text2.font = global::Assets.DebugFont;
			text2.text = msg;
			text2.color = color;
			text2.horizontalOverflow = HorizontalWrapMode.Overflow;
			text2.verticalOverflow = VerticalWrapMode.Overflow;
			text2.alignment = TextAnchor.MiddleCenter;

			return text2;
		}
	}
}