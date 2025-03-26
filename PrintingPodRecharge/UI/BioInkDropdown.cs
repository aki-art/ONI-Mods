using FUtility;
using PrintingPodRecharge.Content.Cmps;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace PrintingPodRecharge.UI
{
	public class BioInkDropdown : KMonoBehaviour, ISim1000ms
	{
		public TMP_Dropdown dropdown;
		private List<Option> options = [];
		public Action<int> onValueChanged;
		private HashSet<Tag> allBioInks = [];
		private HashSet<Tag> discoveredInks = [];

		public Tag SelectedTag
		{
			get => options[dropdown.value].prefabID;
			set
			{
				var index = GetOptionIndex(value);
				index = Math.Max(index, 0);

				dropdown.value = index;
				dropdown.RefreshShownValue();
			}
		}

		public Option Selected
		{
			get
			{
				if (dropdown.value > options.Count)
					return options[0];

				return options[dropdown.value];
			}
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			dropdown = GetComponent<TMP_Dropdown>();

			var item = dropdown.transform.Find("Template/Viewport/Content/Item");

			dropdown.itemText = item.Find("Item Label").GetComponent<LocText>();
			dropdown.captionText = dropdown.transform.Find("Label").GetComponent<LocText>();

			dropdown.onValueChanged.AddListener(OnValueChanged);

			allBioInks = Assets
				.GetPrefabsWithTag(ModAssets.Tags.bioInk)
				.Select(ink => ink.PrefabID())
				.ToHashSet();

			discoveredInks = allBioInks
				.Where(DiscoveredResources.Instance.IsDiscovered)
				.ToHashSet();

			DiscoveredResources.Instance.OnDiscover += OnTagDiscovered;
		}

		private void OnTagDiscovered(Tag categoryTag, Tag tag)
		{
			if (allBioInks.Contains(tag))
			{
				discoveredInks = allBioInks
					.Where(DiscoveredResources.Instance.IsDiscovered)
					.ToHashSet();

				RefreshOptions();
			}
		}

		public void RefreshOptions()
		{
			options.Clear();

			foreach (var tag in discoveredInks)
			{
				Log.Debug($"adding tag: {tag}");
				if (ImmigrationModifier.Instance.IsBundleAvailable(tag))
				{
					var ink = Assets.GetPrefab(tag);
					options.Add(new Option(ink));
				}
			}

			dropdown.options.Clear();

			var newOptions = new List<TMP_Dropdown.OptionData>();

			foreach (var option in options)
			{
				var item = new TMP_Dropdown.OptionData(option.name, option.sprite);
				newOptions.Add(item);
				option.data = item;
			}

			dropdown.AddOptions(newOptions);
			dropdown.RefreshShownValue();
		}

		private void OnValueChanged(int index)
		{
			onValueChanged?.Invoke(index);
		}

		public void ToggleOption(Tag tag, bool enabled)
		{

		}

		private int GetOptionIndex(Tag tag)
		{
			return options.FindIndex(o => o.prefabID == tag);
		}

		public void Sim1000ms(float dt)
		{
			//ClusterManager.Instance.activeWorld.worldInventory.GetAmount
		}

		public class Option
		{
			public string name;
			public string greyedOutname;
			public string description;
			public Sprite sprite;
			public Tag prefabID;
			public bool available;
			public TMP_Dropdown.OptionData data;

			public Option(GameObject go)
			{
				prefabID = go.PrefabID();
				name = go.GetProperName();
				greyedOutname = $"<color=#777777>{name}</color>";
				if (go.TryGetComponent(out InfoDescription infoDescription))
				{
					description = infoDescription.description;
				}
				sprite = Def.GetUISprite(go).first;
			}
		}

	}
}
