using ImGuiNET;
using KSerialization;
using System;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class AETE_GraveStoneMinionStorage : KMonoBehaviour, IImguiDebug
	{
		[Obsolete][MyCmpGet] private MinionStorage minionStorage;
		[MyCmpGet] private Grave grave;
		[MyCmpGet] private AETE_GameObjectSerializer minionSerializer;

		[Serialize] public bool migrated;

		[Serialize] public byte[] minionData;

		public bool HasDupe() => minionSerializer.HasObject();

		public bool HasStoredLegacyDupe() => !migrated && minionStorage.serializedMinions != null && minionStorage.serializedMinions.Count > 0;

		public override void OnSpawn()
		{
			base.OnSpawn();
			Mod.graves.Add(this);

			if (ClusterManager.Instance.GetWorld(this.GetMyWorldId()) == null)
			{
				Util.KDestroyGameObject(gameObject);
				Log.Warning($"Corpse's world {minionSerializer.Name} is missing.");

				return;
			}

			if (!migrated)
			{
				if (HasStoredLegacyDupe())
					MigrateOldDupe();

				migrated = true;
			}
		}

		private void MigrateOldDupe()
		{
			var minionGo = Revive_Legacy();
			if (minionGo != null)
				minionSerializer.Store(minionGo);
		}

		public void Eject(Action<GameObject> callback)
		{
			minionSerializer.Eject(callback);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.graves.Remove(this);
		}

		public string GetName() => minionSerializer.Name;

		public void OnDupeBuried(GameObject corpse)
		{
			if (minionSerializer.HasObject() || corpse == null)
				return;

			corpse.RemoveTag(GameTags.Dead);
			if (corpse.TryGetComponent(out Health health))
			{
				health.hitPoints = health.maxHitPoints;
			}

			minionSerializer.Store(corpse);
			minionSerializer.SetReason("Dead and buried.");
		}

		[Obsolete]
		private GameObject Revive_Legacy()
		{
			if (!HasStoredLegacyDupe())
			{
				Log.Debug("trying to revive an old format dupe, but there isn't one stored.");
				return null;
			}

			grave.smi.GoTo(grave.smi.sm.empty);

			var revived = minionStorage.DeserializeMinion(minionStorage.serializedMinions[0].id, transform.position);


			return revived;
		}

		public void OnImgui()
		{
			if (!HasDupe() && ImGui.Button("Store random dupe"))
			{
				var target = Components.LiveMinionIdentities.First();
				minionSerializer.Store(target.gameObject);
			}

			if (!HasDupe() && ImGui.Button("Store random dupe LEGACY"))
			{
				var target = Components.LiveMinionIdentities.First();

				migrated = false;
				minionStorage.SerializeMinion(target.gameObject);
			}

			if (HasStoredLegacyDupe() && ImGui.Button("Migrate LEGACY data"))
				MigrateOldDupe();

			if (HasDupe() && ImGui.Button("Delete"))
				minionSerializer.ClearData();
		}

		public void Clear()
		{
			minionSerializer.ClearData();
		}
	}
}
