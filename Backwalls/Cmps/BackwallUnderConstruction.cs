using KSerialization;

namespace Backwalls.Cmps
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class BackwallUnderConstruction : KMonoBehaviour
	{
		[Serialize] public Backwall.BackwallSettings settings;
		[Serialize] public bool hasCopyData;

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (!hasCopyData && Backwalls_Mod.Instance.hasCopyOverride)
			{
				settings = Backwalls_Mod.Instance.copySettings;
				hasCopyData = true;
			}
		}
	}
}
