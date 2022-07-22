using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls
{
    public class DyeSideScreen : SideScreenContent
    {
        public static readonly List<Color> colors = new List<Color>
		{
			new Color(0.4862745f, 0.4862745f, 0.4862745f),
			new Color(0f, 0f, 0.9882353f),
			new Color(0f, 0f, 0.7372549f),
			new Color(0.26666668f, 0.15686275f, 0.7372549f),
			new Color(0.5803922f, 0f, 0.5176471f),
			new Color(0.65882355f, 0f, 0.1254902f),
			new Color(0.65882355f, 0.0627451f, 0f),
			new Color(0.53333336f, 0.078431375f, 0f),
			new Color(0.3137255f, 0.1882353f, 0f),
			new Color(0f, 0.47058824f, 0f),
			new Color(0f, 0.40784314f, 0f),
			new Color(0f, 0.34509805f, 0f),
			new Color(0f, 0.2509804f, 0.34509805f),
			new Color(0f, 0f, 0f),
			new Color(0.7372549f, 0.7372549f, 0.7372549f),
			new Color(0f, 0.47058824f, 0.972549f),
			new Color(0f, 0.34509805f, 0.972549f),
			new Color(0.40784314f, 0.26666668f, 0.9882353f),
			new Color(0.84705883f, 0f, 0.8f),
			new Color(0.89411765f, 0f, 0.34509805f),
			new Color(0.972549f, 0.21960784f, 0f),
			new Color(0.89411765f, 0.36078432f, 0.0627451f),
			new Color(0.6745098f, 0.4862745f, 0f),
			new Color(0f, 0.72156864f, 0f),
			new Color(0f, 0.65882355f, 0f),
			new Color(0f, 0.65882355f, 0.26666668f),
			new Color(0f, 0.53333336f, 0.53333336f),
			new Color(0f, 0f, 0f),
			new Color(0.972549f, 0.972549f, 0.972549f),
			new Color(0.23529412f, 0.7372549f, 0.9882353f),
			new Color(0.40784314f, 0.53333336f, 0.9882353f),
			new Color(0.59607846f, 0.47058824f, 0.972549f),
			new Color(0.972549f, 0.47058824f, 0.972549f),
			new Color(0.972549f, 0.34509805f, 0.59607846f),
			new Color(0.972549f, 0.47058824f, 0.34509805f),
			new Color(0.9882353f, 0.627451f, 0.26666668f),
			new Color(0.972549f, 0.72156864f, 0f),
			new Color(0.72156864f, 0.972549f, 0.09411765f),
			new Color(0.34509805f, 0.84705883f, 0.32941177f),
			new Color(0.34509805f, 0.972549f, 0.59607846f),
			new Color(0f, 0.9098039f, 0.84705883f),
			new Color(0.47058824f, 0.47058824f, 0.47058824f),
			new Color(0.9882353f, 0.9882353f, 0.9882353f),
			new Color(0.6431373f, 0.89411765f, 0.9882353f),
			new Color(0.72156864f, 0.72156864f, 0.972549f),
			new Color(0.84705883f, 0.72156864f, 0.972549f),
			new Color(0.972549f, 0.72156864f, 0.972549f),
			new Color(0.972549f, 0.72156864f, 0.7529412f),
			new Color(0.9411765f, 0.8156863f, 0.6901961f),
			new Color(0.9882353f, 0.8784314f, 0.65882355f),
			new Color(0.972549f, 0.84705883f, 0.47058824f),
			new Color(0.84705883f, 0.972549f, 0.47058824f),
			new Color(0.72156864f, 0.972549f, 0.72156864f),
			new Color(0.72156864f, 0.972549f, 0.84705883f),
			new Color(0f, 0.9882353f, 0.9882353f),
			new Color(0.84705883f, 0.84705883f, 0.84705883f)
		};

        private ColorToggle togglePrefab;
		private Transform content;
		private ToggleGroup toggleGroup;
		private Dyeable target;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            content = transform.Find("Contents/ColorGrid");
            togglePrefab = content.Find("SwatchPrefab").gameObject.AddComponent<ColorToggle>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            toggleGroup = content.FindOrAddComponent<ToggleGroup>();

            foreach (var color in colors)
            {
                var toggle = Instantiate(togglePrefab, content);
				toggle.Setup(color);
                toggle.group = toggleGroup;
                toggleGroup.RegisterToggle(toggle);
                toggle.onValueChanged.AddListener(value => toggle.OnToggle(value, target));
                toggle.gameObject.SetActive(true);
            }
        }

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);
			this.target = target.GetComponent<Dyeable>();
        }

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<Dyeable>() != null;

        class ColorToggle : Toggle
        {
            public Color color;
			private Outline outline;

			public void Setup(Color color)
            {
				this.color = color;
				outline = GetComponent<Outline>();
				GetComponent<Image>().color = color;
			}

			public void OnToggle(bool on, Dyeable target)
			{
				var border = on ? 2f : 1f;
				var color = on ? Color.cyan : Color.black;

				outline.effectDistance = Vector2.one * border;
				outline.effectColor = color;

				target.SetColor(this.color);
			}
        }
    }
}
