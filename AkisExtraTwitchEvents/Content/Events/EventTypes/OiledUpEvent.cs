using Klei.AI;
using ONITwitchLib;
using Twitchery.Utils;

namespace Twitchery.Content.Events.EventTypes
{
	public class OiledUpEvent() : TwitchEventBase(ID)
	{
		public const string ID = "OiledUp";

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			foreach (var minion in Components.MinionIdentities.Items)
			{
				var effects = minion.GetComponent<Effects>();

				effects.Add(TEffects.OILED_UP, true);

				if (minion.model == GameTags.Minions.Models.Bionic)
				{
					var smi = minion.GetSMI<BionicOilMonitor.Instance>();

					if (smi == null)
					{
						Log.Warning($"{minion.name} is a Bionic dupe without an oil tank.");
					}
					else if (BionicOilMonitor.WantsOilChange(smi))
					{
						smi.RefillOil(200f - smi.CurrentOilMass);
						effects.Add(BionicOilMonitor.LUBRICANT_TYPE_EFFECT[SimHashes.CrudeOil], true);
					}
				}

				SimMessages.ReplaceAndDisplaceElement(Grid.PosToCell(minion), SimHashes.CrudeOil, AGridUtil.cellEvent, 5f, 303.15f);

				ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.OILEDUP.TOAST, STRINGS.AETE_EVENTS.OILEDUP.DESC);
			}
		}
	}
}
