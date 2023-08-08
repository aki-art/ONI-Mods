using FUtility;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using Twitchery.Patches;
using UnityEngine;

namespace Twitchery
{
    public class Mod : UserMod2
    {
        public static Components.Cmps<Toucher> touchers = new();
        public static Components.Cmps<GiantCrab> giantCrabs = new ();
        public static Components.Cmps<RegularPip> regularPips = new ();
        public static Components.Cmps<AETE_PolymorphCritter> polys = new ();
        public static Components.Cmps<MidasEntityContainer> midasContainers = new ();

		public static Config Settings { get; private set; }

		public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            ModAssets.LoadAll();

			PUtil.InitLibrary(false);
			new POptions().RegisterOptions(this, typeof(Config));
			Settings = POptions.ReadSettings<Config>() ?? new Config();

			if (Settings.Version <= 1 && Settings.MaxDupes == 30)
				Settings.MaxDupes = 40;

			Settings.Version = 1;

			FUtility.Utils.RegisterDevTool<AETE_DevTool>("Mods/Akis Extra Twitch Events");

		}

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);
			TwitchDeckManagerPatch.TryPatch(harmony);
			PocketDimensionPatch.TryPatch(harmony);
		}
	}
}
