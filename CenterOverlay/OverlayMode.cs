namespace CenterOverlay
{
	public class MirrorSide : OverlayModes.Mode
	{
		public override HashedString ViewMode() => ID;
		public static readonly HashedString ID = "MirrorSide";
		public override string GetSoundName() => "Decor";
	}
}
