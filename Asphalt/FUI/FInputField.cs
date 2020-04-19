using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Asphalt
{
    public class FInputField : KScreen
    {
		public InputField inputField;

        public event System.Action OnStartEdit;
        public event System.Action OnEndEdit;
		public event System.Action<string> OnValueChanged;

		private bool isEditing;

		public string Value
		{
			get
			{
				return inputField.text;
			}
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();

			inputField = gameObject.GetComponent<InputField>();

			inputField.onEndEdit.AddListener(OnEditEnd);
			inputField.onValueChanged.AddListener(OnChangeValue);
		}
		private void OnChangeValue(string input)
		{
			if(!isEditing)
			{
				isEditing = true;
				KScreenManager.Instance.RefreshStack();

				OnStartEdit?.Invoke();
			}
			else
				ProcessInput(input);

			OnValueChanged?.DynamicInvoke(input);
		}
		private void OnEditEnd(string input)
		{
				StartCoroutine(DelayedEndEdit());
		}

		private IEnumerator DelayedEndEdit()
		{
			if (isEditing)
			{
				yield return new WaitForEndOfFrame();
				StopEditing();
			}
			yield break;
		}
		public override void OnKeyDown(KButtonEvent e)
		{
			if (isEditing)
				e.Consumed = true;
		}
		private void StopEditing()
		{
			isEditing = false;
			inputField.DeactivateInputField();
			OnEndEdit?.Invoke();
		}
		protected virtual void ProcessInput(string input)
		{
			if (!input.IsNullOrWhiteSpace())
			{ 
				SetDisplayValue(input);
			}
		}

		public void SetDisplayValue(string input)
		{
			if(inputField != null)
				inputField.text = input;
		}
	}
}
