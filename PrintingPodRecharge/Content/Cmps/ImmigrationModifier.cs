using FUtility;
using KSerialization;
using PrintingPodRecharge.Content.Items;
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

		private Dictionary<Bundle, CarePackageBundle> bundles = [];

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public bool AreBionicDupesEnabled() => Db.Get().Personalities?.resources != null && Db.Get().Personalities.resources.Any(m => m.model == GameTags.Minions.Models.Bionic);

		public CarePackageBundle GetBundle(Bundle bundle) => bundles[bundle];

		public override void OnSpawn()
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
			return identity.nameStringKey != null ? Db.Get().Personalities.TryGet(identity.nameStringKey) : null;
		}

		public void LoadBundles() => BundleLoader.LoadBundles(ref bundles);

		public void SetRefund(Bundle bundle)
		{
			refundBundle = bundle;
		}

		public void SetModifier(Bundle bundle)
		{
			if (bundle == Bundle.Bionic && !DlcManager.IsContentSubscribed(CONSTS.DLC_BIONIC))
				return;

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

		public int GetDupeCount(int otherwise) => IsOverrideActive ? dupeCount : otherwise;

		public int GetItemCount(int otherwise) => IsOverrideActive ? itemCount : otherwise;

		public override void OnCleanUp()
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

			return infos.GetRandom();
		}

		public bool IsBundleAvailable(string prefabTag) => prefabTag switch
		{
			BioInkConfig.TWITCH => DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || Mod.otherMods.IsTwitchIntegrationHere,
			BioInkConfig.MEDICINAL => DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || Mod.otherMods.IsDiseasesExpandedHere,
			BioInkConfig.BIONIC => AreBionicDupesEnabled(),
			_ => true
		};

		public bool IsBundleAvailable(Bundle bundle) => bundle switch
		{
			Bundle.Twitch => DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || Mod.otherMods.IsTwitchIntegrationHere,
			Bundle.Medicinal => DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || Mod.otherMods.IsDiseasesExpandedHere,
			Bundle.Bionic => AreBionicDupesEnabled(),
			_ => true
		};

		internal bool IsBundleAvailable(Tag tag)
		{
			var bundle = Assets.GetPrefab(tag).GetComponent<BundleModifier>().bundle;
			return IsBundleAvailable(bundle);
		}

		public class CarePackageBundle
		{
			public List<CarePackageInfo> info;
			private int dupeCountMin;
			private int dupeCountMax;
			private int packageCountMin;
			private int packageCountMax;
			public List<Tag> permittedDupeModels;
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
				this.bgAnim = [Assets.GetAnim(bgAnim)];
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
				return Random.Range(packageCountMin, packageCountMax + 1);
			}

			public int GetDupeCount()
			{
				return Random.Range(dupeCountMin, dupeCountMax + 1);
			}
		}
	}
}
