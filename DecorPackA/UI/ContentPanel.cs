using FUtility.FUI;
using TMPro;

namespace DecorPackA.UI
{
	public class ContentPanel : KMonoBehaviour
	{
		private InputFieldPanel inputPrefab;
		private UnitInputFieldPanel unitInputPrefab;
		private RangedPairInputFieldPanel doubleInputPrefab;
		private TitlePanel titlePrefab;
		private TogglePanel togglePrefab;

		private bool initialized;

		private void Initialize()
		{
			if (initialized)
				return;

			inputPrefab = transform.Find("InputPanel").gameObject.AddComponent<InputFieldPanel>();
			unitInputPrefab = transform.Find("QuantityInputPanel").gameObject.AddComponent<UnitInputFieldPanel>();
			titlePrefab = transform.Find("TitlePanel").gameObject.AddComponent<TitlePanel>();
			togglePrefab = transform.Find("TogglePanel").gameObject.AddComponent<TogglePanel>();
			doubleInputPrefab = transform.Find("DoubleInputPanel").gameObject.AddComponent<RangedPairInputFieldPanel>();

			initialized = true;
		}

		public LocText AddTitle(string label, string tooltip = null)
		{
			Initialize();

			var result = Instantiate(titlePrefab, transform);
			result.SetText(label);
			result.gameObject.SetActive(true);

			if (tooltip != null)
				Helper.AddSimpleToolTip(result.gameObject, tooltip);

			return result.label;
		}

		public void AddDoubleInputField(string label, object defaultValue, object defaultRange, out FInputField2 valueField, out FInputField2 rangeField)
		{
			Initialize();

			var result = Instantiate(doubleInputPrefab, transform);

			result.transform.SetParent(transform, true);
			result.Initialize();

			result.inputFieldValue.Text = defaultValue?.ToString();
			result.inputFieldRange.Text = defaultRange?.ToString();

			result.labelValue.SetText(label);
			result.labelRange.SetText(STRINGS.UI.DECORPACKA_SETTINGS.RANGE);

			result.gameObject.SetActive(true);

			valueField = result.inputFieldValue;
			rangeField = result.inputFieldRange;
		}

		public void AddUnitInputField(string label, string unit, object defaultValue, out FInputField2 inputField, string tooltip = null)
		{
			Initialize();

			var result = Instantiate(unitInputPrefab, transform);

			result.transform.SetParent(transform, true);
			result.Initialize();
			result.inputField.Text = defaultValue?.ToString();
			result.label.key = "";
			result.label.SetText(label);
			result.label.text = label;
			result.gameObject.SetActive(true);
			result.SetUnit(unit);

			if (tooltip != null)
				Helper.AddSimpleToolTip(result.label.gameObject, tooltip);

			inputField = result.inputField;
		}

		public void AddInputField(string label, object defaultValue, out FInputField2 inputField, string tooltip = null)
		{
			Initialize();

			var result = Instantiate(inputPrefab, transform);

			result.transform.SetParent(transform, true);
			result.Initialize();
			result.inputField.Text = defaultValue?.ToString();
			result.label.key = "";
			result.label.SetText(label);
			result.label.text = label;
			result.gameObject.SetActive(true);

			if (tooltip != null)
				Helper.AddSimpleToolTip(result.label.gameObject, tooltip);

			inputField = result.inputField;
		}

		public void AddToggle(string label, bool value, out FToggle2 toggle, string tooltip = null)
		{
			Initialize();

			var result = Instantiate(togglePrefab, transform);

			result.transform.SetParent(transform, true);
			result.Initialize(value, label);
			result.gameObject.SetActive(true);

			if (tooltip != null)
				Helper.AddSimpleToolTip(result.label.gameObject, tooltip);

			toggle = result.toggle;
		}

		public class TogglePanel : KMonoBehaviour
		{
			public FToggle2 toggle;
			public LocText label;

			public void Initialize(bool defaultValue, string labelText)
			{
				toggle = gameObject.AddComponent<FToggle2>();
				toggle.SetCheckmark("Background/Checkmark");
				toggle.On = defaultValue;

				label = transform.Find("Label").GetComponent<LocText>();
				label.key = "";
				label.SetText(labelText);
			}
		}

		public class TitlePanel : KMonoBehaviour
		{
			public LocText label;

			public void SetText(string text)
			{
				if (label == null)
				{
					label = transform.Find("Label").GetComponent<LocText>();
					label.alignment = TextAlignmentOptions.TopLeft;
				}

				label.key = "";
				label.SetText(text);
			}
		}

		public class RangedPairInputFieldPanel : KMonoBehaviour
		{
			public FInputField2 inputFieldValue;
			public FInputField2 inputFieldRange;
			public LocText labelValue;
			public LocText labelRange;

			public void Initialize()
			{
				inputFieldValue = InitializeInputField("Input1");
				inputFieldRange = InitializeInputField("Input2");

				labelValue = transform.Find("Label1").GetComponent<LocText>();
				labelValue.key = "";

				labelRange = transform.Find("Label2").GetComponent<LocText>();
				labelRange.key = "";
			}

			private FInputField2 InitializeInputField(string name)
			{
				var field = transform.Find(name).gameObject.AddComponent<FInputField2>();
				field.inputField = field.GetComponent<TMP_InputField>();

				field.inputField.textComponent = field.inputField.textViewport.transform.Find("Text").GetComponent<LocText>();
				field.inputField.placeholder = field.inputField.textViewport.transform.Find("Placeholder").GetComponent<LocText>();

				field.inputField.contentType = TMP_InputField.ContentType.IntegerNumber;

				return field;
			}
		}

		public class InputFieldPanel : KMonoBehaviour
		{
			public FInputField2 inputField;
			public LocText label;

			public virtual void Initialize()
			{
				inputField = transform.Find("Input").gameObject.AddComponent<FInputField2>();
				inputField.inputField = inputField.GetComponent<TMP_InputField>();

				inputField.inputField.textComponent = inputField.inputField.textViewport.transform.Find("Text").GetComponent<LocText>();
				inputField.inputField.placeholder = inputField.inputField.textViewport.transform.Find("Placeholder").GetComponent<LocText>();

				inputField.inputField.contentType = TMP_InputField.ContentType.IntegerNumber;

				label = transform.Find("Label").GetComponent<LocText>();
				label.key = "";
			}
		}

		public class UnitInputFieldPanel : InputFieldPanel
		{
			public LocText unit;

			public override void Initialize()
			{
				base.Initialize();
				unit = transform.Find("Input/UnitLabel").GetComponent<LocText>();
			}

			public void SetUnit(string unitLabel)
			{
				unit.key = "";
				unit.SetText(unitLabel);
			}
		}
	}
}
