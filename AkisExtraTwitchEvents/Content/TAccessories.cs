using Database;
using FUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitchery.Content
{
	public class TAccessories
	{
		public static void Register(Accessories accessories, AccessorySlots slots)
		{
			var hulkHead = Assets.GetAnim("aete_hulk_head_kanim");

			AddAccessories(hulkHead, slots.Hair, accessories);
			AddAccessories(hulkHead, slots.HatHair, accessories);
			AddAccessories(hulkHead, slots.Mouth, accessories);
			AddAccessories(hulkHead, slots.HeadShape, accessories);
		}

		public static void AddAccessories(KAnimFile file, AccessorySlot slot, ResourceSet parent)
		{
			var build = file.GetData().build;
			var id = slot.Id.ToLower();

			for (var i = 0; i < build.symbols.Length; i++)
			{
				var symbolName = HashCache.Get().Get(build.symbols[i].hash);
				if (symbolName.StartsWith(id))
				{
					var accessory = new Accessory(symbolName, parent, slot, file.batchTag, build.symbols[i]);
					slot.accessories.Add(accessory);
					HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);

					Log.Debuglog("Added accessory: " + accessory.Id);
				}
			}
		}
	}
}
