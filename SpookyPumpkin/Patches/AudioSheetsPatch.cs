using HarmonyLib;
using PeterHan.PLib.Core;
using System;

namespace SpookyPumpkinSO.Patches
{
    public class AudioSheetsPatch
    {
        [HarmonyPatch(typeof(AudioSheets), "Initialize")]
        public class AudioSheets_Initialize_Patch
        {
            public static void Prefix(AudioSheets __instance)
            {
                var ev = new AudioSheet.SoundInfo
                {
                    File = "sp_pip_licks_butt_kanim",
                    Anim = "lick_butt_loop",
                    Name0 = "HatchBaby_lick",
                    Frame0 = 5
                };

                __instance.sheets.Add(new AudioSheet()
                {
                    defaultType = "SoundEvent",
                    soundInfos = new AudioSheet.SoundInfo[] { ev }
                });

                PGameUtils.CopySoundsToAnim("sp_ghostpip_kanim", "squirrel_kanim");
            }
        }
    }
}
