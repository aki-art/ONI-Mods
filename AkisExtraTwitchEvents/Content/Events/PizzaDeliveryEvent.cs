﻿using Twitchery.Content.Defs;
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

            var message = STRINGS.AETE_EVENTS.PIZZA_DELIVERY.DESC;
            if(AkisTwitchEvents.Instance.hasUnlockedPizzaRecipe)
            {
                message += STRINGS.AETE_EVENTS.PIZZA_DELIVERY.DESC_RECIPE;
            }

            AudioUtil.PlaySound(ModAssets.Sounds.DOORBELL, ModAssets.GetSFXVolume());

            ONITwitchLib.ToastManager.InstantiateToastWithGoTarget(
                STRINGS.AETE_EVENTS.PIZZA_DELIVERY.TOAST,
                message, 
                box);
        }
    }
}