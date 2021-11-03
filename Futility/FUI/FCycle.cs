using System.Collections.Generic;
using TMPro;

namespace FUtility.FUI
{
    public class FCycle : KMonoBehaviour
    {
        public event System.Action OnChange;

        FButton leftArrow;
        FButton rightArrow;
        public TextMeshProUGUI label;
        private int currentIndex = 0;
        public List<string> Options;

        public class Option
        {
            public string key;
            public string value;

            public Option(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }

        private bool HasOptions => Options.Count > 0;

        public string Value
        {
            get => Options.Count >= currentIndex ? Options[currentIndex] :  default;

            set
            {
                label.SetText(value);
                currentIndex = Options.IndexOf(value);
            }
        }

        protected override void OnPrefabInit()
        {
            SetObjects();

            leftArrow.OnClick += CycleLeft;
            rightArrow.OnClick += CycleRight;

        }

        private void SetObjects()
        {
            leftArrow = transform.Find("Cycle/PrevBtn").gameObject.AddComponent<FButton>();
            rightArrow = transform.Find("Cycle/NextBtm").gameObject.AddComponent<FButton>();
            label = transform.Find("Cycle").GetComponent<TextMeshProUGUI>();
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
            if (Options.Count >= currentIndex)
                Value = Options[currentIndex];
        }
    }
}
