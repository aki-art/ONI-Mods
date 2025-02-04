using LibNoiseDotNet.Graphics.Tools.Noise;

namespace Moonlet.LibNoiseExtension.Primitives
{
	public class Texture2D : PrimitiveModule, IModule3D, IModule
	{
		public UnityEngine.Texture2D texture;

		public Texture2D(UnityEngine.Texture2D texture)
		{
			this.texture = texture;
			this.texture.wrapMode = UnityEngine.TextureWrapMode.Repeat;
		}

		public float GetValue(float x, float y, float z)
		{
			Log.Debug($"x: {x} y: {z}");
			var color = texture.GetPixel((int)(x * 10), (int)(z * 10));

			return color.r;

			//return UnityEngine.Random.value;
		}
	}
}
