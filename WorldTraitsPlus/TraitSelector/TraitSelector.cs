using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WorldTraitsPlus.TraitSelector
{
    class TraitSelector : KScreen
    {
        GameObject traitPrefab;
        Transform container;
        List<TraitEntry> traits;

        protected override void OnPrefabInit()
        {
            traits = new List<TraitEntry>();
            container = transform.Find("Scroll View/Viewport/Content/");
            traitPrefab = container.Find("TraitEntry").gameObject;
            traitPrefab.gameObject.SetActive(false);
        }

        internal void Refresh(List<AsteroidDescriptor> asteroidDescriptors)
        {
            foreach (TraitEntry child in traits)
                DestroyImmediate(child.gameObject);
            traits.Clear();

            Debug.Log("traits: " + traits.Count);
            foreach (var descriptor in asteroidDescriptors)
            {
                Debug.Log("adding new entry " + descriptor.text);
                var newItem = Instantiate(traitPrefab, container);
                var entry = newItem.gameObject.AddComponent<TraitEntry>();
                entry.color = Color.black;
                entry.content = descriptor.text;

                entry.gameObject.SetActive(true);
                entry.Refresh();

                traits.Add(entry);
            }
            Debug.Log("traits: " + traits.Count);
        }
    }
}
