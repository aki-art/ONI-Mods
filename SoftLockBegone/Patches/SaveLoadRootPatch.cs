using FUtility;
using HarmonyLib;
using SoftLockBegone.Content;
using SoftLockBegone.Content.Scripts;
using UnityEngine;

namespace SoftLockBegone.Patches
{
	public class SaveLoadRootPatch
	{
		//[HarmonyPatch(typeof(SaveLoadRoot), "Load", typeof(GameObject), typeof(IReader))]
		public class SaveLoadRoot_Load_Patch
		{
			public static void Postfix(ref GameObject prefab, IReader reader)
			{
				if (prefab.IsPrefabID(SLBEntityConfig.ID))
				{
					Log.Debuglog("is slb");
					if (prefab.TryGetComponent(out SLB_EntityComponent component))
					{
						component.TryTransferring();
/*						Log.Debuglog(component.originalPrefabTag);
						var prefabId = component.originalPrefabTag;
						var originalPrefab = Assets.TryGetPrefab(prefabId);

						if (originalPrefab != null)
						{
							Log.Debuglog("prefab exists");
							prefab = originalPrefab;
						}*/
					}
				}
			}
		}
	}
}
