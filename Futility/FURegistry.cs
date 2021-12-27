using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FUtility
{
    // Credit to PRegistry for the general idea
    public class FURegistry : KMonoBehaviour, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> registry = new Dictionary<string, object>();

		public static IDictionary<string, object> Initialize()
		{
			Log.Debuglog($"Initializing registry");

			if(Global.Instance.gameObject is null)
			{
				Log.Warning($"Global.Instance.gameObject doesn't exist.");
				return null;
			}

            Component registryComponent = Global.Instance.gameObject.GetComponent(nameof(FURegistry));

			if (registryComponent is null)
			{
				registryComponent = Global.Instance.gameObject.AddComponent<FURegistry>();
			}

            return registryComponent as IDictionary<string, object>;
		}

		#region dictionary stuff

		public object this[string key] { 
            get => registry[key]; 
            set => registry[key] = value; 
        }

        public ICollection<string> Keys => registry.Keys;

        public ICollection<object> Values => registry.Values;

        public int Count => registry.Count;

        public bool IsReadOnly => false;

        public void Add(string key, object value) => registry.Add(key, value);

        public void Add(KeyValuePair<string, object> item) => registry.Add(item.Key, item.Value);

        public void Clear() => registry.Clear();

        public bool Contains(KeyValuePair<string, object> item) => registry.Contains(item);

        public bool ContainsKey(string key) => registry.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, object>>)registry).CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => registry.GetEnumerator();

        public bool Remove(string key) => registry.Remove(key);

        public bool Remove(KeyValuePair<string, object> item) => ((ICollection<KeyValuePair<string, object>>)registry).Remove(item);

        public bool TryGetValue(string key, out object value) => registry.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => registry.GetEnumerator();
        #endregion
    }
}
