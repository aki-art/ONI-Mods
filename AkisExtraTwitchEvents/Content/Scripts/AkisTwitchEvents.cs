using KSerialization;
using ONITwitchLib;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AkisTwitchEvents : KMonoBehaviour
	{
		public static AkisTwitchEvents Instance;

		[Serialize] public float lastLongBoiSpawn;
		[Serialize] public float lastRadishSpawn;
		[Serialize] internal bool hasRaddishSpawnedBefore;
		[Serialize] public bool hasUnlockedPizzaRecipe;
		[Serialize] public bool hasFixedGoop;

		public float originalLiquidTransparency;
		public bool hideLiquids;
		public bool eggActive;
		public AETE_EggPostFx eggFx;

		public static EventInfo polymorph;
		public static MinionIdentity polymorphTarget;
		public static string polyTargetName;

		public static string pizzaRecipeID;
		public static string radDishRecipeID;

		public void ApplyLiquidTransparency(WaterCubes waterCubes)
		{
			if (originalLiquidTransparency == 0)
				originalLiquidTransparency = waterCubes.material.GetFloat("_BlendScreen");

			waterCubes.material.SetFloat("_BlendScreen", hideLiquids ? 0 : originalLiquidTransparency);
		}

		public AkisTwitchEvents()
		{
			lastLongBoiSpawn = float.NegativeInfinity;
			lastRadishSpawn = 0;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public void OnVoteOver()
		{

		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			if(!hasFixedGoop)
			{
				var goop = ElementLoader.GetElementIndex(Elements.PinkSlime);

				for (int i = 0; i < Grid.CellCount; i++)
				{
					if (Grid.ElementIdx[i] == goop)
					{
						var mass = Grid.Mass[i];
						SimMessages.ConsumeMass(i, Elements.PinkSlime, mass * 0.9f, 1);
					}
				}

				hasFixedGoop = true;
			}
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public void OnDraw()
		{
			polymorphTarget = Components.LiveMinionIdentities.GetRandom();
			polyTargetName = polymorph.FriendlyName = $"Polymorph {Util.StripTextFormatting(polymorphTarget.GetProperName())}";
		}
	}
}
