/*using System;

namespace Moonlet
{
	public class FastNoise
	{
		private IntPtr mNodeHandle = IntPtr.Zero;
		private int mMetadataId = -1;

		public static FastNoise FromEncodedNodeTree(string encodedNodeTree)
		{
			IntPtr nodeHandle = fnNewFromEncodedNodeTree(encodedNodeTree);

			if (nodeHandle == IntPtr.Zero)
			{
				return null;
			}

			return new FastNoise(nodeHandle);
		}



		private FastNoise(IntPtr nodeHandle)
		{
			mNodeHandle = nodeHandle;
			mMetadataId = fnGetMetadataID(nodeHandle);
		}
	}
}
*/