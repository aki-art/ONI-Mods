using FUtility;
using FUtility.FUI;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace TrueTiles.Settings
{
	public class PackEntry : KScreen
	{
		public string Id;
		private LocText title;
		private LocText description;
		private Image icon;
		private FToggle2 enabledToggle;
		private FButton openFolderButton;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			Helper.ListChildren(transform);

			title = transform.Find("Title/Text").GetComponent<LocText>();
			icon = transform.Find("Icon/Image").GetComponent<Image>();
			description = transform.Find("Info").GetComponent<LocText>();
			enabledToggle = transform.Find("Enabled").FindOrAddComponent<FToggle2>();
			enabledToggle.mark = enabledToggle.transform.Find("Background/Checkmark").GetComponent<Image>();
			openFolderButton = transform.Find("Buttons/Open").FindOrAddComponent<FButton>();
		}

		public void SetFolder(string path)
		{
			if (!Directory.Exists(path))
			{
				Log.Warning($"Invalid path given to texture pack: {path}");
				openFolderButton.SetInteractable(false);
				return;
			}

			openFolderButton.OnClick += () => Application.OpenURL("file://" + path);
		}

		public void SetEnabled(bool enabled)
		{
			if (enabledToggle == null)
			{
				Log.Warning("enabledtoggle is null");
				return;
			}

			enabledToggle.On = enabled;
		}

		public void SetTooltip(string text)
		{
			if (Strings.TryGet(text, out var entry))
			{
				Helper.AddSimpleToolTip(title.gameObject, entry.String);
			}
			else
			{
				Helper.AddSimpleToolTip(title.gameObject, text);
			}
		}

		public void SetTitle(string text)
		{
			if (Strings.TryGet(text, out var entry))
			{
				title.SetText(entry);
			}
			else
			{
				title.SetText(text);
			}
		}

		public bool IsEnabled()
		{
			return enabledToggle.On;
		}

		public void SetIcon(Texture2D texture)
		{
			if (icon == null)
			{
				Log.Warning("null icon");
				return;
			}

			icon.sprite = Sprite.Create(texture, new Rect(0, 0, 48, 48), new Vector2(0.5f, 0.5f));
		}

		public void SetDescription(string author, int textureCount)
		{
			description.SetText(string.Format(STRINGS.TEXTUREPACKS.INFO, author, textureCount));
		}
	}
}
