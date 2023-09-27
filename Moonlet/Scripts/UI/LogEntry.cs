using UnityEngine;

namespace Moonlet.Scripts.UI
{
	public class LogEntry : KMonoBehaviour
	{
		public LocText text;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			text = GetComponentInChildren<LocText>();
		}

		public void SetText(string text) => this.text.SetText(text);
	}
}
