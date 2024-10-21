using HarmonyLib;
using Moonlet.Loaders;
using UnityEngine;

namespace Moonlet.Patches
{
	public class PasteBaseTemplateScreenPatch
	{
		[HarmonyPatch(typeof(PasteBaseTemplateScreen), "RefreshStampButtons")]
		public class PasteBaseTemplateScreen_RefreshStampButtons_Patch
		{
			public static void Postfix(PasteBaseTemplateScreen __instance)
			{
				if (Mod.templatesLoader.templateAddingMods.Count > 0)
				{
					if (__instance.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
					{
						AddDirectoryButton(__instance, MTemplatesLoader.MOONLET_TEMPLATES, "Moonlet");
						return;
					}

					if (__instance.m_CurrentDirectory == MTemplatesLoader.MOONLET_TEMPLATES)
					{
						foreach (var mod in Mod.templatesLoader.templateAddingMods)
						{
							var modName = MoonletMods.Instance.GetModData(mod).title;
							AddDirectoryButton(__instance, mod, modName);
						}

						return;
					}

					else if (__instance.m_CurrentDirectory.StartsWith(MTemplatesLoader.MOONLET_TEMPLATES))
					{
						var holder = Mod.templatesLoader.GetPathHolder(__instance.m_CurrentDirectory);
						if (holder != null)
						{
							foreach (var children in holder.children)
							{
								if (children.children.Count > 0)
									AddDirectoryButton(__instance, children.id, children.id);
								else
									AddStampButton(__instance, children.id);
							}
						}
					}
				}
			}

			private static GameObject AddDirectoryButton(PasteBaseTemplateScreen instance, string path, string text)
			{
				var gameObject = Util.KInstantiateUI(instance.prefab_directory_button, instance.button_list_container, true);
				gameObject.GetComponent<KButton>().onClick += (() => instance.UpdateDirectory(path));
				gameObject.GetComponentInChildren<LocText>().text = text;
				instance.m_template_buttons.Add(gameObject);

				return gameObject;
			}

			private static GameObject AddStampButton(PasteBaseTemplateScreen instance, string path)
			{
				var gameObject = Util.KInstantiateUI(instance.prefab_paste_button, instance.button_list_container, true);
				gameObject.GetComponent<KButton>().onClick += (() => instance.OnClickPasteButton(path));
				gameObject.GetComponentInChildren<LocText>().text = path;
				instance.m_template_buttons.Add(gameObject);

				return gameObject;
			}
		}
	}
}
