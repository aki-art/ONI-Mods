using System;
using System.Collections.Generic;
using TUNING;

namespace FUtility
{
    public class Buildings
    {
        public static void RegisterBuildings(List<Type> buildings)
        {
            foreach (var building in buildings)
                RegisterSingleBuilding(building);
        }

        public static void RegisterSingleBuilding(Type building)
        {
            if (typeof(IModdedBuilding).IsAssignableFrom(building))
            {
                object obj = Activator.CreateInstance(building);
                Register(obj as IModdedBuilding);
            }
        }

        private static void Register(IModdedBuilding b)
        {
            AddToBuildMenu(b);
            AddToResearch(b.Research, b.GetID);
        }

        private static void AddToBuildMenu(IModdedBuilding b)
        {
            if (b.BuildMenu.IsNullOrWhiteSpace()) 
                return;

            if(!b.Following.IsNullOrWhiteSpace())
            {
                IList<string> category = FindCategory(b);
                int index = category.IndexOf(DoorConfig.ID);
                if (index != -1)
                { 
                    category.Insert(index + 1, b.GetID);
                    return;
                }
            }

            ModUtil.AddBuildingToPlanScreen(b.BuildMenu, b.GetID);
        }

        private static void AddToResearch(string techGroup, string id)
        {
            if (!techGroup.IsNullOrWhiteSpace())
            {
                var techList = new List<string>(Database.Techs.TECH_GROUPING[techGroup]) { id };
                Database.Techs.TECH_GROUPING[techGroup] = techList.ToArray();
            }
        }

        private static IList<string> FindCategory(IModdedBuilding b)
        {
            return BUILDINGS.PLANORDER.Find(x => x.category == b.BuildMenu).data as IList<string>;
        }
    }
}
