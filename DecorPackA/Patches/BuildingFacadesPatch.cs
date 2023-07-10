using Database;
using HarmonyLib;
using System.Collections.Generic;

namespace DecorPackA.Patches
{
	public class BuildingFacadesPatch
	{
		// manually patching, because referencing BuildingFacades class will load strings too early
		public static void Patch(Harmony harmony)
		{
			var targetType = AccessTools.TypeByName("Database.BuildingFacades");
			var target = AccessTools.Constructor(targetType, new[] { typeof(ResourceSet) });
			var postfix = AccessTools.Method(typeof(BuildingFacades_Ctor_Patch), "Postfix");

			harmony.Patch(target, null, new HarmonyMethod(postfix));
		}

		public class BuildingFacades_Ctor_Patch
		{
			public static void Postfix(object __instance)
			{
				var resource = (ResourceSet<BuildingFacadeResource>)__instance;

				AddPotFacade(
					resource,
					"DecorPackA_FlowerVaseHangingFancy_Colorful",
					STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_COLORFUL.NAME,
					STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_COLORFUL.DESC,
					"decorpacka_hangingvase_colorful_kanim");

				AddPotFacade(
					resource,
					"DecorPackA_FlowerVaseHangingFancy_BlueYellow",
					STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_BLUEYELLOW.NAME,
					STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_BLUEYELLOW.DESC,
					"decorpacka_hangingvase_blueyellow_kanim");

				AddPotFacade(
					resource,
					"DecorPackA_FlowerVaseHangingFancy_ShoveVole",
					STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_SHOVEVOLE.NAME,
					STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_SHOVEVOLE.DESC,
					"decorpacka_hangingvase_shovevoleb_kanim");

				AddPotFacade(
					resource,
					"DecorPackA_FlowerVaseHangingFancy_Honey",
					STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_HONEY.NAME,
					STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_HONEY.DESC,
					"decorpacka_hangingvase_honey_kanim");

				AddPotFacade(
					resource,
					"DecorPackA_FlowerVaseHangingFancy_Uranium",
					STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_URANIUM.NAME,
					STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_URANIUM.DESC,
					"decorpacka_hangingvase_uranium_kanim");
			}

			private static void AddPotFacade(ResourceSet<BuildingFacadeResource> resource, string id, string name, string desc, string anim)
			{
				Add(
					resource,
					id,
					name,
					desc,
					PermitRarity.Universal,
					FlowerVaseHangingFancyConfig.ID,
					anim);

				ModDb.myFacades.Add(id);
			}

			public static void Add(
				ResourceSet<BuildingFacadeResource> set,
				string id,
				LocString name,
				LocString description,
				PermitRarity rarity,
				string prefabId,
				string animFile,
				Dictionary<string, string> workables = null)
			{
				set.resources.Add(new BuildingFacadeResource(id, name, description, rarity, prefabId, animFile, workables));
			}
		}
	}
}
