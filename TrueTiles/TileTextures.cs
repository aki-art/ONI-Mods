using UnityEngine;

namespace TrueTiles
{
    public class TileTextures
    {
        public readonly Texture2D main;
        public readonly Texture2D spec;
        public readonly Texture2D top;
        public readonly Texture2D topSpec;

        public TileTextures(Texture2D main, Texture2D spec, Texture2D top, Texture2D topSpec)
        {
            this.main = main;
            this.spec = spec;
            this.top = top;
            this.topSpec = topSpec;
        }
    }
}
