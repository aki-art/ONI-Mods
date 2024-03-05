using FUtility;
using ImGuiNET;
using KSerialization;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class AETE_GraveStoneMinionStorage : KMonoBehaviour, IImguiDebug
	{
		[Obsolete][MyCmpGet] private MinionStorage minionStorage;
		[MyCmpGet] private Grave grave;

		[Serialize] public string minionName;
		[Serialize] public bool migrated;

		[Serialize] public byte[] minionData;

		public bool HasDupe() => minionData != null;

		public bool HasStoredLegacyDupe() => !migrated && minionStorage.serializedMinions != null && minionStorage.serializedMinions.Count > 0;

		public AETE_GraveStoneMinionStorage()
		{
			minionData = null;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			Mod.graves.Add(this);

			if (ClusterManager.Instance.GetWorld(this.GetMyWorldId()) == null)
			{
				Util.KDestroyGameObject(gameObject);
				Log.Warning($"Corpse's world {minionName} is missing.");

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
				Store(minionGo);
		}

		public GameObject Eject()
		{
			if (minionData == null)
			{
				Log.Warning($"{nameof(AETE_GraveStoneMinionStorage)} Trying to deserialize minion but it is null.");
				return null;
			}

			var reader = new FastReader(minionData);
			var result = SaveLoadRoot.Load(MinionConfig.ID, reader);

			minionData = null;

			if (result != null)
				result.transform.SetPosition(this.transform.position);

			return result?.gameObject;
		}

		public void Store(GameObject identity)
		{
			SaveData(identity.GetComponent<SaveLoadRoot>());
			Util.KDestroyGameObject(identity.gameObject);
		}

		public void SaveData(SaveLoadRoot root)
		{
			using MemoryStream stream = new();

			using BinaryWriter writer = new(stream);
			{
				root.Save(writer);
			}

			stream.Flush();

			minionData = stream.ToArray();
		}


		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.graves.Remove(this);
		}

		public string GetName() => minionName;

		public void OnDupeBuried(GameObject corpse)
		{
			if (HasDupe())
				return;

			minionName = corpse.name;
			Store(corpse);
		}

		[Obsolete]
		private GameObject Revive_Legacy()
		{
			if (!HasStoredLegacyDupe())
			{
				Log.Debug("trying to revive an old format dupe, but there isn't one stored.");
				return null;
			}

			minionName = null;

			grave.smi.GoTo(grave.smi.sm.empty);

			var revived = minionStorage.DeserializeMinion(minionStorage.serializedMinions[0].id, transform.position);


			return revived;
		}

		public void ClearData()
		{
			minionData = null;
		}

		public void OnImgui()
		{
			if (!HasDupe() && ImGui.Button("Store random dupe"))
			{
				var target = Components.LiveMinionIdentities.First();
				Store(target.gameObject);
			}

			if (!HasDupe() && ImGui.Button("Store random dupe LEGACY"))
			{
				var target = Components.LiveMinionIdentities.First();

				migrated = false;
				minionName = target.name;
				minionStorage.SerializeMinion(target.gameObject);
			}

			if (HasStoredLegacyDupe() && ImGui.Button("Migrate LEGACY data"))
				MigrateOldDupe();

			if (HasDupe() && ImGui.Button("Eject"))
				Eject();

			if (HasDupe() && ImGui.Button("Delete"))
				ClearData();
		}
	}
}
