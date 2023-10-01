using HarmonyLib;
using KSerialization;
using System.Collections.Generic;

namespace SpookyPumpkinSO.Content.Cmps
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SpiceRestorer : KMonoBehaviour
	{
		[Serialize]
		public bool hasPumpkinSpice;

		[Serialize]
		private SpiceInstance spiceInstance;

		[MyCmpReq]
		private Edible edible;

		private static AccessTools.FieldRef<Edible, List<SpiceInstance>> ref_Spices;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			ref_Spices = AccessTools.FieldRefAccess<Edible, List<SpiceInstance>>("spices");
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			Mod.spiceRestorers.Add(this);

			if (hasPumpkinSpice)
			{
				var spice = new SpiceInstance
				{
					Id = SPSpices.PUMPKIN_SPICE_ID,
					TotalKG = SPSpices.PumpkinSpice.TotalKG
				};

				edible.SpiceEdible(spice, SpiceGrinderConfig.SpicedStatus);
			}
		}

		public void OnSaveGame()
		{
			var spices = ref_Spices(edible);

			var index = spices.FindIndex(s => s.Id == SPSpices.PUMPKIN_SPICE_ID);

			if (index == -1)
			{
				hasPumpkinSpice = false;
				return;
			}

			hasPumpkinSpice = true;
			spiceInstance = spices[index];
			spices.RemoveAt(index);
		}

		public void OnLoadGame()
		{
			if (hasPumpkinSpice)
			{
				ref_Spices(edible).Add(spiceInstance);
			}
		}
	}
}
