using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utils
{
    // TODO:
    // do the thing where you can neighbour items in the build menu

    public class Buildings
    {
        private const string STRING_PATH = "STRINGS.BUILDINGS.PREFABS";
        public static void RegisterAllBuildings(List<BuildingConfig> buildings)
        {
            foreach (BuildingConfig building in buildings)
            {
                string menu = GetBuildMenu(building.type);
                string research = GetResearch(building.type);
                Type type = GetStringsPathAttribute(building.type);

                if (type != null)
                    AddStrings(type, building.id);

                if (!menu.IsNullOrWhiteSpace())
                    ModUtil.AddBuildingToPlanScreen(menu, building.id);

                if (!research.IsNullOrWhiteSpace())
                    AddToResearch(research, building.id);
            }
        }

        private static void AddToResearch(string techGroup, string id)
        {
            var techList = new List<string>(Database.Techs.TECH_GROUPING[techGroup]) { id };
            Database.Techs.TECH_GROUPING[techGroup] = techList.ToArray();
        }

        private static void AddStrings(Type type, string id)
        {
            string ID = id.ToUpper();
            string root = $"{STRING_PATH}.{ID}";

            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(LocString))
                {
                    LocString value = (LocString)field.GetValue(null);
                    if (value != null)
                        Strings.Add($"{root}.{field.Name}", value.text);
                }
            }
        }

        private static string GetBuildMenu(Type type)
        {
            if (type.GetCustomAttributes(typeof(BuildMenuAttribute), true).FirstOrDefault() is BuildMenuAttribute dnAttribute)
                return dnAttribute.Menu;
            return null;
        }
        private static string GetResearch(Type type)
        {
            if (type.GetCustomAttributes(typeof(ResearchTreeAttribute), true).FirstOrDefault() is ResearchTreeAttribute dnAttribute)
                return dnAttribute.Research;
            return null;
        }
        private static Type GetStringsPathAttribute(Type type)
        {
            if (type.GetCustomAttributes(typeof(StringsPathAttribute), true).FirstOrDefault() is StringsPathAttribute dnAttribute)
                return dnAttribute.Path;
            return null;
        }

        public struct BuildingConfig
        {
            public string id;
            public Type type;

            public BuildingConfig(string id, Type type)
            {
                this.id = id;
                this.type = type;
            }
        }
    }


    [AttributeUsage(AttributeTargets.Class)]
    public class BuildMenuAttribute : Attribute
    {
        public BuildMenuAttribute(string menu, bool hidden = false)
        {
            Menu = menu;
            Hidden = hidden;
        }

        public string Menu { get; }
        public bool Hidden { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class StringsPathAttribute : Attribute
    {
        public StringsPathAttribute(Type path)
        {
            Path = path;
        }

        public Type Path { get; }
    }


    [AttributeUsage(AttributeTargets.Class)]
    public class ResearchTreeAttribute : Attribute
    {
        public ResearchTreeAttribute(string research)
        {
            Research = research;
        }

        public string Research { get; }
    }
}
