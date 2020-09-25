using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;

namespace WorldTraitsPlus.TraitSelector
{
    class TraitEntry : KMonoBehaviour
    {
        public TMP_Dropdown dropdown;
        public TextMeshProUGUI current;
        public List<string> options = new List<string>();

        public string content;
        public Color color;

        protected override void OnPrefabInit()
        {
            dropdown = transform.Find("Select/Dropdown").GetComponent<TMP_Dropdown>();
            current = transform.Find("Select/Dropdown/Label").GetComponent<TextMeshProUGUI>();
            dropdown.itemText = transform.Find("Select/Dropdown/Template/Viewport/Content/Item/Item Label").GetComponent<TextMeshProUGUI>();
            dropdown.captionText = transform.Find("Select/Dropdown/Label").GetComponent<TextMeshProUGUI>();
        }

        internal void Refresh()
        {
            options = new List<string>() { content };
            dropdown.options.Clear();
            foreach (string option in options)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(option));
            }
            current.color = color;
            current.text = content;
            dropdown.RefreshShownValue();
            dropdown.template.hasChanged = true;
        }
    }
}
