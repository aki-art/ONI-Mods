using ONITwitchLib;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class PizzaDeliveryEvent() : TwitchEventBase(ID)
	{
		public const string ID = "PizzaDelivery";
		public override int GetWeight() => WEIGHTS.COMMON;

		public override Danger GetDanger() => Danger.None;

		public string GetID() => ID;

		public override void Run()
		{
			if (Game.Instance == null)
				return;

			var telepad = GameUtil.GetActiveTelepad();
			var position = telepad == null
				? Grid.CellToPos(ONITwitchLib.Utils.PosUtil.RandomCellNearMouse())
				: telepad.transform.position;

			var box = FUtility.Utils.Spawn(PizzaBoxConfig.ID, position + Vector3.up);

			AkisTwitchEvents.Instance.hasUnlockedPizzaRecipe = true;

			var message = STRINGS.AETE_EVENTS.PIZZADELIVERY.DESC;
			if (AkisTwitchEvents.Instance.hasUnlockedPizzaRecipe)
			{
				message += STRINGS.AETE_EVENTS.PIZZADELIVERY.DESC_RECIPE;
			}

			AudioUtil.PlaySound(ModAssets.Sounds.DOORBELL, ModAssets.GetSFXVolume());

			var toast = ToastManager.InstantiateToastWithGoTarget(
				STRINGS.AETE_EVENTS.PIZZADELIVERY.TOAST,
				message,
				box);

			ConfigureGif(AddCustomContainerToToast(toast)); // TODO: fix
		}

		private void ConfigureGif(GameObject gameObject)
		{
			var kbac = FXHelpers.CreateEffect("aete_pizzatime_kanim", Vector3.zero, gameObject.transform);

			kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
			kbac.animScale = 0.25f;
			kbac.setScaleFromAnim = false;
			kbac.isMovable = false;
			kbac.materialType = KAnimBatchGroup.MaterialType.UI;
			kbac.animOverrideSize = new Vector2(100, 100);
			kbac.usingNewSymbolOverrideSystem = true;
			kbac.SetLayer(5);
			kbac.SetDirty();

			kbac.Play("pizzatime");
		}
	}
}
