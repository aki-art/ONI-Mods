using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularStorage
{
    public class Tuning
    {
        public static string ModularStorageCategory = "ModularStorage";
        public class Tags
        {
            public static Tag StorageModule = TagManager.Create("StorageModule", "Storage Module");
            public static Tag StorageController = TagManager.Create("StorageController", "Storage Controller");
            public static Tag LiquidStorageModule = TagManager.Create("LiquidStorageModule", "Liquid Storage Module");
            public static Tag GasStorageModule = TagManager.Create("GasStorageModule", "Gas Storage Module");
            public static Tag ItemStorageModule = TagManager.Create("ItemStorageModule", "Item Storage Module");
        }
    }
}
