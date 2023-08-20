using Database;
using FUtility;
using System;

namespace Twitchery.Content
{
	public class TAccessories
	{
		public static void Register(Accessories accessories, AccessorySlots slots)
		{
			var hulkHead = Assets.GetAnim("aete_hulk_head_kanim");
			var hulkBody = Assets.GetAnim("aete_hulk_body_kanim");

			AddAccessories(hulkHead, slots.Hair, accessories);
			AddAccessories(hulkHead, slots.HatHair, accessories);
			AddAccessories(hulkHead, slots.Mouth, accessories);
			AddAccessories(hulkHead, slots.HeadShape, accessories);

			AddCustomAccessories(Assets.GetAnim("aete_hulk_body_kanim"), accessories, slots);
		}

		public static void AddCustomAccessories(KAnimFile anim_file, ResourceSet parent, AccessorySlots slots)
		{
			if (anim_file == null)
				return;

			KAnim.Build build = anim_file.GetData().build;
			for (int i = 0; i < build.symbols.Length; i++)
			{
				string symbol_name = HashCache.Get().Get(build.symbols[i].hash);
				var accessorySlot = slots.resources.Find((AccessorySlot slot) => symbol_name.IndexOf(slot.Id, 0, StringComparison.OrdinalIgnoreCase) != -1);
				if (accessorySlot != null)
				{
					var accessory = new Accessory(symbol_name, parent, accessorySlot, anim_file.batchTag, build.symbols[i], anim_file);
					accessorySlot.accessories.Add(accessory);
					HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
					Log.Debuglog("added accessory: " + accessory.Id);
				}
			}
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
