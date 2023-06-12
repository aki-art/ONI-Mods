using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using Twitchery.Content.Events;
using static UnityEngine.UI.Image;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AkisTwitchEvents : KMonoBehaviour, ISim4000ms, ISim1000ms
	{
		public static AkisTwitchEvents Instance;

		[Serialize]
		public float lastLongBoiSpawn;

		[Serialize]
		public float lastRadishSpawn;

		[Serialize]
		public float dupedDupePurgeTime;

		[Serialize]
		internal bool hasRaddishSpawnedBefore;

		[Serialize]
		public bool hasUnlockedPizzaRecipe;

		[Serialize]
		public List<Ref<MinionIdentity>> duplicateDupes;

		public float originalLiquidTransparency;
		public bool hideLiquids;


		//[Serialize]
		//public List<Ref<KPrefabID>> wormies;

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

		/*
                public void AddLongWormy(GameObject wormy)
                {
                    if (wormy != null && wormy.TryGetComponent(out KPrefabID kPrefabID))
                    {
                        wormies ??= new();
                        wormies.Add(new Ref<KPrefabID>(kPrefabID));
                    }
                }

                public bool HasWormy(GameObject wormy)
                {
                    if(wormies == null)
                    {
                        return false;
                    }

                    if (wormy.TryGetComponent(out KPrefabID kPrefabID))
                    {
                        return (wormies.Any(w => w != null && w.Get() == kPrefabID));
                    }

                    return false;
                }

                public void RemoveWormy(GameObject wormy)
                {
                    if (wormy.TryGetComponent(out KPrefabID kPrefabID))
                    {
                        wormies?.RemoveAll(w => w != null && w.Get() == kPrefabID);
                    }
                }
        */
		public override void OnPrefabInit()
		{
			Instance = this;
		}

		public override void OnSpawn()
		{
			Log.Debuglog("akis twitch events spawn");
			base.OnSpawn();

			if (duplicateDupes != null)
			{
				foreach (var dupeRef in duplicateDupes)
				{
					var dupe = dupeRef.Get();
					if (dupe != null)
					{
						dupe.GetComponent<KSelectable>().AddStatusItem(TStatusItems.DupeStatus, dupe);
					}
				}
			}
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public void Sim4000ms(float dt)
		{
			if (TwitchEvents.myEvents.Count > 0)
			{
				foreach (var ev in TwitchEvents.myEvents)
				{
					Log.Debuglog($"{ev.GetID()} {ev.Condition(null)}");
				}
			}
		}

		public void Sim1000ms(float dt)
		{
			if (dupedDupePurgeTime > 0 && dupedDupePurgeTime < GameClock.Instance.GetTimeInCycles())
			{
				if (duplicateDupes == null)
					return;

				foreach (var dupe in duplicateDupes)
				{
					var dupeGo = dupe.Get();
					if (dupeGo != null)
					{
						Log.Debuglog("destroying minion");
						Util.KDestroyGameObject(dupeGo.gameObject);
					}
				}

				duplicateDupes.Clear();
				dupedDupePurgeTime = 0;
			}
		}

	}
}
