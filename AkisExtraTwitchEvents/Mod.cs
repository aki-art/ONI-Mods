﻿using FUtility;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using System;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery
{
    public class Mod : UserMod2
    {
        public static Components.Cmps<MidasToucher> midasTouchers = new();
        public static Components.Cmps<GiantCrab> giantCrabs = new ();

		public static Config Settings { get; private set; }

		public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            ModAssets.LoadAll();

			PUtil.InitLibrary(false);
			new POptions().RegisterOptions(this, typeof(Config));
			Settings = POptions.ReadSettings<Config>() ?? new Config();

			FUtility.Utils.RegisterDevTool<AETE_DevTool>("Mods/Akis Extra Twitch Events");
		}

		public static Vector3 ClampedPosWithRange(Vector3 position, int range)
		{
			var f = UnityEngine.Random.value * (float)Math.PI * 2f;
			var vector2 = range * Mathf.Sqrt(UnityEngine.Random.value) * new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
			var vector3 = position + vector2;
			var minimumBounds = ClusterManager.Instance.activeWorld.minimumBounds;
			var maximumBounds = ClusterManager.Instance.activeWorld.maximumBounds;

			return new Vector3(
                Mathf.Clamp(vector3.x, minimumBounds.x, maximumBounds.x), 
                Mathf.Clamp(vector3.y, minimumBounds.y, maximumBounds.y), 
                vector3.z);
		}
	}
}
