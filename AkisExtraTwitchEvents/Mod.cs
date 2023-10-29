using FUtility;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using System.Collections.Generic;
using System.Reflection;
using Twitchery.Content;
using Twitchery.Content.Scripts;
using Twitchery.Patches;

namespace Twitchery
{
	public class Mod : UserMod2
	{
		public static Components.Cmps<Toucher> touchers = new();
		public static Components.Cmps<GiantCrab> giantCrabs = new();
		public static Components.Cmps<RegularPip> regularPips = new();
		public static Components.Cmps<WereVoleContainer> wereVoles = new();
		public static Components.Cmps<AETE_PolymorphCritter> polys = new();
		public static Components.Cmps<MidasEntityContainer> midasContainers = new();
		public static Components.Cmps<MidasEntityContainer> midasContainersWithDupes = new();
		public static Components.Cmps<AETE_GraveStoneMinionStorage> graves = new();
		public static HashSet<MinionIdentity> doubledDupe = new();

		public static bool isBeachedHere;

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

			RegisterDevTool<AETE_DevTool>("Mods/Akis Extra Twitch Events");
		}

		private static MethodInfo m_RegisterDevTool;

		public static void RegisterDevTool<T>(string path) where T : DevTool, new()
		{
			if (m_RegisterDevTool == null)
			{
				m_RegisterDevTool = AccessTools.DeclaredMethod(typeof(DevToolManager), "RegisterDevTool", new[]
				{
					typeof(string)
				});

				if (m_RegisterDevTool == null)
				{
					Log.Warning("DevToolManager.RegisterDevTool couldnt be found, skipping adding dev tools.");
					return;
				}

				if (DevToolManager.Instance == null)
				{
					Log.Warning("DevToolManager.Instance is null, probably trying to call this too early. (try OnAllModsLoaded)");
					return;
				}

				m_RegisterDevTool
					.MakeGenericMethod(typeof(T))
					.Invoke(DevToolManager.Instance, new object[] { path });
			}
		}

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);
			TwitchDeckManagerPatch.TryPatch(harmony);

			foreach (var mod in mods)
			{
				if (mod.IsEnabledForActiveDlc())
				{
					if (mod.staticID == "Beached")
					{
						isBeachedHere = true;
						break;
					}
				}
			}

			PocketDimensionPatch.TryPatch(harmony);
			TPocketDimensions.Register();
			//TEMP.Patch(harmony);
		}
	}
}
