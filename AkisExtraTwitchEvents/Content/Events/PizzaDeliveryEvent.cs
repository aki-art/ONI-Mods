using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
{
    public class PizzaDeliveryEvent : ITwitchEvent
    {
        public const string ID = "PizzaDelivery";

        public bool Condition(object data) => true;

        public string GetID() => ID;

        public void Run(object data)
        {
            var telepad = GameUtil.GetActiveTelepad();
            var position = telepad == null 
                ? Grid.CellToPos(ONITwitchLib.Utils.PosUtil.RandomCellNearMouse()) 
                : telepad.transform.position;

            var box = FUtility.Utils.Spawn(PizzaBoxConfig.ID, position + Vector3.up);
            var pizzas = FUtility.Utils.Spawn(PizzaConfig.ID, position);
            if(pizzas.TryGetComponent(out PrimaryElement primaryElement))
            {
                primaryElement.SetMass(12f);
            }

            if(box.TryGetComponent(out Storage storage))
            {
                storage.Store(pizzas);
            }

            AkisTwitchEvents.Instance.hasUnlockedPizzaRecipe = true;

            var message = "Pizza time!";
            if(AkisTwitchEvents.Instance.hasUnlockedPizzaRecipe)
            {
                message += "\nAlso new recipe at Gas Range.";
            }

            ONITwitchLib.ToastManager.InstantiateToastWithGoTarget("Pizza Delivery", message, box);
        }
    }
}
