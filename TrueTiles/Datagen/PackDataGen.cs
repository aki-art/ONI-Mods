using System.IO;

namespace TrueTiles.Datagen
{
    // Used to generate my json before release, and when the user manually "resets" mod data
    public class PackDataGen : DataGen
    {
        private const string AUTHOR = "Aki";

        public PackDataGen(string path) : base(path)
        {
            ConfigureMetaData("Default", true, "truetiles_default", "assets/tiles/default_textures/", - 1);
            ConfigureMetaData("Material", false);
            ConfigureMetaData("BeautifulGranite", false);
            ConfigureMetaData("CutesyCarpet", false);
            ConfigureMetaData("AltAirflow", false);
        }

        private void ConfigureMetaData(string id, bool enabled, string assetBundleName = null, string assetFolderRoot = null, int order = 0)
        {
            Write(Path.Combine(path, id), "metadata", new PackData()
            {
                Id = id,
                Name = $"TrueTiles.STRINGS.TEXTUREPACKS.{id.ToUpperInvariant()}.NAME",
                Description = $"TrueTiles.STRINGS.TEXTUREPACKS.{id.ToUpperInvariant()}.DESCRIPTION",
                Author = AUTHOR,
                Order = order,
                Enabled = enabled,
                AssetBundle = assetBundleName,
                AssetBundleRoot = assetFolderRoot
            });
        }
    }
}
