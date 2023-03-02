using DecorPackA;
using DecorPackA.Buildings.MoodLamp;
using KSerialization;
using System;
using static DecorPackA.STRINGS.UI.USERMENUACTIONS.HAMIS_MAID;

namespace Buildings.MoodLamp
{
    [SerializationConfig(MemberSerialization.OptIn)]
    internal class Hamis : KMonoBehaviour
    {
        [Serialize] public bool isMaid;
        [MyCmpReq] public DecorPackA.Buildings.MoodLamp.MoodLamp moodLamp;
        public const string HAMIS_ID = "hamis";
        private const string MAID_ANIM_ON = "hamis_maid_on";
        private const string MAID_ANIM_OFF = "hamis_maid_off";

        public override void OnSpawn()
        {
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
        }

        private void OnRefreshUserMenu(object obj)
        {
            if (moodLamp.currentVariantID == HAMIS_ID)
            {
                var text = isMaid ? DISABLED.NAME : ENABLED.NAME;
                var toolTip = isMaid ? DISABLED.TOOLTIP : ENABLED.TOOLTIP;

                var button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", text, OnToggleMaid, tooltipText: toolTip);

                Game.Instance.userMenu.AddButton(gameObject, button);
            }
        }

        public string GetAnimOverride(bool on)
        {
            if(!isMaid)
            {
                var variant = ModDb.lampVariants.TryGet(HAMIS_ID);
                if(variant != null)
                {
                    return on ? variant.on : variant.off;
                }
            }

            return on ? MAID_ANIM_ON : MAID_ANIM_OFF;
        }

        private void OnToggleMaid()
        {
            isMaid = !isMaid;
            moodLamp.RefreshAnimation();
        }
    }
}
