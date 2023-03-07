using DecorPackA.Buildings.StainedGlassTile;
using UnityEngine;

namespace DecorPackA
{
    public class ModAPI
    {
        // methods that are promised to not change signature or disappear, so they are safe to reflect for by other mods

        /// <summary>
        /// Add a tile configuration
        /// </summary>
        /// <param name="elementID">String id of the element (SimHashes.ToString())</param>
        /// <param name="specularColor">Color.white for default</param>
        /// <param name="isSolid">true for solids, false for gases, liquids or specials.</param>
        /// <param name="dlcIds">leave null for all, <see cref="DlcManager.AVAILABLE_EXPANSION1_ONLY"/></param>
        /// <param name="main">Texture of the body of the tiles</param>
        /// <param name="top">top texture</param>
        /// <param name="place">white outline for placing. only the body, the top uses the default glass.</param>
        /// <param name="specular">specular texture. leave null if none.</param>
        public static void AddTile(
            string elementID,
            Color specularColor,
            bool isSolid,
            string[] dlcIds,
            Texture2D main,
            Texture2D top,
            Texture2D place,
            Texture2D specular)
        {
            var info = new StainedGlassTiles.TileInfo(elementID);
            if(specular != null)
            {
                info.SpecColor(specularColor);
            }

            if(!isSolid)
            {
                info.NotSolid();
            }

            if(dlcIds != null)
            {
                info.DLC(dlcIds);
            }

            StainedGlassTiles.tileInfos.Add(info);

            var id = elementID.ToLowerInvariant();
            if (main != null) TextureLoader.textureRegistry.Add($"{id}_glass_tiles", main);
            if (top != null) TextureLoader.textureRegistry.Add($"{id}_glass_tiles_tops", top);
            if (place != null) TextureLoader.textureRegistry.Add($"{id}_glass_tiles_place", place);
            if (specular != null) TextureLoader.textureRegistry.Add($"{id}_glass_tiles_spec", specular);
        }
    }
}
