using System;
using System.Collections.Generic;

namespace FUtility
{
    public class Buildings2
    {
        public static void RegisterBuildings(List<Type> buildings)
        {
            foreach (var building in buildings)
                RegisterSingleBuilding(building);
        }

        public static void RegisterSingleBuilding(Type building)
        {
            if(building is IModdedBuilding)
            { 
                object obj = Activator.CreateInstance(building);
                Register(obj as IModdedBuilding);
            }
        }

        private static void Register(IModdedBuilding b)
        {
            AddToBuildMenu(b.BuildMenu, b.ID);
            AddToResearch(b.Research, b.ID);
        }

        private static void AddToBuildMenu(string menu, string id)
        {
            if (!menu.IsNullOrWhiteSpace())
                ModUtil.AddBuildingToPlanScreen(menu, id);
        }

        private static void AddToResearch(string techGroup, string id)
        {
            if (!techGroup.IsNullOrWhiteSpace())
            {
                var techList = new List<string>(Database.Techs.TECH_GROUPING[techGroup]) { id };
                Database.Techs.TECH_GROUPING[techGroup] = techList.ToArray();
            }
        }
    }
}
