using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    public class Aquarium : KMonoBehaviour
    {
        private AquariumStages.Instance smi;
		GameObject fakeFish;
		Vector3 fishOffset = new Vector3(0f, 1.2f, -1f);
		GameObject originalFish;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			//base.Subscribe<EggIncubator>((int)GameHashes.OccupantChanged, EggIncubator.OnOccupantChangedDelegate);
			smi = new AquariumStages.Instance(this);
			smi.StartSM();
		}

		public void ReplaceFish()
		{
			//gameObject.GetComponent<SingleEntityReceptacle>().OrderRemoveOccupant();
			originalFish = gameObject.GetComponent<SingleEntityReceptacle>().Occupant;
			//Util.KDestroyGameObject(fish);
			originalFish.SetActive(false);
			var prefab = Assets.GetPrefab(FakeFishConfig.ID);
			fakeFish = Util.KInstantiate(prefab, transform.position + fishOffset);
			gameObject.GetComponent<SingleEntityReceptacle>().ForceDepositPickupable(fakeFish.AddOrGet<Pickupable>());
			//fakeFish.SetActive(true);
		}
	}
}
