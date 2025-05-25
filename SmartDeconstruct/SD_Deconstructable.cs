using KSerialization;
using SmartDeconstruct.Content;

namespace SmartDeconstruct
{
	public class SD_Deconstructable : KMonoBehaviour
	{
		[Serialize] public Ref<SD_DeconstructionExtractionPoint> extractionPointRef;

		public void MarkForDeconstruction(SD_DeconstructionExtractionPoint point)
		{
			extractionPointRef = new(point);
			GetComponent<KSelectable>().AddStatusItem(SD_StatusItems.queuedForDeconstruction);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			if (extractionPointRef != null)
			{
				var point = extractionPointRef.Get();
				if (point == null)
					return;

			}
		}
		/*		private void CollectConnectedSames()
				{
					var cell = Grid.PosToCell(this);
					var worldIdx = this.GetMyWorldId();

					// see if any neighbor is already in a network
					foreach (var offset in offsets)
					{
						var offsetCell = Grid.OffsetCell(cell, offset);
						if (!Grid.IsValidCellInWorld(offsetCell, worldIdx))
							continue;

						if (SD_DeconstructablesManager.Instance.TryGetNetworkId(offsetCell, out networkId))
						{
							SD_DeconstructablesManager.Instance.Add(networkId, this);
							return;
						}

					}

					// if not start own network
					networkId = SD_DeconstructablesManager.Instance.GetNewNetworkId(cell);
					SD_DeconstructablesManager.Instance.Add(networkId, this);
				}*/
	}
}
