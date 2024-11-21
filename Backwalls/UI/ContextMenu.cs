using FUtility.FUI;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backwalls.UI
{
	public class ContextMenu : FScreen
	{
		private Transform panel;
		private FButton optionPrefab;

		private Dictionary<string, GameObject> buttons = [];

		public override void SetObjects()
		{
			panel = transform.Find("Panel");
			optionPrefab = panel.Find("MenuEntryPrefab").gameObject.AddComponent<FButton>();
			optionPrefab.hoverColor = Util.ColorFromHex("626987");
			optionPrefab.normalColor = Util.ColorFromHex("3E4357");
		}

		public void SetOptions(params string[] options)
		{
			Log.Debug("setting options for " + options.Join());
			foreach (var button in buttons)
			{
				Log.Debug($"{button.Key} {options.Contains(button.Key)}");
				button.Value.SetActive(options.Contains(button.Key));
			}
		}

		public void AddOption(string id, string label, System.Action onClick)
		{
			var button = Instantiate(optionPrefab);

			button.transform.SetParent(panel.transform, true);
			var locText = button.transform.GetChild(0).GetComponent<LocText>();
			locText.key = "";
			locText.SetText(label);
			button.OnClick += onClick;

			buttons[id] = button.gameObject;

			button.gameObject.SetActive(true);
		}
	}
}
