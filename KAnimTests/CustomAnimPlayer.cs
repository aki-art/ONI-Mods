using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAnimTests
{
    public class CustomAnimPlayer : KMonoBehaviour
    {
        public static string animFile = "anim_cheer_kanim";
        public static string animation = "cheer_pre\ncheer_loop\ncheer_pst";
        public static string message = "";

        [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
        public class SaveGame_OnPrefabInit_Patch
        {
            public static void Postfix(SaveGame __instance)
            {
                __instance.FindOrAdd<CustomAnimPlayer>();
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 240, 200, 500));

            GUILayout.Label("Anim file");
            animFile = GUILayout.TextField(animFile);
            GUILayout.Label("Animations to play");
            animation = GUILayout.TextArea(animation);

            if (GUILayout.Button("Play") && !animFile.IsNullOrWhiteSpace() && !animation.IsNullOrWhiteSpace())
            {
                PlayAnim(false);
            }

            if (GUILayout.Button("Play All") && !animFile.IsNullOrWhiteSpace())
            {
                PlayAnim(true);
            }

            GUILayout.Label(message);

            if (GUILayout.Button("Dump anim names to Log"))
            {
                Console.WriteLine("Animation names dump:");

                foreach (var anim in Assets.Anims)
                {
                    if (anim.batchTag.HashValue == -1371425853)
                    {
                        Console.WriteLine(anim.name);

                        var data = anim.GetData();
                        for (var i = 0; i < data.animCount; i++)
                        {
                            Console.WriteLine("\t- " + data.GetAnim(i).name);
                        }
                    }
                }
            }

            GUILayout.EndArea();
        }

        private static void PlayAnim(bool all)
        {
            message = "";

            if (animation.IsNullOrWhiteSpace())
            {
                message = "anim file must not be empty";
                return;
            }

            var animName = animFile;
            if (!animName.EndsWith("_kanim"))
            {
                animName += "_kanim";
            }

            var kAnimFile = Assets.GetAnim(animName);

            if (kAnimFile == null)
            {
                message = animFile + " is not an animation.";
                return;
            }

            var data = kAnimFile.GetData();

            HashedString[] animations = null;

            if (all)
            {
                var list = new List<HashedString>();

                for (var i = 0; i < data.animCount; i++)
                {
                    list.Add(data.GetAnim(i).name);
                }

                animations = list.ToArray();
            }

            else
            {
                animations = animation.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(s => (HashedString)s).ToArray();
            }

            if (animations.Length == 0)
            {
                message = "animations to play should not be empty";
            }

            message += "Anim names available for " + animFile + "\n";
            for (var i = 0; i < data.animCount; i++)
            {
                message += "- " + data.GetAnim(i).name + "\n";
            }

            foreach (MinionIdentity minion in Components.MinionIdentities)
            {
                new EmoteChore(
                    minion.GetComponent<ChoreProvider>(),
                    Db.Get().ChoreTypes.EmoteHighPriority,
                    animName,
                    animations,
                    null);
            }
        }
    }
}
