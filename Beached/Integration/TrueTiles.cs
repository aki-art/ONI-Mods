using HarmonyLib;
using System.IO;
using System.Reflection;

namespace Beached.Integration
{
    public class TrueTiles
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                if (AccessTools.Method("TrueTiles.TileAssetLoader, TrueTiles:LoadAssets") is MethodInfo LoadAssetsMethod)
                {
                    var myModsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var pathToMyTiles = Path.Combine(myModsPath, "assets", "truetiles");

                    LoadAssetsMethod.Invoke(null, new object[] { pathToMyTiles });
                }
            }
        }
    }
}
