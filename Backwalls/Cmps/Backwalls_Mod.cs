using KSerialization;
using rendering;
using System;
using UnityEngine;

namespace Backwalls.Cmps
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Backwalls_Mod : KMonoBehaviour
	{
		public static Backwalls_Mod Instance;

		[Serialize][SerializeField] public bool CopyColor;
		[Serialize][SerializeField] public bool CopyPattern;
		[Serialize][SerializeField] public bool ShowHSV;
		[Serialize][SerializeField] public bool ShowSwatches;

		public Backwall.BackwallSettings copySettings;
		public bool hasCopyOverride;

		public void OnCopyBuilding()
		{
			if (SelectTool.Instance.selected == null)
				return;

			if (SelectTool.Instance.selected.TryGetComponent(out Backwall backWall))
			{
				copySettings = backWall.settings;
				hasCopyOverride = true;
			}
			else if(SelectTool.Instance.selected.TryGetComponent(out BackwallUnderConstruction backWallUnderConstruction))
			{
				copySettings = backWallUnderConstruction.settings;
				hasCopyOverride = true;
			}
		}

		public void ClearCopyBuilding()
		{
			hasCopyOverride = false;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}
	}
}
