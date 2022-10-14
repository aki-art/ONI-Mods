using Database;
using FUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpookyPumpkinSO.Content
{
    internal class SPAccessories
    {
        public const string SKELLINGTON_MOUTH = "mouth_skellington";

        public static void Register(AccessorySlots instance, ResourceSet parent)
        {
            var skellingtonMouth = Assets.GetAnim("sp_skellington_mouth_kanim");
            AddAccessories(skellingtonMouth, instance.Mouth, parent);
        }

        public static void AddAccessories(KAnimFile file, AccessorySlot slot, ResourceSet parent)
        {
            var build = file.GetData().build;
            var id = slot.Id.ToLower();

            for (var i = 0; i < build.symbols.Length; i++)
            {
                var symbolName = HashCache.Get().Get(build.symbols[i].hash);
                Log.Debuglog("RESIGSTERING " + symbolName);
                if (symbolName.StartsWith(id))
                {
                    var accessory = new Accessory(symbolName, parent, slot, file.batchTag, build.symbols[i]);
                    slot.accessories.Add(accessory);
                    HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
                }
            }
        }
    }
}
