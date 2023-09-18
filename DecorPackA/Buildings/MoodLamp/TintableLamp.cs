using DecorPackA.UI;
using HarmonyLib;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class TintableLamp : KMonoBehaviour
	{
		[Serialize] public string colorHex;
		[Serialize] public int swatchIdx;
		[Serialize] public bool initialized;

		[MyCmpReq] private MoodLamp moodLamp;

		public bool IsActive { get; private set; }

		public HashSet<KAnimHashedString> tintTargetSymbols = new()
		{
			"tintable",
			"tintable_bloom",
			"tintable_fg"
		};

		public Color Color { get; private set; }

		public TintableLamp()
		{
			swatchIdx = SwatchSelector.Invalid;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			Subscribe(ModEvents.OnMoodlampChangedEarly, OnMoodlampChanged);
			Subscribe(ModEvents.OnLampRefreshedAnimation, _ =>
			{
				Log.Debuglog("refreshing animation");
				if (IsActive)
				{
					Log.Debuglog("refreshed animation");
					RefreshColor();
				}
				else Log.Debuglog("no tag");
			});

			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			GameScheduler.Instance.ScheduleNextFrame("update moodlamp tint", _ =>
			{
				if (IsActive)
					RefreshColor();
			});
		}

		private void OnMoodlampChanged(object data)
		{
			Log.Debuglog("tinable on moodlamp changed");

			var isBeingTurnedOn = LampVariant.HasTag(data, LampVariants.TAGS.TINTABLE);

			if (isBeingTurnedOn)
			{
				Log.Debuglog("isBeingTurnedOn");
				var targetSymbols = LampVariant.GetCustomDataOrDefault<HashSet<KAnimHashedString>>(data, "Tintable_TargetSymbols", null);

				if (targetSymbols != null)
					tintTargetSymbols = targetSymbols;
			}
			else if (IsActive)
			{
				Log.Debuglog("not isBeingTurnedOn, IsActive");
				// if we are turning this component off, and was active before, reset tints to white
				TintKbacs(Color.white);
			}

			IsActive = isBeingTurnedOn;

			if (IsActive)
				RefreshColor();
		}

		private void OnCopySettings(object obj)
		{
			if (IsActive && ((GameObject)obj).TryGetComponent(out TintableLamp tintable))
			{
				SetColor(tintable.Color);
				RefreshColor();
			}
		}

		private void RefreshColor()
		{
			Log.Debuglog("refresing color");
			if (swatchIdx != SwatchSelector.Invalid)
				SetColor(swatchIdx);
			else
			{
				if (colorHex.IsNullOrWhiteSpace())
					colorHex = Random.ColorHSV(0, 1, 0.4f, 1, 0.5f, 0.9f).ToHexString();

				SetColor(Util.ColorFromHex(colorHex));
			}
		}

		private Color GetLightColor(Color color) => (color * 2.55f) with { a = 1f };

		private void TintKbacs(Color color)
		{
			Log.Debuglog("tint kbacs" + color.ToString());

			if (!IsActive)
				return;

			if (tintTargetSymbols == null)
			{
				Log.Warning($"Tintable lamp {moodLamp.currentVariantID} has no target symbols defined.");
				return;
			}

			foreach (var symbol in tintTargetSymbols)
			{
				moodLamp.lampKbac.SetSymbolTint(symbol, color);
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
			Log.Debuglog("Tintable SetColor " + color.ToString());
			colorHex = color.ToHexString();
			Color = color;

			if (IsActive)
			{
				moodLamp.SetLightColor(GetLightColor(color));
				TintKbacs(color);
			}

			Trigger(ModEvents.OnLampTinted, color);
		}
	}
}
