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
            AddToResearch(b.Info.Research, b.Info.ID);
        }

        private static void AddToBuildMenu(IModdedBuilding b)
        {
            if (b.Info.BuildMenu.IsNullOrWhiteSpace()) 
                return;

            if(!b.Info.Following.IsNullOrWhiteSpace())
            {
                IList<string> category = FindCategory(b);
                int index = category.IndexOf(DoorConfig.ID);
                if (index != -1)
                { 
                    category.Insert(index + 1, b.Info.ID);
                    return;
                }
            }

            ModUtil.AddBuildingToPlanScreen(b.Info.BuildMenu, b.Info.ID);
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
            return BUILDINGS.PLANORDER.Find(x => x.category == b.Info.BuildMenu).data as IList<string>;
        }
    }
}
