using Database;

namespace SpookyPumpkinSO.Content
{
	public class SPAccessories
	{
		public const string SKELLINGTON_MOUTH = "mouth_skellington";
		public const string SCARECROW_MOUTH = "mouth_scarecrow";

		public static void Register(AccessorySlots instance, ResourceSet parent)
		{
			AddAccessories(Assets.GetAnim("sp_skellington_mouth_kanim"), instance.Mouth, parent);
			AddAccessories(Assets.GetAnim("sp_scarecrow_mouth_kanim"), instance.Mouth, parent);
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
				}
			}
		}
	}
}
