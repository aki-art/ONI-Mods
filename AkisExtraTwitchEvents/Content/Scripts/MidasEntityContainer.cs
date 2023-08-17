﻿using FUtility;
using KSerialization;
using System;
using System.Collections;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class MidasEntityContainer : KMonoBehaviour, ISim200ms
	{
		[MyCmpReq] private MinionStorage minionStorage;
		[MyCmpReq] private Storage storage;
		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private SymbolOverrideController controller;
		[MyCmpReq] private KBoxCollider2D collider;

		[Serialize] public float timeRemaining;
		[Serialize] public string sprite;
		[Serialize] public bool storedItem;
		[Serialize] public bool storedMinion;
		[Serialize] public HashedString currentAnim;
		[Serialize] public float positionPercent;

		private bool restoreAnim = true;

		public static readonly Tag[] ignoreTags = new[]
		{
			TTags.midased,
			TTags.midasSafe,
			GameTags.Stored
		};

		public override void OnSpawn()
		{
			base.OnSpawn();
			GetComponent<KSelectable>().AddStatusItem(TStatusItems.GoldStruckStatus, this);

			if (restoreAnim && (storedItem || storedMinion))
				StartCoroutine(RestoreAnim());

			Mod.midasContainers.Add(this);
			//ClusterManager.Instance.Subscribe((int)GameHashes.WorldRemoved, OnWorldRemoved);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.midasContainers.Remove(this);
		}

		public void StoreMinion(MinionIdentity identity, float duration)
		{
			if (storedMinion)
				return;

			Store(identity.gameObject, duration, false);
			minionStorage.SerializeMinion(identity.gameObject);

			storedMinion = true;
			restoreAnim = false;

			Mod.midasContainersWithDupes.Add(this);
		}

		public void StoreCritter(GameObject critter, float duration)
		{
			if (storedItem)
				return;

			Store(critter, duration, false);
			storage.Store(critter, true);

			storedItem = true;
			restoreAnim = false;
		}

		private IEnumerator RestoreAnim()
		{
			var item = Release(false);

			yield return new WaitForEndOfFrame();

			if (item != null)
			{
				if (item.TryGetComponent(out MinionIdentity identity))
					minionStorage.SerializeMinion(identity.gameObject);
				else
					storage.Store(item, true);

				Store(item, timeRemaining, true);
			}
		}

		private void Store(GameObject go, float duration, bool force)
		{
			Log.Debuglog("storing " + go.name);
			if (!force && go.HasAnyTags(ignoreTags))
				return;

			CopyAnim(go);
			CopyCollider(go);
			timeRemaining = duration;

			if (TryGetComponent(out KSelectable kSelectable))
				kSelectable.SetName(go.GetProperName());

			if (go.TryGetComponent(out KPrefabID kPrefabID))
				kPrefabID.AddTag(TTags.midased, true);

			storedItem = true;
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

			if (!currentAnim.IsValid)
			{
				currentAnim = originalKbac.currentAnim;
				positionPercent = originalKbac.GetPositionPercent();
			}

			kbac.Play(currentAnim, KAnim.PlayMode.Paused);
			kbac.SetPositionPercent(positionPercent);
			kbac.animScale = originalKbac.animScale;
			kbac.TintColour = Color.yellow;
			kbac.offset = originalKbac.offset;
			kbac.FlipX = originalKbac.flipX;
			kbac.FlipY = originalKbac.flipY;
			kbac.Rotation = originalKbac.Rotation;

		}

		public GameObject Release(bool removeTag)
		{
			if (removeTag)
			{
				foreach (var item in storage.items)
					item.RemoveTag(TTags.midased);
			}

			if (storage.items != null && storage.items.Count > 0)
			{
				var go = storage.items[0];
				storage.DropAll();

				return go;
			}

			if (minionStorage.serializedMinions.Count > 0)
			{
				if (minionStorage.serializedMinions.Count > 1)
					Log.Warning("There is somehow multiple dupes in this midas state. ignoring all but the first");

				var minionGo = minionStorage.DeserializeMinion(minionStorage.serializedMinions[0].id, transform.position);

				if (removeTag)
					minionGo.RemoveTag(TTags.midased);

				Mod.midasContainersWithDupes.Remove(this);

				return minionGo;
			}

			return null;
		}

		public void Sim200ms(float dt)
		{
			timeRemaining -= dt;

			if (timeRemaining <= 0)
			{
				Release(true);
				Util.KDestroyGameObject(gameObject);
			}

			kbac.TintColour = Color.yellow; // need to reapply because the game randomly clears it
		}

		public string GetTimeLeft() => GameUtil.GetFormattedTime(timeRemaining);
	}
}
