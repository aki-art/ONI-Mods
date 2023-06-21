using ONITwitchLib;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
	// this event is not added to the rotation by default
	// instead, it gets added as variations
	public class PolymorphEvent : ITwitchEvent
	{
		public const string ID = "Polymorph";

		public bool Condition(object data) => Components.LiveMinionIdentities.Count > 0; // check if not all are turned yet

		public string GetID() => ID;

		public void Run(object data)
		{
			var identity = Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId).GetRandom();
			var creaturePrefabId = PolymorphFloorCritterConfig.ID;

			var critter = FUtility.Utils.Spawn(creaturePrefabId, identity.transform.position);
			var morph = TDb.polymorphs.GetRandom();

			var toast = STRINGS.AETE_EVENTS.POLYMOPRH.DESC
				.Replace("{Dupe}", identity.GetProperName())
				.Replace("{Critter}", morph.Name);

			critter.GetComponent<AETE_PolymorphCritter>().SetMorph(identity, morph);

			ToastManager.InstantiateToastWithGoTarget(STRINGS.AETE_EVENTS.POLYMOPRH.TOAST_ALT, toast, critter.gameObject);
			AudioUtil.PlaySound(ModAssets.Sounds.POLYMORHPH, ModAssets.GetSFXVolume());
			Game.Instance.SpawnFX(ModAssets.Fx.pinkPoof, critter.transform.position, 0);
		}
	}
}
