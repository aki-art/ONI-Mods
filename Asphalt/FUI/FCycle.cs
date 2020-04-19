using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asphalt
{
    public class FCycle : KMonoBehaviour
    {
        public event System.Action OnChange;

        private FButton leftArrow;
        private FButton rightArrow;

        private Text label;
        private Text description;

        private int currentIndex = 0;

        private bool HasOptions
        {
            get
            {
                return Options.Count > 0;
            }
        }

        public List<KeyValuePair<string, string>> Options;

        protected override void OnPrefabInit()
        {
            #region setting object references
            leftArrow = transform.Find("CycleSelector/LightShapeLeftButton").gameObject.AddComponent<FButton>();
            rightArrow = transform.Find("CycleSelector/LightShapeRightButton").gameObject.AddComponent<FButton>();
            label = transform.Find("CycleSelector").GetComponent<Text>();
            description = transform.Find("Note").GetComponent<Text>();
            #endregion

            leftArrow.OnClick += CycleLeft;
            rightArrow.OnClick += CycleRight;

        }
        public void CycleLeft()
        {
            if (HasOptions)
            {
                currentIndex = (currentIndex + Options.Count - 1) % Options.Count;
                UpdateLabel();
                OnChange?.Invoke();
            }
        }
        public void CycleRight()
        {
            if (HasOptions)
            {
                currentIndex = (currentIndex + 1) % Options.Count;
                UpdateLabel();
                OnChange?.Invoke();
            }
        }

        private void UpdateLabel()
        {
            if (HasOptions)
            {
                label.text = Options[currentIndex].Key;
                description.text = Options[currentIndex].Value;
            }
        }

        public string GetValue()
        {
            return HasOptions ? Options[currentIndex].Key : null;
        }

    }
}
