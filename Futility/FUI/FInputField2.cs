using TMPro;
using UnityEngine;

namespace FUtility.FUI
{
	public class FInputField2 : KScreen, IInputHandler
	{
		[MyCmpReq]
		public TMP_InputField inputField;

		[SerializeField]
		public string textPath = "Text";

		[SerializeField]
		public string placeHolderPath = "Placeholder";

		private bool initialized;

		public bool IsEditing()
		{
			return isEditing;
		}

		public string Text
		{
			get => inputField.text;
			set
			{
				if (!initialized)
				{
					inputField = GetComponent<TMP_InputField>();

					if (inputField == null)
						Log.Warning("No inputfield on FInputField2");

					inputField.textComponent = inputField.textViewport.transform.Find(textPath).gameObject.AddOrGet<LocText>();
					inputField.placeholder = inputField.textViewport.transform.Find(placeHolderPath).gameObject.AddOrGet<LocText>();

					initialized = true;
				}

				Log.Assert("inputField", inputField);
				Log.Assert("textViewport", inputField.textViewport);
				Log.Assert("textcomponent", inputField.textComponent);
				Log.Assert("placeholder", inputField.placeholder);

				inputField.text = value;
			}
		}

		public TMP_InputField.OnChangeEvent OnValueChanged => inputField.onValueChanged;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			inputField = GetComponent<TMP_InputField>();

		}

		protected override void OnSpawn()
		{
			base.OnSpawn();

			inputField.onFocus += OnEditStart;
			inputField.onEndEdit.AddListener(OnEditEnd);

			inputField.enabled = false;
			inputField.enabled = true;

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