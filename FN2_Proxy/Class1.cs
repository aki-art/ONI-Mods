using System;
using System.Runtime.InteropServices;

namespace FN2_Proxy
{
	public class Class1
	{
		private const string NATIVE_LIB = "FastNoise";

		[DllImport(NATIVE_LIB)]
		private static extern IntPtr fnNewFromEncodedNodeTree([MarshalAs(UnmanagedType.LPStr)] string encodedNodeTree, uint simdLevel = 0);


		[DllImport(NATIVE_LIB)]
		private static extern int fnGetMetadataID(IntPtr nodeHandle);
	}
}
