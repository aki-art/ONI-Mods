using Klei.AI;
using SpookyPumpkinSO.Content.Cmps;
using System.Collections.Generic;
using UnityEngine;

namespace SpookyPumpkinSO.Content.Equipment
{
	public class HalloweenCostumeConfig : IEquipmentConfig
	{
		public const string ID = "SP_HalloweenCostume";

		public static List<ComplexRecipe> facadeRecipes = new List<ComplexRecipe>();
		public static readonly ClothingWearer.ClothingInfo clothingInfo = new ClothingWearer.ClothingInfo(
			global::STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME,
			40,
			0.0025f,
			-1.25f);

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

			foreach (var item in Db.GetEquippableFacades().resources)
			{
				if (item.DefID == ID)
					TagManager.Create(item.Id, EquippableFacade.GetNameOverride(ID, item.Id));
			}

			return equipmentDef;
		}

		private void OnEquip(Equippable equippable)
		{
			ClothingWearer.ClothingInfo.OnEquipVest(equippable, clothingInfo);

			if (equippable.TryGetComponent(out EquippableFacade facade))
			{
				var minion = GetMinionGo(equippable);

				if (minion == null)
					return;

				if (minion.TryGetComponent(out FacePaint facePoint))
				{
					switch (facade.FacadeID)
					{
						case SPEquippableFacades.SKELLINGTON:
							facePoint.Apply(SPAccessories.SKELLINGTON_MOUTH);
							break;
						case SPEquippableFacades.SCARECROW:
							facePoint.Apply(SPAccessories.SCARECROW_MOUTH);
							break;
					}
				}
			}
		}

		private void OnUnEquip(Equippable equippable)
		{
			ClothingWearer.ClothingInfo.OnUnequipVest(equippable);

			var minion = GetMinionGo(equippable);

			if (minion != null && minion.TryGetComponent(out FacePaint facePoint))
				facePoint.Remove();
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
