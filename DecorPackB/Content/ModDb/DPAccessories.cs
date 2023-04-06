using Database;
using FUtility;

namespace DecorPackB.Content.ModDb
{
    public class DPAccessories
    {
        public static Accessory archeologistHat;
        public const string ARCHEOLOGISTHAT_ID = Mod.PREFIX + "ArcheologistHat";

        public static void Register(AccessorySlots accessorySlots)
        {
            var hatSwapAnim = Assets.GetAnim("dp_hat_swap_kanim");
            var bodyCompDefaultAnim = Assets.GetAnim("body_comp_default_kanim");

            AddAccessories(bodyCompDefaultAnim, Db.Get().Accessories, accessorySlots.Hat, hatSwapAnim);
        }

        public static void AddAccessories(KAnimFile defaultBuild, ResourceSet parent, AccessorySlot slot, KAnimFile file)
        {
            var build = file.GetData().build;
            defaultBuild.GetData().build.GetSymbol("snapTo_Hat");

            foreach (var symbol in build.symbols)
            {
                var name = HashCache.Get().Get(symbol.hash);

                if (name.StartsWith("hat"))
                {
                    var accessory = new Accessory(name, parent, slot, file.batchTag, symbol);
                    slot.accessories.Add(accessory);

                    HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
                }
            }
        }
    }
}
