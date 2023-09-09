using DecorPackA.UI;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using static STRINGS.UI.TOOLS;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class TintableLamp : KMonoBehaviour
	{
		[Serialize] public string colorHex;
		[Serialize] public int swatchIdx;

		[MyCmpReq] private MoodLamp moodLamp;

		private static readonly HashSet<KAnimHashedString> tintTargetSymbols = new()
		{
			"tintable",
			"tintable_bloom",
			"tintable_fg"
		};

		public Color Color { get; private set; }

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe(ModEvents.OnLampRefresh, _ => RefreshColor());
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
		}

		private void OnCopySettings(object obj)
		{
			if (((GameObject)obj).TryGetComponent(out TintableLamp tintable))
			{
				if (swatchIdx != SwatchSelector.Invalid)
					tintable.SetColor(swatchIdx);
				else
					tintable.SetColor(Color);
			}
		}

		private void RefreshColor()
		{
			if (swatchIdx != SwatchSelector.Invalid)
				SetColor(swatchIdx);
			else
				SetColor(Util.ColorFromHex(colorHex));
		}

		private Color GetLightColor(Color color) => (color * 2.55f) with { a = 1f };

		public TintableLamp()
		{
			swatchIdx = SwatchSelector.Invalid;
		}

		public override void OnCmpEnable()
		{
			if (colorHex.IsNullOrWhiteSpace() && swatchIdx == SwatchSelector.Invalid)
				colorHex = Random.ColorHSV(0, 1, 0.4f, 1, 0.5f, 0.9f).ToHexString();

			base.OnCmpEnable();
			RefreshColor();
		}

		public override void OnCmpDisable()
		{
			base.OnCmpDisable();
			TintKbacs(Color.white);
		}

		private void TintKbacs(Color color)
		{
			foreach (var symbol in tintTargetSymbols)
			{
				moodLamp.lampKbac.SetSymbolTint(symbol, color);
				if (moodLamp.secondaryLampKbac != null)
					moodLamp.secondaryLampKbac.SetSymbolTint(symbol, color);
			}
		}

		public void SetColor(int index)
		{
			Log.Debuglog("setting color from index: " + index);
			if (index == SwatchSelector.Invalid)
				return;

			swatchIdx = index;
			SetColor(ModAssets.colors[index]);
		}

		public void SetColor(Color color)
		{
			Log.Debuglog("setting color from color: " + color.ToString());
			colorHex = color.ToHexString();
			moodLamp.SetLightColor(GetLightColor(color));
			Color = color;
			TintKbacs(color);

			Trigger(ModEvents.OnLampTinted, color);
		}
	}
}
