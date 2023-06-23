using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class MidasEntityContainer : KMonoBehaviour, ISim200ms
	{
		[MyCmpReq] private MinionStorage minionStorage;
		[MyCmpReq] private Storage storage;
		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private SymbolOverrideController controller;
		[MyCmpReq] private KBoxCollider2D collider;

		[Serialize] public float timeRemaining;
		[Serialize] public string sprite;

		public override void OnSpawn()
		{
			base.OnSpawn();
			GetComponent<KSelectable>().AddStatusItem(TStatusItems.GoldStruckStatus, this);
		}

		public void StoreMinion(MinionIdentity identity, float duration)
		{
			Store(identity.gameObject, duration);
			minionStorage.SerializeMinion(identity.gameObject);
		}

		public void StoreCritter(GameObject critter, float duration)
		{
			Store(critter, duration);
			storage.Store(critter);
		}

		private void Store(GameObject go, float duration)
		{
			if (go.HasTag(TTags.midasSafe))
				return;

			CopyAnim(go);
			CopyCollider(go);
			timeRemaining = duration;

			if(TryGetComponent(out KSelectable kSelectable))
				kSelectable.SetName(go.GetProperName());
		}

		private void CopyCollider(GameObject go)
		{
			if (go.TryGetComponent(out KBoxCollider2D collider))
			{
				this.collider.offset = collider.offset;
				this.collider.size = collider.size;
			}
		}

		private void CopyAnim(GameObject original)
		{
			original.TryGetComponent(out KBatchedAnimController originalKbac);

			kbac.SwapAnims(originalKbac.animFiles);

			original.TryGetComponent(out KBatchedAnimController mKbac);
			original.TryGetComponent(out SymbolOverrideController mController);

			if (mKbac.OverrideAnimFiles != null)
			{
				foreach (var animOverride in mKbac.OverrideAnimFiles)
				{
					kbac.AddAnimOverrides(animOverride.file, animOverride.priority);
				}
			}

			foreach (var symbol in mController.symbolOverrides)
			{
				controller.AddSymbolOverride(symbol.targetSymbol, symbol.sourceSymbol);
			}

			if (mKbac.animFiles != null)
			{
				foreach (var mAnim in mKbac.animFiles)
				{
					foreach (var anim in kbac.animFiles)
					{
						if (anim?.data.name == anim.name)
						{
							var build = anim.GetData().build;

							foreach (var symbol in build.symbols)
							{
								kbac.SetSymbolVisiblity(symbol.hash, mKbac.GetSymbolVisiblity(symbol.hash));
							}

							break;
						}
					}
				}
			}

			kbac.Play(originalKbac.currentAnim, KAnim.PlayMode.Paused);
			kbac.SetPositionPercent(originalKbac.GetPositionPercent());
			kbac.animScale = originalKbac.animScale;
			kbac.TintColour = Color.yellow;
			kbac.offset = originalKbac.offset;
			kbac.FlipX = originalKbac.flipX;
			kbac.FlipY = originalKbac.flipY;
			kbac.Rotation = originalKbac.Rotation;
		}

		public void Sim200ms(float dt)
		{
			timeRemaining -= dt;

			if (timeRemaining <= 0)
			{
				storage.DropAll();

				if (minionStorage.serializedMinions.Count > 0)
				{
					for (int i = 0; i < minionStorage.serializedMinions.Count; i++)
					{
						minionStorage.DeserializeMinion(minionStorage.serializedMinions[i].id, transform.position);
					}
				}

				Util.KDestroyGameObject(gameObject);
			}
		}

		public string GetTimeLeft() => GameUtil.GetFormattedTime(timeRemaining);
	}
}
