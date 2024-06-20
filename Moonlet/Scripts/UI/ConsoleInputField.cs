﻿
using System;
using TMPro;
using UnityEngine;

namespace Moonlet.Scripts.UI
{
	public class ConsoleInputField : KScreen, IInputHandler
	{
		[MyCmpReq] public TMP_InputField inputField;

		[SerializeField] public string textPath = "Text";
		[SerializeField] public string placeHolderPath = "Placeholder";

		private bool initialized;

		public Action<string> onSubmit;
		public System.Action onSelectUp;
		public System.Action onSelectDown;
		public System.Action onAutoComplete;

		public bool IsEditing() => isEditing;

		public string Text
		{
			get => inputField.text;
			set
			{
				FUtility.Log.Assert("inputField", inputField);
				FUtility.Log.Assert("textViewport", inputField.textViewport);

				if (!initialized)
				{
					inputField.textComponent = inputField.textViewport.transform.Find(textPath).gameObject.AddOrGet<LocText>();
					inputField.placeholder = inputField.textViewport.transform.Find(placeHolderPath).gameObject.AddOrGet<LocText>();

					initialized = true;
				}

				FUtility.Log.Assert("textcomponent", inputField.textComponent);
				FUtility.Log.Assert("placeholder", inputField.placeholder);

				inputField.text = value;
			}
		}

		public TMP_InputField.OnChangeEvent OnValueChanged => inputField.onValueChanged;

		public override void OnSpawn()
		{
			base.OnSpawn();

			inputField.onFocus += OnEditStart;
			inputField.onEndEdit.AddListener(OnEditEnd);

			inputField.enabled = false;
			inputField.enabled = true;

			Activate();
		}

		public override void OnShow(bool show)
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
			onSubmit?.Invoke(input);
			inputField.text = "";
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

			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				Log.Debug("up keycode");
				onSelectUp?.Invoke();
				e.Consumed = true;
			}

			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				Log.Debug("down keycode");
				onSelectDown?.Invoke();
				e.Consumed = true;
			}

			else if (Input.GetKeyDown(KeyCode.Tab))
			{
				Log.Debug("tab keycode");
				onAutoComplete?.Invoke();
				e.Consumed = true;
			}

			else if (Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				Log.Debug("tab keycode");
				onSubmit?.Invoke(null);
				e.Consumed = true;
			}


			if (e.TryConsume(Action.Escape))
			{
				inputField.DeactivateInputField();
				e.Consumed = true;
				isEditing = false;
			}

			else if (e.TryConsume(Action.DialogSubmit))
			{
				Log.Debug("on submit here");
				onSubmit?.Invoke(inputField.text);
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