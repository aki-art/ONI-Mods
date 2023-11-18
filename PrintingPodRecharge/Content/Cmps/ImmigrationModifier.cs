﻿using FUtility;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrintingPodRecharge.Content.Cmps
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ImmigrationModifier : KMonoBehaviour
	{
		[Serialize] private Bundle selectedBundle;
		[Serialize] public Bundle refundBundle;
		[Serialize] public bool hasHadLeaky;
		[Serialize] public bool migratedDupes;

		public bool IsOverrideActive;

		public Bundle ActiveBundle => IsOverrideActive ? selectedBundle : Bundle.None;

		public CarePackageBundle GetActiveCarePackageBundle()
		{
			if (ActiveBundle == Bundle.None)
				return null;

			return bundles.TryGetValue(ActiveBundle, out var result)
				? result
				: null;
		}

		public int maxItems = 4;
		public int dupeCount = 1;
		public int itemCount = 3;

		public bool randomColor = false;

		public static ImmigrationModifier Instance { get; private set; }

		private Dictionary<Bundle, CarePackageBundle> bundles = new Dictionary<Bundle, CarePackageBundle>();

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public CarePackageBundle GetBundle(Bundle bundle) => bundles[bundle];

		protected override void OnSpawn()
		{
			base.OnSpawn();

			if (selectedBundle != Bundle.None)
				SetModifier(selectedBundle);

			if (!migratedDupes)
				MigrateDupes();
		}

		private void MigrateDupes()
		{
			foreach (var identity in Components.LiveMinionIdentities.Items)
			{
				if (identity.TryGetComponent(out Accessorizer accessorizer))
				{
					var personality = TryGetOriginalPersonality(identity);

					if (personality != null)
					{
						identity.nameStringKey = personality.nameStringKey;
						identity.personalityResourceId = personality.Id;
						accessorizer.ApplyMinionPersonality(personality);
						accessorizer.ApplyAccessories();
						accessorizer.UpdateHairBasedOnHat();
					}
				}
			}

			migratedDupes = true;
		}

		private Personality TryGetOriginalPersonality(MinionIdentity identity)
		{
			Log.Debug("-------------------------------");

			var displayName = identity.GetComponent<KSelectable>().GetName();
			Log.Debug("name: " + displayName);
			Log.Debug("name string key: " + identity.nameStringKey);

			if (identity.nameStringKey != null)
			{
				var personality = Db.Get().Personalities.TryGet(identity.nameStringKey);
				return personality;
			}
			/*			foreach (var personality in Db.Get().Personalities.resources)
						{
							if(Strings.TryGet(personality.GetDescription(), out var name))
							{
								Log.Debug(name);
								if(displayName == name)
									return personality;
							}
						}*/

			return null;
		}

		public void LoadBundles() => BundleLoader.LoadBundles(ref bundles);

		public void SetRefund(Bundle bundle)
		{
			Log.Debug("set refund to " + bundle);
			refundBundle = bundle;
		}

		public void SetModifier(Bundle bundle)
		{
			Log.Debug("Set modifier to " + bundle.ToString());
			selectedBundle = bundle;

			if (bundle == Bundle.None)
			{
				IsOverrideActive = false;
				return;
			}

			SetRefund(bundle);
			IsOverrideActive = true;

			var current = bundles[selectedBundle];

			randomColor = selectedBundle == Bundle.Shaker;

			dupeCount = current.GetDupeCount();
			itemCount = current.GetItemCount();
		}

		public int GetDupeCount(int otherwise)
		{
			return IsOverrideActive ? dupeCount : otherwise;
		}

		public int GetItemCount(int otherwise)
		{
			return IsOverrideActive ? itemCount : otherwise;
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public CarePackageInfo GetRandomPackage()
		{
			if (bundles[selectedBundle]?.info == null)
			{
				return null;
			}

			var infos = bundles[selectedBundle].info.Where(i => i.requirement == null || i.requirement.Invoke()).ToList();

			if (infos == null || infos.Count == 0)
			{
				return null;
			}

			Log.Debug("Selecting package from " + infos.Count);

			return infos.GetRandom();
		}

		public bool IsBundleAvailable(Bundle bundle)
		{
			if (bundle == Bundle.Twitch)
			{
				return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || Mod.otherMods.IsTwitchIntegrationHere;
			}

			if (bundle == Bundle.Medicinal)
			{
				return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || Mod.otherMods.IsDiseasesExpandedHere;
			}

			return true;
		}

		public class CarePackageBundle
		{
			public List<CarePackageInfo> info;
			private int dupeCountMin;
			private int dupeCountMax;
			private int packageCountMin;
			private int packageCountMax;
			public Color printerBgTint;
			public Color printerBgTintGlow;
			public bool replaceAnim;
			public KAnimFile[] bgAnim;
			public bool alwaysAvailable;

			public CarePackageBundle(List<CarePackageInfo> info, int dupeCountMin, int dupeCountMax, int packageCountMin, int packageCountMax, Color bg, Color fx, bool alwaysAvailable, string bgAnim = "rpp_greyscale_dupeselect_kanim") : this(info, dupeCountMin, dupeCountMax, packageCountMin, packageCountMax, alwaysAvailable)
			{
				printerBgTint = bg;
				printerBgTintGlow = fx;
				replaceAnim = true;
				this.bgAnim = new KAnimFile[] { Assets.GetAnim(bgAnim) };
			}

			public CarePackageBundle(List<CarePackageInfo> info, int dupeCountMin, int dupeCountMax, int packageCountMin, int packageCountMax, bool alwaysAvailable)
			{
				this.info = info;
				this.dupeCountMin = dupeCountMin;
				this.dupeCountMax = dupeCountMax;
				this.packageCountMin = packageCountMin;
				this.packageCountMax = packageCountMax;
				this.alwaysAvailable = alwaysAvailable;
			}

			public int GetItemCount()
			{
				return UnityEngine.Random.Range(packageCountMin, packageCountMax + 1);
			}

			public int GetDupeCount()
			{
				return UnityEngine.Random.Range(dupeCountMin, dupeCountMax + 1);
			}
		}
	}
}
