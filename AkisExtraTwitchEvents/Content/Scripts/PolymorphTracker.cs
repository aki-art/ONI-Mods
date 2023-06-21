/*using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class PolymorphTracker : KMonoBehaviour
	{
		[MyCmpReq] private MinionStorage minionStorage;

		[Serialize] private List<Info> polymorphs;

		public PolymorphTracker() 
		{
			polymorphs = new List<Info>();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			AkisTwitchEvents.Instance.SetPolymorphTracker(this);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			AkisTwitchEvents.Instance.SetPolymorphTracker(null);
		}

		public void Polymorph(MinionIdentity identity, string targetSpecies)
		{
			var critter = FUtility.Utils.Spawn(targetSpecies, identity.transform.position);

			critter.gameObject.AddOrGet<UserNameable>().SetName(identity.GetProperName());

			var polymorph = critter.gameObject.AddOrGet<AETE_PolymorphCritter>();
			polymorph.duration = 100f;

			minionStorage.SerializeMinion(identity.gameObject);
			var guid = minionStorage.serializedMinions.Last().id;
			polymorph.minionRef = guid;

			polymorphs.Add(new Info(guid, new Ref<AETE_PolymorphCritter>(polymorph), GameClock.Instance.time));
		}

		public void RestoreDupe(Guid minionRef)
		{
			foreach(var info in polymorphs)
			{
				if(info.id == minionRef)
				{
					var critter = info.polymorphRef.Get();

					Vector3 position;

					if(critter == null)
					{
						Log.Warning("Polymorphed critter not found. restoring duplicant to a telepad");
						position = GameUtil.GetActiveTelepad().transform.position;
					}
					else
						position = critter.transform.position;

					minionStorage.DeserializeMinion(info.id, position);

					return;
				}
			}

			Log.Warning("Tried to restore a dupe from polymorph state, but there is no stored data with it's Guid.");
		}

		public struct Info
		{
			public Guid id;
			public Ref<AETE_PolymorphCritter> polymorphRef;
			public float transformationTime;

			public Info(Guid id, Ref<AETE_PolymorphCritter> polymorphRef, float transformationTime)
			{
				this.id = id;
				this.polymorphRef = polymorphRef;
				this.transformationTime = transformationTime;
			}
		}
	}
}
*/