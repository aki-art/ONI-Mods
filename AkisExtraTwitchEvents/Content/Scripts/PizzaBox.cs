using KSerialization;
using Twitchery.Content.Defs;

namespace Twitchery.Content.Scripts
{
	public class PizzaBox : KMonoBehaviour
	{
		[MyCmpReq] public Storage storage;
		[Serialize] public bool hasFilled;

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (!hasFilled && storage.IsEmpty())
			{
				var pizzas = FUtility.Utils.Spawn(PizzaConfig.ID, gameObject);

				if (pizzas.TryGetComponent(out PrimaryElement primaryElement))
					primaryElement.SetMass(Mod.Settings.Pizzabox_Kcal / TFoodInfos.PIZZA_KCAL_PER_KG);

				if (TryGetComponent(out Storage storage))
					storage.Store(pizzas);
			}

			Subscribe((int)GameHashes.OnStorageChange, OnStorageChanged);
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
		}

		private void OnRefreshUserMenu(object obj)
		{
			var button = new KIconButtonMenu.ButtonInfo(
				"action_empty_contents",
				STRINGS.UI.AKIS_EXTRA_TWITCH_EVENTS.UNLOAD.LABEL,
				DropContents,
				tooltipText: STRINGS.UI.AKIS_EXTRA_TWITCH_EVENTS.UNLOAD.TOOLTIP);

			Game.Instance.userMenu.AddButton(gameObject, button);
		}

		private void DropContents()
		{
			storage.DropAll();
			Util.KDestroyGameObject(gameObject);
		}

		private void OnStorageChanged(object obj)
		{
			if (storage.IsEmpty())
				Util.KDestroyGameObject(gameObject);
		}
	}
}
