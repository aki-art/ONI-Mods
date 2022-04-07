using FUtility;
using FUtility.FUI;
using UnityEngine;

namespace TrueTiles.Settings
{
    public class SettingsScreen : FScreen
    {
        private GameObject entryPrefab;

        public override void SetObjects()
        {
            entryPrefab = transform.Find("ScrollView/Viewport/Content/Entry").gameObject;
            entryPrefab.AddComponent<PackEntry>();

            entryPrefab.gameObject.SetActive(false);
        }

        public override void ShowDialog()
        {
            base.ShowDialog();

            for (int i = 0; i < 10; i++)
            {
                var test = Instantiate(entryPrefab, entryPrefab.transform.parent);
                test.SetActive(true);
                Log.Debuglog("TEST" + i);
            }
        }

        public override void OnClickApply()
        {
            Deactivate();
        }
    }
}
