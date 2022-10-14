using FUtility;

namespace SpookyPumpkinSO.Content.Cmps
{
    internal class FacePaint : KMonoBehaviour
    {
        [MyCmpGet]
        private Accessorizer accessorizer;

        [MyCmpGet]
        private SymbolOverrideController symbolOverrideController;

        string animFile = "sp_skellingtonfacepaint_kanim";
        string mouth = "sp_skellington_mouth_kanim";

        public void Apply(string prefix)
        {
            if (accessorizer != null && symbolOverrideController != null)
            {
                Log.Debuglog("adding override");
                string str = "snapto_cheek";
                string mouth = "snapto_mouth";
                var kAnimFile = Assets.GetAnim(animFile);
                var mouthAnim = Assets.GetAnim(mouth);
                //symbolOverrideController.AddSymbolOverride("snapto_cheek", kAnimFile.GetData().build.GetSymbol(str), 99);
                //symbolOverrideController.AddSymbolOverride(Db.Get().AccessorySlots.Mouth.targetSymbolId, kAnimFile.GetData().build.GetSymbol(mouth), 99);

                var accessorySlots = Db.Get().AccessorySlots;
                var accessory = accessorizer.GetAccessory(accessorySlots.HeadShape);
                Log.Debuglog(accessory.Id);
                //accessorizer.RemoveAccessory(accessorizer.GetAccessory(accessorySlots.HeadShape));
                accessorizer.RemoveAccessory(accessorizer.GetAccessory(accessorySlots.Mouth));
                //accessorizer.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup("headshape_001"));
                //accessorizer.AddAccessory(accessorySlots.Mouth.Lookup(SPAccessories.SKELLINGTON_MOUTH));
                accessorizer.AddAccessory(accessorySlots.Mouth.Lookup(SPAccessories.SKELLINGTON_MOUTH));
                accessorizer.ApplyAccessories();
            }
        }
    }
}
