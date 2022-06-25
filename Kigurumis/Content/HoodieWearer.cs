using FUtility;
using HarmonyLib;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static KBatchedAnimInstanceData;

namespace Kigurumis.Content
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class HoodieWearer : KMonoBehaviour
    {
        [MyCmpReq]
        private KBatchedAnimController kbac;

        [MyCmpReq]
        private SymbolOverrideController symbolOverrideController;

        [MyCmpReq]
        private MinionResume resume;

        [MyCmpReq]
        private MinionIdentity identity;

        [MyCmpReq]
        private SnapOn snapOn;

        [MyCmpReq]
        private Accessorizer accessorizer;

        [Serialize]
        public bool isHoodieOn;

        [Serialize]
        public HoodieState state;

        [Serialize]
        public bool currentClothingHasHoodie;

        private KAnim.Build.Symbol hatSymbol;
        private KAnim.Build.Symbol bodySymbol;

        public Equippable equippable;

        public Storage kigurumiStorage;

        [SerializeField]
        [Serialize]
        public string facadeID;

        public enum HoodieState
        {
            Unset,
            On,
            Down
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (state == HoodieState.Unset)
            {
                isHoodieOn = Mod.Settings.HoodieDefaultState != Config.HoodieState.Never;
            }

            UpdateHoodie();

            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);

            ConfigureStorage();
        }

        public void OnEquip(Equippable equippable, string ID, bool puttingOnHood)
        {
            if(kigurumiStorage == null)
            {
                ConfigureStorage();
            }
        }

        private void ConfigureStorage()
        {
            foreach (var storage in GetComponents<Storage>())
            {
                if (storage.name == "kigurumiStorage")
                {
                    kigurumiStorage = storage;
                }
            }

            if (kigurumiStorage == null)
            {
                kigurumiStorage = gameObject.AddComponent<Storage>();
                kigurumiStorage.capacityKg = 1000;
                kigurumiStorage.showInUI = true;
                kigurumiStorage.storageFilters = new List<Tag>() { GameTags.Clothes };
                kigurumiStorage.name = "kigurumiStorage";
            }
        }

        private void OnRefreshUserMenu(object obj)
        {
            if (gameObject.HasTag(GameTags.Dead))
            {
                return;
            }

            var text = "Toggle Hoodie";
            var toolTip = "";

            var button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", text, OnToggleHoodie, tooltipText: toolTip);
            Game.Instance.userMenu.AddButton(gameObject, button);
        }

        private void OnToggleHoodie()
        {
            state = isHoodieOn ? HoodieState.Down : HoodieState.On;
            ToggleHoodie(!isHoodieOn);
        }

        public void UpdateHoodie()
        {
            ToggleHoodie(isHoodieOn);
        }

        public void ToggleHoodie(bool on)
        {
            isHoodieOn = on;

            if (Mod.Settings.HoodieDefaultState == Config.HoodieState.Never)
            {
                return;
            }

            if (Mod.Settings.HoodieDefaultState == Config.HoodieState.HatsHavePriority && IsWearingHat())
            {
                return;
            }

            SetSymbol();

            var hatHairSymbolId = Db.Get().AccessorySlots.HatHair.targetSymbolId;
            var hatSymbolId = Db.Get().AccessorySlots.Hat.targetSymbolId;

            kbac.SetSymbolVisiblity(hatSymbolId, on);
            kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, on);
            kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, !on);

            var currentSymbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash;

            Log.Debuglog("CURRENT SYMBOL " + currentSymbol.ToString());

            if (on)
            {
                // put on hoodie

                var facade = Db.Get().EquippableFacades.Get(facadeID);
                Log.Debuglog("FACADE ID IS " + facadeID);

                var hood = Assets.GetAnim(facadeID + "_hood_kanim");
                hatSymbol = hood.GetData().build.GetSymbol("snapto_hat");
                symbolOverrideController.AddSymbolOverride(hatSymbolId, hatSymbol, 2);

                var animFileName = "none";

                foreach (var overr in symbolOverrideController.GetSymbolOverrides)
                {
                    if (overr.targetSymbol.HashValue == hatHairSymbolId.HashValue)
                    {
                        animFileName = overr.sourceSymbol.build.name;
                    }
                }

                if(animFileName.IsNullOrWhiteSpace())
                {
                    Log.Warning("No Anim override for HatHair, skipping kigurumi hair styling.");
                    return;
                }

                // cut hair");
                var hoodieHairAnim = Assets.GetAnim("kigurumihood_" + animFileName + "_kanim");
                hatSymbol = hoodieHairAnim.GetData().build.GetSymbol("hat_" + currentSymbol.ToString()); // the game also hardcodes this prefix, so mods shouldn't really deviate either
                
                Log.Debuglog($"CURRENT ANIM IS {animFileName}");

                GameScheduler.Instance.ScheduleNextFrame("test", obj =>
                {
                    symbolOverrideController.AddSymbolOverride(hatHairSymbolId, hatSymbol, 2);
                });

            }
            else
            {
                symbolOverrideController.RemoveSymbolOverride("hat_hair_004");
                symbolOverrideController.RemoveSymbolOverride(hatSymbolId);
                // restore previous

                var hoodlessFacade = Db.Get().EquippableFacades.Get(facadeID + "_hoodless");

                equippable = identity.GetEquipment().GetSlot(Db.Get().AssignableSlots.Outfit)?.assignable as Equippable;
                identity.GetEquipment().Unequip(equippable);
            }


            if (equippable != null)
            {
                equippable = identity.GetEquipment().GetSlot(Db.Get().AssignableSlots.Outfit)?.assignable as Equippable;
            }


            //EquippableFacade.AddFacadeToEquippable(equippable, on ? facadeID : facadeID + "_hoodless");

            //CropHair();
        }

        public void SetEquippableBody(Equippable equippable)
        {
            this.equippable = equippable;
        }

        private void SetSymbol()
        {
            //var swapAnim = kbac.GetCurrentAnim().animFile;


        }

        private bool IsWearingHat()
        {
            return !HasSkillHat() && !HasHatSnapOn();
        }

        private bool HasSkillHat()
        {
            return resume.CurrentHat.IsNullOrWhiteSpace();
        }

        private bool HasHatSnapOn()
        {
            foreach (var snapPoint in snapOn.snapPoints)
            {
                if (snapPoint.overrideSymbol == "snapTo_Hat")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
