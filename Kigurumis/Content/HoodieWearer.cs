using FUtility;
using HarmonyLib;
using KSerialization;
using System;
using System.Linq;
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
        private SnapOn snapOn;

        [MyCmpReq]
        private Accessorizer accessorizer;

        [Serialize]
        public bool isHoodieOn;

        KAnim.Build.Symbol symbol;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            UpdateHoodie();
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
                var hood = Assets.GetAnim("kigurumi_unicorn_hood_kanim");
                symbol = hood.GetData().build.GetSymbol("snapto_hat");
                symbolOverrideController.AddSymbolOverride(hatSymbolId, symbol, 2);

                // cut hair
                var hoodieHairAnim = Assets.GetAnim("kigurumihood_hair_swap_kanim");
                symbol = hoodieHairAnim.GetData().build.GetSymbol("hat_" + currentSymbol.ToString()); // the game also hardcodes this prefix, so mods shouldn't really deviate either

                //symbolOverrideController.AddSymbolOverride(symbolId, symbol, 2);

                GameScheduler.Instance.ScheduleNextFrame("test", obj =>
                {
                    symbolOverrideController.AddSymbolOverride(hatHairSymbolId, symbol, 2);
                });

            }
            else
            {
                symbolOverrideController.RemoveSymbolOverride("hat_hair_004");
                symbolOverrideController.RemoveSymbolOverride(hatSymbolId);
                // restore previous
            }

            //CropHair();
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
