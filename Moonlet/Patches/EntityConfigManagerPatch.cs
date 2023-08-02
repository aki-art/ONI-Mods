using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlet.Patches
{
	internal class EntityConfigManagerPatch
	{

        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                foreach (var mod in Mod.modLoaders)
				{
					mod.entitiesLoader.LoadPois();
					mod.entitiesLoader.LoadItems();
				}
			}
        }
	}
}
