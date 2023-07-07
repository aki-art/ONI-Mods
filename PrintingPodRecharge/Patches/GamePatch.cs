using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
	public class GamePatch
	{

		[HarmonyPatch(typeof(Game), "OnSpawn")]
		public class Game_OnSpawn_Patch
		{
			public static void Postfix()
			{
				if (!Mod.Settings.HasShownDupesGoneMessage)
					ShowFarewell();
			}
		}

		private static void ShowFarewell()
		{
			var names = new List<string>();
			foreach (var minion in Components.MinionResumes.Items)
			{
				if (minion.TryGetComponent(out CustomDupe dupe) && dupe.dyedHair)
					names.Add(dupe.GetComponent<KSelectable>().name);
			}

			if (names.Count > 0)
			{
				var verb = names.Count == 1 ? "has" : "have";
				var duplicant = names.Count == 1 ? "duplicant" : "duplicants";

				var tex = FUtility.Assets.LoadTexture(Path.Combine(Utils.ModPath, "assets", "coffeepip.png"));
				var sprite = tex != null ? Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector3.zero) : null;

				var screen = Util.KInstantiateUI<InfoDialogScreen>(
					ScreenPrefabs.Instance.InfoDialogScreen.gameObject,
					FUtility.FUI.Helper.GetACanvas("dupe farewell").gameObject,
					true)
					.SetHeader("Bio-Inks")
					.AddDefaultOK()
					.AddPlainText($"Bio-Inks has retired the colorful duplicants feature. {names.Join()} {verb} been turned to regular {duplicant}, " +
					$"but their stats were unchanged. Suspicious Bio Ink now only affects traits and skills, not appearance.")
					.AddSpacer(10)
					.AddPlainText("Unfortunately this feature was taking up too much time to maintain, and I decided it is best to let it go, and focus on other mod projects instead.")
					.AddSpacer(10)
					.AddPlainText("Thank you for using this feature. I hope you will continue to enjoy the rest of the mod!")
					.AddSpacer(20)
					.AddPlainText("- Aki");

				if (sprite != null)
					screen.AddSprite(sprite);

				Mod.Settings.HasShownDupesGoneMessage = true;
				Mod.SaveSettings();
			}
		}

		[HarmonyPatch(typeof(Game), "Load")]
		public class Game_Load_Patch
		{
			public static void Postfix()
			{
				Integration.ArtifactsInCarePackages.SetData();
			}

		}
	}
}
