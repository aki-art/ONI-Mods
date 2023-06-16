using FUtility;
using HarmonyLib;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace PrintingPodRecharge.Content.Cmps
{
	// TODO: support modded hairstyles
	public class CustomDupe : KMonoBehaviour
	{
		public static Dictionary<MinionStartingStats, MinionData> rolledData = new Dictionary<MinionStartingStats, MinionData>();
		private static MethodInfo SuspendUpdates;
		private static object[] parameters = new object[] { false };

		public struct MinionData
		{
			public Color hairColor;
			public string name;
			public bool colorHair;
			public string descKey;
			public int hair;

			public MinionData(Color hairColor, string name, bool colorHair, string descKey, int hair)
			{
				this.hairColor = hairColor;
				this.name = name;
				this.colorHair = colorHair;
				this.descKey = descKey;
				this.hair = hair;
			}
		}

		[SerializeField]
		[Serialize]
		public Color hairColor;

		[Serialize]
		public bool initialized;

		[SerializeField]
		[Serialize]
		public string descKey; // used for base personality ID, but leaving the name for backwards compatibility reasons

		[Serialize]
		public bool dyedHair;

		[MyCmpReq]
		private KBatchedAnimController kbac;

		[MyCmpReq]
		private MinionIdentity identity;

		[MyCmpReq]
		private Accessorizer accessorizer;

		private HashedString serializedHair;

		[Serialize]
		public HashedString runtimeHair;

		[Serialize]
		public int hairID;

		[Serialize]
		public bool unColoredMeep;

		private bool forceUpdateAccessories;

		private static AccessTools.FieldRef<Accessorizer, List<ResourceRef<Accessory>>> ref_accessories;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			ref_accessories = AccessTools.FieldRefAccess<Accessorizer, List<ResourceRef<Accessory>>>("accessories");
		}

		[OnDeserialized]
		public void OnDeserialized()
		{
			forceUpdateAccessories = UpdateIdentity(identity);
		}

		public static bool UpdateIdentity(MinionIdentity identity)
		{
			if (identity == null)
				return false;

			if (identity.personalityResourceId == HashedString.Invalid
				|| identity.personalityResourceId == null
				|| Db.Get().Personalities.TryGet(identity.personalityResourceId) == null)

			{
				Log.Debuglog("personalityResourceId Invalid");
				Personality personality = null;

				if (Mod.otherMods.IsMeepHere)
				{
					personality = ModDb.Meep;
				}
				else
				{
					Log.Info("Updating duplicant. (the 2 above warnings about the body and arm resources can be ignored.)");
					// the mouth is the only reference left the the past skin color
					// try to find a matching one
					var mouth = identity.GetComponent<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Mouth);
					if (mouth != null)
					{
						var mouthId = mouth.Id.Replace("mouth_", "");
						if (int.TryParse(mouthId, out var number))
						{
							var personalities = new List<Personality>(Db.Get().Personalities.resources);
							personalities.Shuffle();
							personality = personalities.Find(p => p.mouth == number && !p.Disabled);
						}
					}
				}

				personality = personality ?? Db.Get().Personalities.GetRandom(true, false);
				identity.personalityResourceId = personality.Id;
				return true;
			}

			return false;
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();

			UpdateAccessories();
		}

		public void UpdateAccessories()
		{
			if (forceUpdateAccessories)
			{
				Log.Debuglog("force update");
				var p = Db.Get().Personalities.Get(identity.personalityResourceId);
				accessorizer.ApplyMinionPersonality(p);
			}

			accessorizer.UpdateHairBasedOnHat();

			if (!dyedHair)
			{
				return;
			}

			if (Strings.TryGet($"STRINGS.DUPLICANTS.PERSONALITIES.{descKey}.DESC", out var desc))
			{
				var key = "STRINGS.DUPLICANTS.PERSONALITIES." + identity.nameStringKey.ToUpperInvariant() + ".DESC";
				Strings.Add(key, desc.String);
			}

			TintHair(kbac, hairColor);

			var hashCache = HashCache.Get();
			serializedHair = hashCache.Add(hashCache.Get(accessorizer.bodyData.hair).Replace("hair_bleached", "hair"));
			OnLoadGame();
		}

		public void OnLoadGame()
		{
			if (dyedHair)
			{
				ChangeAccessorySlot(runtimeHair);
			}
		}

		public void OnSaveGame()
		{
			if (dyedHair)
			{
				ChangeAccessorySlot(serializedHair);
			}
		}

		[HarmonyPatch(typeof(Accessorizer), "ApplyAccessories")]
		public class Accessorizer_ApplyAccessories_Patch
		{
			public static void Prefix(Accessorizer __instance)
			{
				if (__instance.TryGetComponent(out CustomDupe dye) && dye.dyedHair)
				{
					dye.OnLoadGame();
				}
			}
		}

		// make sure a vanilla hair is saved as the body data, so if this mod is removed, these dupes can still load and exist
		private void ChangeAccessorySlot(HashedString value)
		{
			if (!value.IsValid)
			{
				return;
			}

			HashedString hatHairId = "hat_" + HashCache.Get().Get(value);
			var bodyData = accessorizer.bodyData;
			bodyData.hair = value;

			var items = ref_accessories(accessorizer);
			var slot = Db.Get().AccessorySlots.Hair;
			var slotHatHair = Db.Get().AccessorySlots.HatHair;
			var accessories = Db.Get().Accessories;

			for (var i = 0; i < items.Count; i++)
			{
				var item = items[i];
				var accessory = item.Get();

				if (accessory.slot == slot || accessory.slot == slotHatHair)
				{
					var targetAccessory = accessory.slot == slot ? value : hatHairId;
					items[i] = new ResourceRef<Accessory>(accessories.Get(targetAccessory));

					// force refresh the symbol
					var newAccessory = items[i].Get();
					kbac.GetComponent<SymbolOverrideController>().AddSymbolOverride(newAccessory.slot.targetSymbolId, newAccessory.symbol, 0);

					accessorizer.UpdateHairBasedOnHat();

					return;
				}
			}
		}

		public static bool Apply(KMonoBehaviour dupe, KBatchedAnimController kbac = null)
		{
			if (dupe != null && dupe.TryGetComponent(out CustomDupe dye))
			{
				if (dupe.TryGetComponent(out MinionIdentity identity))
				{
					identity.personalityResourceId = dye.descKey;
				}

				if (dye.dyedHair)
				{
					kbac = kbac ?? dupe.GetComponent<KBatchedAnimController>();
					if (kbac == null)
					{
						return false;
					}

					TintHair(kbac, dye.hairColor);
				}

				return true;
			}

			return false;
		}

		private static KAnimHashedString hair = new KAnimHashedString("snapTo_hair");
		private static KAnimHashedString hairAlways = new KAnimHashedString("snapTo_hair_always");
		private static KAnimHashedString hatHair = new KAnimHashedString("snapTo_hat_hair");

		public static void TintHair(KBatchedAnimController kbac, Color color)
		{
			if (kbac == null)
			{
				Log.Warning("Cannot dye the hair of a dupe with no KBatchedAnimController.");
				return;
			}

			if (!hair.IsValid() || !hairAlways.IsValid() || !hatHair.IsValid())
			{
				Log.Warning("Invalid hash");
				return;
			}

			TintHairInternal(kbac, color);
		}

		private const int TARGET_BATCH_ID = 0x11B1B1ED;

		private static void TintHairInternal(KBatchedAnimController kbac, Color color)
		{
			if (kbac == null || kbac.AnimFiles == null || kbac.AnimFiles.Length == 0)
			{
				return;
			}

			var groupID = kbac.batchGroupID;
			if (groupID.HashValue != TARGET_BATCH_ID)
			{
				var group = KAnimBatchManager.Instance()?.GetBatchGroupData(groupID);

				if (group == null)
				{
					Log.Warning("Batch group data is null", groupID);
					return;
				}
			}

			var accessorySlots = Db.Get().AccessorySlots;
			//kbac.SetSymbolTint(accessorySlots.Hair.targetSymbolId, color);
			var data = kbac.GetBatch()?.group?.data;

			var symbol = KAnimBatchManager.Instance()?.GetBatchGroupData(kbac.batchGroupID)?.GetSymbol(accessorySlots.Hair.targetSymbolId);
			if (symbol != null)
			{
				kbac.symbolInstanceGpuData.SetSymbolTint(symbol.symbolIndexInSourceBuild, color);

				if (SuspendUpdates == null)
				{
					SuspendUpdates = typeof(KBatchedAnimController).GetMethod("SuspendUpdates", BindingFlags.NonPublic | BindingFlags.Instance);
				}

				SuspendUpdates.Invoke(kbac, parameters);

				kbac.SetDirty();
			}
			else
			{
				Log.Debuglog("symbol was null");
			}

			kbac.SetSymbolTint("snapto_hair_always", color);
			kbac.SetSymbolTint(accessorySlots.HatHair.targetSymbolId, color);
		}
	}
}
