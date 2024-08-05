using Database;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackB.Content.Db
{
	public class DPPermitDioaramaVis_Museum : KMonoBehaviour, IKleiPermitDioramaVisTarget
	{
		[SerializeField] public KBatchedAnimController buildingKAnim;
		[SerializeField] public Image background;

		public void ConfigureSetup()
		{
			SymbolOverrideControllerUtil.AddToPrefab(buildingKAnim.gameObject);
			background.sprite = Assets.GetSprite(ModAssets.Sprites.FOSSIL_BG);
		}

		public void ConfigureWith(PermitResource permit) => KleiPermitVisUtil.ConfigureToRenderBuilding(buildingKAnim, (ArtableStage)permit);

		public GameObject GetGameObject() => gameObject;
	}
}
