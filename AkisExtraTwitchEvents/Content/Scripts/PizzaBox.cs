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
		}

		private void OnStorageChanged(object obj)
		{
			if (storage.IsEmpty())
				Util.KDestroyGameObject(gameObject);
		}
	}
}
