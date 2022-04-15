using System.IO;

namespace TrueTiles.Datagen
{
    // Used to generate my json before release, and when the user manually "resets" mod data
    public class PackDataGen : DataGen
    {
        private const string AUTHOR = "Aki";

        public PackDataGen(string path) : base(path)
        {
            ConfigureMetaData("Material", false);
            ConfigureMetaData("BeautifulGranite", false);
            ConfigureMetaData("CutesyCarpet", false);
            ConfigureMetaData("Default", true, -1);
        }

        private void ConfigureMetaData(string id, bool enabled, int order = 0)
        {
            Write(Path.Combine(path, id), "metadata", new PackData()
            {
                Id = id,
                Name = $"TrueTiles.STRINGS.TEXTUREPACKS.{id.ToUpperInvariant()}.NAME",
                Description = $"TrueTiles.STRINGS.TEXTUREPACKS.{id.ToUpperInvariant()}.DESCRIPTION",
                Author = AUTHOR,
                Order = order,
                Enabled = enabled
            });
        }
    }
}
