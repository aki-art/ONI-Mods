using SketchPad.Tools.Pencil;

namespace SketchPad
{
    public class SketchOverlayMode : OverlayModes.Mode
    {
        public static StatusItem.StatusItemOverlays overlayItem = (StatusItem.StatusItemOverlays)0x35101;

        public override HashedString ViewMode() => ID;

        public static readonly HashedString ID = "SketchOverlay";

        public override string GetSoundName() => "Decor";

        public override void Enable()
        {
            base.Enable();
            PencilTool.Instance.ActivateTool();
        }

        public override void Disable()
        {
            base.Disable();
            PencilTool.Instance.DeactivateTool();
        }
    }
}
