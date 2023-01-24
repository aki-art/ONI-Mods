using System.Collections.Generic;
using System.Linq;
using static SandboxToolParameterMenu.SelectorValue;

namespace FUtility
{
    public class SandboxUtil
    {
        // Creates a filter based on an entity icon, and a tag check
        // SandboxToolParameterMenu.instance does not exist when this is most sensible to be called, so don't use that here
        public static SearchFilter CreateSimpleTagFilter(SandboxToolParameterMenu menu, string name, Tag tag, string prefabIDToUseForIcon, string parentFilterID = null)
        {
            var sprite = Def.GetUISprite(global::Assets.GetPrefab(prefabIDToUseForIcon));
            var parent = FindParent(menu, parentFilterID);

            return new SearchFilter(name, obj => obj is KPrefabID id && id.HasTag(tag), parent, sprite);
        }

        public static void AddFilters(SandboxToolParameterMenu menu, params SearchFilter[] newFilters)
        {
            var filters = menu.entitySelector.filters;

            if(filters == null)
            {
                Log.Warning("Filters are null");
                return;
            }

            Log.Debuglog("Adding filters here");

            var f = new List<SearchFilter>(filters);
            f.AddRange(newFilters);
            menu.entitySelector.filters = f.ToArray();

            UpdateOptions(menu);
        }

        public static void UpdateOptions(SandboxToolParameterMenu menu)
        {
            var filters = menu.entitySelector.filters;

            if (filters == null)
            {
                return;
            }

            var options = ListPool<object, SandboxToolParameterMenu>.Allocate();

            foreach (var prefab in global::Assets.Prefabs)
            {
                foreach (var filter in filters)
                {
                    if (filter.condition(prefab))
                    {
                        options.Add(prefab);
                        break;
                    }
                }
            }

            menu.entitySelector.options = options.ToArray();
            options.Recycle();
        }

        private static SearchFilter FindParent(SandboxToolParameterMenu menu, string parentFilterID)
        {
            return parentFilterID != null ? menu.entitySelector.filters.First(x => x.Name == parentFilterID) : null;
        }
    }
}
