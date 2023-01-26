using System.IO;
using System.Reflection;
using UnityEngine;

namespace CrittersDropBones
{
    internal class ModAssets
    {
        public class Tags
        {
            public static Tag Bones = TagManager.Create("cdb_Bone");
        }

        public class StatusItems
        {
            public static StatusItem needsStirring;

            public static void Register()
            {
                needsStirring = new StatusItem(
                   "CrittersDropBones_NeedsStirringStatus",
                   "BUILDING",
                   "status_item_pending_switch_toggle",
                   StatusItem.IconType.Custom,
                   NotificationType.BadMinor,
                   false,
                   OverlayModes.None.ID);
            }
        }

        public static void LoadKanims()
        {
            var directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "anim", "interact_anims");

            var anim = LoadFile("anim_interacts_artifact_analysis_anim", Path.Combine(directory, "cooking_pot"));
            var buildBytes = LoadFile("interact_build", directory); // there is probably a way to get this from already loaded anims

            var sampleAnim = Assets.GetAnim("anim_interacts_compost_kanim");

            ModUtil.AddKAnim("anim_interacts_cookingpot_kanim", anim, buildBytes, sampleAnim.textureList);
        }

        public static TextAsset LoadFile(string name, string directory = null)
        {
            var path = Path.Combine(directory, name + ".bytes");

            if (File.Exists(path))
            {
                return new TextAsset(File.ReadAllText(path));
            }
            else
            {
                Debug.LogError($"Could not load file at path {path}.");
            }

            return null;
        }
    }
}
