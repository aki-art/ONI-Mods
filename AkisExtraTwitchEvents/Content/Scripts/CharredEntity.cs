using Twitchery.Content.Defs;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	class CharredEntity : KMonoBehaviour
	{
		[MyCmpReq] public AnimationMimic mimic;
		[MyCmpReq] public KBoxCollider2D collider;
		[MyCmpReq] public KBatchedAnimController kbac;
		[MyCmpReq] public KSelectable kSelectable;

		[SerializeField] HashedString pastPersonalityId;

		private static readonly Color gray = new(0.05f, 0.05f, 0.05f, 1f);

		public static readonly Tag[] ignoreTags =
		[
			TTags.midased,
			TTags.midasSafe,
			GameTags.Stored,
		];

		public override void OnSpawn()
		{
			base.OnSpawn();
			UpdateKAnim();
		}

		private void UpdateKAnim()
		{
			kbac.TintColour = gray;
			kbac.SetBlendValue(0.7f);
			kbac.SetDirty();
			kbac.UpdateAnim(0);
		}

		public static void CreateAndChar(GameObject original)
		{
			if (original.HasAnyTags(ignoreTags))
				return;

			var entity = FUtility.Utils.Spawn(CharredEntityContainerConfig.ID, original.gameObject);
			entity.SetActive(true);
			entity.GetComponent<CharredEntity>().Char(original);

			//entity.AddTag(GameTags.PreventChoreInterruption);
			entity.AddTag(GameTags.Corpse);

			var deathMessage = new DeathMessage(entity, TDeaths.lasered);
			KFMOD.PlayOneShot(GlobalAssets.GetSound("Death_Notification_localized"), entity.transform.position);
			KFMOD.PlayUISound(GlobalAssets.GetSound("Death_Notification_ST"));
			Messenger.Instance.QueueMessage(deathMessage);

			Util.KDestroyGameObject(original);
		}

		public void Char(GameObject original)
		{
			foreach (var storage in original.GetComponents<Storage>())
				storage.DropAll();

			CopyCollider(original);
			mimic.ScribeAnimation(original);
			mimic.ApplyAnimation();

			UpdateKAnim();

			if (original.TryGetComponent(out MinionIdentity identity))
				pastPersonalityId = identity.personalityResourceId;

			kSelectable.SetName(original.GetProperName());

			if (original.TryGetComponent(out InfoDescription description))
				description.description = $"A mass of carbon in the shape of a(n) {TagManager.StripLinkFormatting(original.GetProperName())}";
		}

		private void CopyCollider(GameObject go)
		{
			if (go.TryGetComponent(out KBoxCollider2D collider))
			{
				this.collider.offset = collider.offset;
				this.collider.size = collider.size;
			}
		}
	}
}
