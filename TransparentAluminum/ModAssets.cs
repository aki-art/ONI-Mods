using TransparentAluminum.Buildings.Borer;
using UnityEngine;

namespace TransparentAluminum
{
    public class ModAssets
    {
        public static void Load()
        {
            Textures.Alon.diffuse = FUtility.Assets.LoadTexture("slag_glass");
            Textures.Alon.specular = FUtility.Assets.LoadTexture("slag_glass_mask");
        }

        public static class Colors
        {
            public static readonly Color alon = new Color32(100, 188, 232, 30);
            public static readonly Color alonOpaque = new Color32(100, 188, 232, 255);
        }

        public static class Tags
        {
            public static readonly Tag TransparentAluminum = TagManager.Create("TransparentAluminum");
            public static readonly Tag Coating = TagManager.Create(Mod.Prefix("Coating"));
        }

        public static class Textures
        {
            public class Alon
            {
                public static Texture2D diffuse;
                public static Texture2D specular;
            }
        }

        public static class StatusItems
        {
            public static StatusItem drilling;
            public static StatusItem stuck;
            public static StatusItem falling;
            public static StatusItem integrity;

            public static void Register()
            {
                drilling = new StatusItem(Mod.Prefix("Drilling"), "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, false);
                stuck = new StatusItem(Mod.Prefix("Stuck"), "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID);
                falling = new StatusItem(Mod.Prefix("Falling"), "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, false);
                integrity = new StatusItem(Mod.Prefix("Integrity"), "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, false);
                integrity.SetResolveStringCallback((str, obj) =>
                {
                    if (obj is Borer borer)
                    {
                        float percent = borer.integrity / borer.maxIntegrity;
                        return str.Replace("{percent}", GameUtil.GetFormattedPercent(percent * 100f, GameUtil.TimeSlice.None));
                    }

                    return str;
                });
            }
        }
    }
}
