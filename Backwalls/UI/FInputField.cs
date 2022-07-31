using TMPro;
using UnityEngine;

namespace Backwalls.UI
{
    public class FInputField : KScreen, IInputHandler
    {
        [MyCmpReq]
        private TMP_InputField inputField;

        [SerializeField]
        public string textPath = "Text";

        [SerializeField]
        public string placeHolderPath = "Placeholder";

        public bool IsEditing()
        {
            return isEditing;
        }

        public string Text
        {
            get => inputField.text;
            set => inputField.text = value;
        }

        public TMP_InputField.OnChangeEvent OnValueChanged => inputField.onValueChanged;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            // rehook references, these were lost on LocText conversion
            inputField.textComponent = inputField.textViewport.transform.Find(textPath).GetComponent<LocText>();
            inputField.placeholder = inputField.textViewport.transform.Find(placeHolderPath).GetComponent<LocText>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            inputField.onFocus += OnEditStart;
            inputField.onEndEdit.AddListener(OnEditEnd);

            Activate();
        }

        protected override void OnShow(bool show)
        {
            base.OnShow(show);

            if (show)
            {
                Activate();
                inputField.ActivateInputField();
            }
            else
            {
                Deactivate();
            }
        }

        public void Submit()
        {
            inputField.OnSubmit(null);
        }

        private void OnEditEnd(string input)
        {
            isEditing = false;
            inputField.DeactivateInputField();
        }

        private void OnEditStart()
        {
            isEditing = true;
            inputField.Select();
            inputField.ActivateInputField();

            KScreenManager.Instance.RefreshStack();
        }

        public override void OnKeyDown(KButtonEvent e)
        {
            if (!isEditing)
            {
                base.OnKeyDown(e);
                return;
            }

            if (e.TryConsume(Action.Escape))
            {
                inputField.DeactivateInputField();
                e.Consumed = true;
                isEditing = false;
            }

            if (e.TryConsume(Action.DialogSubmit))
            {
                e.Consumed = true;
                inputField.OnSubmit(null);
            }

            if (isEditing)
            {
                e.Consumed = true;
                return;
            }

            if (!e.Consumed)
            {
                base.OnKeyDown(e);
            }
        }
    }
}
