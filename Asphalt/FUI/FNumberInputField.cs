using UnityEngine;
using UnityEngine.UI;

namespace Asphalt
{
    public class FNumberInputField : FInputField
    {
        public int maxValue = int.MaxValue;

        public float FloatValue
        {
            get
            {
                float.TryParse(inputField.text, out float val);
                val = Mathf.Clamp(val, 0, maxValue);
                return val;
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            inputField.contentType = InputField.ContentType.DecimalNumber;
        }
        protected override void ProcessInput(string input)
        {
            if (!input.IsNullOrWhiteSpace()) 
                base.ProcessInput(FloatValue.ToString());
        }
    }
}
