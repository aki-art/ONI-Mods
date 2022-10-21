using FUtility;
using Klei.AI;
using SpookyPumpkinSO;
using SpookyPumpkinSO.Content.Cmps;
using System.Collections.Generic;
using UnityEngine;

namespace SpookyPumpkinSO.Content
{
    public class HalloweenCostumeConfig : IEquipmentConfig
    {
        public const string ID = "SP_HalloweenCostume";

        public static List<ComplexRecipe> facadeRecipes = new List<ComplexRecipe>();
        public static readonly ClothingWearer.ClothingInfo clothingInfo = new ClothingWearer.ClothingInfo(global::STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 0.0025f, -1.25f);

        public EquipmentDef CreateEquipmentDef()
        {
            var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
                ID,
                TUNING.EQUIPMENT.CLOTHING.SLOT,
                SimHashes.Carbon,
                TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_MASS,
                "shirt_decor01_kanim",
                TUNING.EQUIPMENT.VESTS.SNAPON0,
                "body_shirt_decor01_kanim",
                4,
                new List<AttributeModifier>(),
                TUNING.EQUIPMENT.VESTS.SNAPON1,
                true,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.75f,
                0.4f);

            equipmentDef.OnEquipCallBack = OnEquip;
            equipmentDef.OnUnequipCallBack = OnUnEquip;

            foreach (var item in Db.Get().EquippableFacades.resources)
            {
                if (item.DefID == ID)
                {
                    TagManager.Create(item.Name, EquippableFacade.GetNameOverride(ID, item.Name));
                }
            }

            return equipmentDef;
        }

        private void OnEquip(Equippable equippable)
        {
            CoolVestConfig.OnEquipVest(equippable, clothingInfo);

            if (equippable.TryGetComponent(out EquippableFacade facade) && ModAssets.SnapOns.Lookup.TryGetValue(facade.FacadeID, out var snapOnId))
            {
                var minion = GetMinionGo(equippable);

                if (minion == null)
                {
                    return;
                }

                var snapOn = minion.AddOrGet<SnapOn>();

                foreach(var id in snapOnId)
                {
                    snapOn.AttachSnapOnByName(id);
                }

                if(facade.FacadeID == SPEquippableFacades.SKELLINGTON)
                {
                    minion.GetComponent<FacePaint>().Apply(SPAccessories.SKELLINGTON_MOUTH);
                }
            }
        }

        private void OnUnEquip(Equippable equippable)
        {
            CoolVestConfig.OnUnequipVest(equippable);

            if (equippable.TryGetComponent(out EquippableFacade facade) && ModAssets.SnapOns.Lookup.TryGetValue(facade.FacadeID, out var snapOnId))
            {
                var minion = GetMinionGo(equippable);

                if (minion == null)
                {
                    return;
                }

                var snapOn = minion.AddOrGet<SnapOn>();
                foreach (var id in snapOnId)
                {
                    snapOn.DetachSnapOnByName(id);
                }

                minion.GetComponent<FacePaint>().Restore();
            }
        }

        private static GameObject GetMinionGo(Equippable obj)
        {
            return obj.assignee?.GetSoleOwner()?.GetComponent<MinionAssignablesProxy>()?.GetTargetGameObject();
        }

        public void DoPostConfigure(GameObject go)
        {
            CustomClothingConfig.SetupVest(go);
            go.AddTag(GameTags.PedestalDisplayable);
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;
    }
}
