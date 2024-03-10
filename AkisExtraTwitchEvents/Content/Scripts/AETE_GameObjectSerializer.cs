using HarmonyLib;
using ImGuiNET;
using KSerialization;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	/** Saves a game object as a byte array. This is similar to <see cref="MinionStorage"/> but 
	 * it isn't coupled with UI and it's data cannot be easily accessed before unpacking. **/
	public class AETE_GameObjectSerializer : KMonoBehaviour, IOnModCleanup, IImguiDebug
	{
		[SerializeField] public bool showInUIs;
		[SerializeField] public bool checkForSafeLocation;
		[SerializeField] public bool saveToFile;

		[Serialize] public byte[] data;
		[Serialize] public string objectName;
		[Serialize] public string uiSprite;
		[Serialize] public string uiSerializedReason;
		[Serialize] public string fileIdentifier;

		private bool deserializeReady;
		private GameObject ejectedGameObject;

		public Sprite UISprite { private set; get; }

		public bool HasObject() => data != null && data.Length > 0;

		public string Name => objectName;

		public AETE_GameObjectSerializer()
		{
			data = null;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (uiSprite != null)
			{
				UISprite = Assets.GetSprite(uiSprite);
				if (UISprite == null)
					uiSprite = null;
			}
		}

		private IEnumerator WaitForUnpause()
		{
			while (Game.Instance.IsPaused)
				yield return new WaitForEndOfFrame();

			deserializeReady = true;
		}

		private IEnumerator EjectOnUnpause(Action<GameObject> callback)
		{
			while (Game.Instance.IsPaused)
				yield return new WaitForEndOfFrame();

			var result = Eject_Internal();
			callback?.Invoke(result);
		}

		public void Eject(Action<GameObject> callback)
		{
			StartCoroutine(EjectOnUnpause(callback));
		}

		private GameObject Eject_Internal()
		{
			if (data == null)
			{
				Log.Warning($"{nameof(AETE_GameObjectSerializer)} Trying to deserialize gameobject but it is null.");
				return null;
			}

			foreach (var tempkate in Manager.deserializationTemplatesByType)
			{
				Log.Debug($"{tempkate.Key.Name}: {tempkate.Value.serializedMembers.Join(m => m.name)}");
			}
			var reader = new FastReader(data);
			var result = SaveLoadRoot.Load(MinionConfig.ID, reader);


			if (result != null)
			{
				var position = transform.position;
				if (checkForSafeLocation && !IsLocationSafe(position))
				{
					position = AGridUtil.GetSafeLocation();
					// notify
				}

				result.transform.SetPosition(position);
			}
			else Log.Warning("Could not deserialize object. :(");

			ClearData();

			return result?.gameObject;
		}

		private bool IsLocationSafe(Vector3 position)
		{
			var cell = Grid.PosToCell(position);

			if (!Grid.IsValidCell(cell))
				return false;

			if (Grid.WorldIdx[cell] == byte.MaxValue)
				return false;

			if (!ONITwitchLib.Utils.GridUtil.IsCellEmpty(cell))
				return false;

			if (Grid.Temperature[cell] > 573.15f) // 300C
				return false;

			return true;
		}

		private string GetBackupFolderPath() => Path.Combine(
			FUtility.Utils.ConfigPath(Mod.ID),
			"backups",
			SaveLoader.Instance.GameInfo.colonyGuid.ToString());

		private string GetBackupFilePath() => Path.Combine(
			GetBackupFolderPath(),
			fileIdentifier.ToString());

		public void ClearData()
		{
			if (!fileIdentifier.IsNullOrWhiteSpace())
			{
				var path = GetBackupFilePath();

				if (File.Exists(path))
					File.Delete(path);

				string folder = GetBackupFolderPath();
				if (Directory.GetFiles(folder).Length == 0)
					Directory.Delete(folder, false);

				Log.Info($"Removed cached data for {objectName} from {path}.");

				fileIdentifier = null;
			}

			data = null;
			objectName = null;
			uiSprite = null;
			uiSerializedReason = null;
			showInUIs = false;
		}

		public void Store(GameObject identity)
		{
			deserializeReady = false;
			objectName = identity.name;
			SaveData(identity.GetComponent<SaveLoadRoot>());
			Util.KDestroyGameObject(identity.gameObject);
			StartCoroutine(WaitForUnpause());

			if (saveToFile)
			{
				fileIdentifier = Guid.NewGuid().ToString();
				SaveToFile();
			}
		}

		public void SaveData(SaveLoadRoot root)
		{
			using MemoryStream stream = new();
			using BinaryWriter writer = new(stream);

			root.Save(writer);
			stream.Flush();
			data = stream.ToArray();
		}

		public void SetReason(string str)
		{
			uiSerializedReason = str;
		}

		public void SetSprite(Sprite sprite)
		{
			uiSprite = sprite.name;
			UISprite = sprite;
		}

		public void SaveToFile()
		{
			var saveData = new SaveFileData()
			{
				colonyIdentifier = SaveLoader.Instance.GameInfo.colonyGuid,
				data = data,
				reasonOfSave = uiSerializedReason
			};

			var path = GetBackupFolderPath();

			try
			{
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
				string filePath = GetBackupFilePath();

				File.WriteAllText(filePath, json);

				Log.Info($"Saved backup data of {Name} to {filePath}. If for any reason the mod cannot restore this entity, you can use the mod settings panel while in game to restore it manually. The in-engine serialized data will have priority, this is only a failsafe.");
			}
			catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
			{
				Log.Warning("Could not write backup file: " + e.Message);
			}
		}

		public void OnModCleanup()
		{
			if (HasObject())
				Eject(null);
		}

		public void OnImgui()
		{
			ImGui.Text("Currently stored: " + Name ?? "Nothing");

			if (HasObject())
			{
				if (deserializeReady)
				{
					if (ImGui.Button($"Eject˝{Name}"))
						Eject(null);
				}
				else
				{
					var postfix = Game.Instance.IsPaused ? " (Game Paused)" : "";
					ImGui.Text($"Waiting on serialization{postfix}");
				}
			}

			if (!HasObject() && ImGui.Button("Store Random dupe"))
			{
				var target = Components.LiveMinionIdentities.GetRandom();
				if (target != null)
					Store(target.gameObject);
			}
		}

		private class SaveFileData
		{
			public byte[] data;
			public Guid colonyIdentifier;
			public string reasonOfSave;
		}
	}
}
