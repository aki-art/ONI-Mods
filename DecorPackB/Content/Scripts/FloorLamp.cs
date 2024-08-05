using DecorPackB.Content.Db;
using KSerialization;
using UnityEngine;

namespace DecorPackB.Content.Scripts
{
	public class FloorLamp : KMonoBehaviour
	{
		[Serialize] public string paneId;

		[MyCmpReq] private Deconstructable deconstructable;
		[MyCmpReq] private KBatchedAnimController kbac;

		public override void OnSpawn()
		{
			base.OnSpawn();
			kbac.Offset = new Vector3(0f, 0.5f);

			if (paneId.IsNullOrWhiteSpace())
				paneId = FloorLampPane.GetIdFromElement(deconstructable.constructionElements[1].ToString());

			var pane = ModDb.FloorLampPanes.TryGet(paneId);
			if (pane != null)
			{
				var currentAnim = kbac.CurrentAnim.name;
				kbac.SwapAnims([Assets.GetAnim(pane.animFile)]);
				kbac.Play(currentAnim);

				GetComponent<Light2D>().Color = pane.lightColor;
			}
		}
	}
}
