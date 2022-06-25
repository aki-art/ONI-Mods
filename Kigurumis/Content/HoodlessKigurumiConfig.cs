using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Kigurumis.Content
{
    public class HoodlessKigurumiConfig : IEquipmentConfig
    {
        public const string ID = "HoodlessKigurumi";

        public static readonly ClothingWearer.ClothingInfo clothingInfo = new ClothingWearer.ClothingInfo(STRINGS.EQUIPMENT.PREFABS.KIGURUMI.NAME, 40, 0.0025f, -1.25f);

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

            foreach (var kigu in Db.Get().EquippableFacades.resources)
            {
                if (kigu.DefID == ID)
                {
                    TagManager.Create(kigu.Name, EquippableFacade.GetNameOverride(ID, kigu.Name));
                }
            }

            return equipmentDef;
        }

        private void OnEquip(Equippable equippable)
        {
            CoolVestConfig.OnEquipVest(equippable, clothingInfo);

            if (Mod.Settings.HoodieDefaultState == Config.HoodieState.Never)
            {
                return;
            }

            if(GetMinionGo(equippable) is GameObject minion)
            {
                var hoodieWearer = minion.AddOrGet<HoodieWearer>();
                hoodieWearer.facadeID = equippable.GetComponent<EquippableFacade>().FacadeID;
                hoodieWearer.ToggleHoodie(false);
            }
        }

        private void OnUnEquip(Equippable equippable)
        {
            CoolVestConfig.OnUnequipVest(equippable);
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
