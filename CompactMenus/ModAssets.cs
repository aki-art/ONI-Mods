using CompactMenus.Cmps;
using FUtility.FUI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CompactMenus
{
    public class ModAssets
    {
        public static GameObject inputFieldPrefab;
        public static GameObject xButtonPrefab;
        public static GameObject menuIconPrefab;
        public static GameObject buildMenuMenuPrefab;

        public static void LoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("compactmenuassets");

            xButtonPrefab = bundle.LoadAsset<GameObject>("assets/prefabs/xbutton.prefab");
            menuIconPrefab = bundle.LoadAsset<GameObject>("assets/prefabs/menuicon.prefab");
            buildMenuMenuPrefab = bundle.LoadAsset<GameObject>("assets/prefabs/buildmenumenu.prefab");
            buildMenuMenuPrefab.AddComponent<BuildMenuMenu>();
            buildMenuMenuPrefab.AddComponent<LayoutElement>(); //.ignoreLayout = true;

            var converter = new TMPConverter();
            converter.ReplaceAllText(buildMenuMenuPrefab);

            CreatePrefabs();
        }

        public static void CreatePrefabs()
        {
            // slightly dumb way to make a kinputfield, but it works
            inputFieldPrefab = TMP_DefaultControls.CreateInputField(new TMP_DefaultControls.Resources());

            var tmpInputField = inputFieldPrefab.GetComponent<TMP_InputField>();

            var viewPort = tmpInputField.textViewport;
            var text = tmpInputField.textComponent;
            var placeholder = tmpInputField.placeholder;
            var font = tmpInputField.fontAsset;

            Object.DestroyImmediate(tmpInputField);

            var kInputTextField = inputFieldPrefab.AddComponent<KInputTextField>();
            kInputTextField.textViewport = viewPort;
            kInputTextField.textComponent = text;
            kInputTextField.placeholder = placeholder;
            kInputTextField.fontAsset = font;

            var fonts = new List<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>());
            kInputTextField.fontAsset = fonts.FirstOrDefault(f => f.name == "NotoSans-Regular");
            kInputTextField.placeholder.GetComponent<TextMeshProUGUI>().text = global::STRINGS.UI.DIAGNOSTICS_SCREEN.SEARCH;

            inputFieldPrefab.transform.position = Vector3.zero;

            var inputRect = inputFieldPrefab.AddOrGet<RectTransform>();

            // Traverse.Create(kInputField).Field("inputField").SetValue(kInputTextField);

            inputRect.localScale = Vector3.one;
            inputRect.localPosition = Vector3.zero;
            inputRect.anchoredPosition = new Vector2(0f, 0f); // setting position, will be on center
            inputRect.sizeDelta = new Vector2(260f, 30f); // custom size

            inputFieldPrefab.AddComponent<Outline>().effectDistance = Vector2.one;

            var xButton = Object.Instantiate(xButtonPrefab, inputFieldPrefab.transform);
            xButton.SetActive(true);
            var xFButton = xButton.AddComponent<FButton>();

            inputFieldPrefab.AddComponent<BuildMenuSearch>().resetSearchButton = xFButton;
        }
    }
}
