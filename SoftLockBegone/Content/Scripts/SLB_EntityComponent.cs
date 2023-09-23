using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using YamlDotNet.Serialization;

namespace SoftLockBegone.Content.Scripts
{
	public class SLB_EntityComponent : KMonoBehaviour, ISidescreenButtonControl
	{
		[MyCmpGet] private PrimaryElement primaryElement;
		[MyCmpGet] private KSelectable kSelectable;

		[Serialize] public string originalPrefabTag;
		[Serialize] public byte[] data;
		[Serialize] public int capacity;

		public string SidescreenButtonText => "Recycle";

		public string SidescreenButtonTooltip => "Drop recovered materials on floor";

		public int ButtonSideScreenSortOrder() => 0;

		public int HorizontalGroupID() => -1;

		public void SetData(string prefabId, byte[] data, int capacity)
		{
			this.data = data;
			this.capacity = capacity;
			originalPrefabTag = prefabId;

			kSelectable.SetName(prefabId);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (originalPrefabTag == null)
				return;

			kSelectable.SetName(originalPrefabTag);
		}

		[OnDeserialized]
		public void OnDeserialized()
		{
			TryTransferring();
		}

		public void TryTransferring()
		{
			var entity = Assets.TryGetPrefab(originalPrefabTag);
			if (entity != null)
			{
				Log.Info($"Restoring entity of ID {originalPrefabTag}");

				if (data != null)
				{
					Log.Debuglog("\t has data");
					Load(entity, new FastReader(data));
				}

				Util.KDestroyGameObject(this);
			}
		}

		public void OnSidescreenButtonPressed()
		{
			Recycle(true);
		}

		private void Recycle(bool dropElement)
		{
			if (dropElement)
			{
				var elements = /*deconstructable != null ? deconstructable.constructionElements : */ new Tag[] { primaryElement.Element.tag };

				foreach (var element in elements)
				{
					var item = Utils.Spawn(element, gameObject);
					item.GetComponent<PrimaryElement>().Mass = primaryElement.Mass;
				}
			}

			foreach (var storage in GetComponents<Storage>())
			{
				storage.DropAll();
			}

			Util.KDestroyGameObject(this);
		}

		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
		}

		public bool SidescreenButtonInteractable() => true;

		public bool SidescreenEnabled() => primaryElement.ElementID != SimHashes.Creature;

		// Massive copy paste job, all so mod components are ignored and don't crash the game
		public static SaveLoadRoot Load(GameObject prefab, IReader reader)
		{
			var position = reader.ReadVector3();

			var rotation = reader.ReadQuaternion();
			var scale = reader.ReadVector3();
			Log.Debuglog("\tposition: " + position);
			Log.Debuglog("\trotation: " + rotation);
			Log.Debuglog("\tscale: " + scale);

			reader.ReadByte();

			return Load(prefab, position, rotation, scale, reader);
		}

		public static SaveLoadRoot Load(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale, IReader reader)
		{
			SaveLoadRoot saveLoadRoot = null;
			if (prefab != null)
			{
				var gameObject = Util.KInstantiate(prefab, position, rotation, null, null, initialize_id: false);
				gameObject.transform.localScale = scale;
				gameObject.SetActive(true);

				saveLoadRoot = gameObject.GetComponent<SaveLoadRoot>();
				if (saveLoadRoot != null)
				{
					try
					{
						LoadInternal(gameObject, reader);
					}
					catch (ArgumentException ex)
					{
						DebugUtil.LogErrorArgs(gameObject, "Failed to load SaveLoadRoot ", ex.Message, "\n", ex.StackTrace);
					}
				}
				else
				{
					Debug.Log("missing SaveLoadRoot", gameObject);
				}
			}
			else
			{
				LoadInternal(null, reader);
			}

			return saveLoadRoot;
		}

		public static void LoadInternal(GameObject gameObject, IReader reader)
		{
			var dictionary = new Dictionary<string, int>();
			KMonoBehaviour[] array = ((gameObject != null) ? gameObject.GetComponents<KMonoBehaviour>() : null);
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string text = reader.ReadKleiString();
				int num2 = reader.ReadInt32();
				int position = reader.Position;
				if (SaveLoadRoot.serializableComponentManagers.TryGetValue(text, out var value))
				{
					value.Deserialize(gameObject, reader);
					continue;
				}

				dictionary.TryGetValue(text, out int value2);
				KMonoBehaviour kMonoBehaviour = null;
				int num3 = 0;
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						Type type = array[j].GetType();
						if (!SaveLoadRoot.sTypeToString.TryGetValue(type, out var value3))
						{
							value3 = type.ToString();
							SaveLoadRoot.sTypeToString[type] = value3;
						}

						if (value3 == text)
						{
							if (num3 == value2)
							{
								kMonoBehaviour = array[j];
								break;
							}

							num3++;
						}
					}
				}

				if (kMonoBehaviour == null && gameObject != null)
				{
					SaveLoadRoot component = gameObject.GetComponent<SaveLoadRoot>();
					int index;
					if (component != null && (index = component.m_optionalComponentTypeNames.IndexOf(text)) != -1)
					{
						bool test = value2 == 0 && num3 == 0;
						DebugUtil.DevAssert(test, $"Implementation does not support multiple components with optional components, type {text}, {value2}, {num3}. Using only the first one and skipping the rest.");
						Type type2 = Type.GetType(component.m_optionalComponentTypeNames[index]);
						if (num3 == 0)
						{
							kMonoBehaviour = (KMonoBehaviour)gameObject.AddComponent(type2);
						}
					}
				}

				if (kMonoBehaviour == null)
				{
					reader.SkipBytes(num2);
					continue;
				}

				if ((object)kMonoBehaviour == null && !(kMonoBehaviour is ISaveLoadableDetails))
				{
					DebugUtil.LogErrorArgs("Component", text, "is not ISaveLoadable");
					reader.SkipBytes(num2);
					continue;
				}

				dictionary[text] = num3 + 1;

				if (kMonoBehaviour is ISaveLoadableDetails)
				{
					var saveLoadableDetails = (ISaveLoadableDetails)kMonoBehaviour;
					DeserializeTypeless(kMonoBehaviour, reader);
					saveLoadableDetails.Deserialize(reader);
				}
				else
				{
					DeserializeTypeless(kMonoBehaviour, reader);
				}

				if (reader.Position != position + num2)
				{
					DebugUtil.LogWarningArgs("Expected to be at offset", position + num2, "but was only at offset", reader.Position, ". Skipping to catch up.");
					reader.SkipBytes(position + num2 - reader.Position);
				}
			}
		}


		public static bool DeserializeTypeless(object obj, IReader reader)
		{
			var type = obj.GetType();
			var deserializationMapping = GetDeserializationMapping(type);

			if (deserializationMapping == null)
			{
				(reader as FastReader).SkipBytes(1);
				return false;
			}

			try
			{
				return deserializationMapping.Deserialize(obj, reader);
			}
			catch (Exception ex)
			{
				string text = $"Exception occurred while attempting to deserialize object {obj}({obj.GetType()}).\n{ex.ToString()}";
				Log.Error( text);
				throw new Exception(text, ex);
			}
		}


		public static DeserializationMapping GetDeserializationMapping(Type type)
		{
			var deserializationTemplate = Manager.GetDeserializationTemplate(type);
			if (deserializationTemplate == null)
			{
				Log.Debuglog($"Deserialization template is null for {type.FullName}");
				return null;
			}

			var serializationTemplate = Manager.GetSerializationTemplate(type);
			if (serializationTemplate == null)
				return null;

			return Manager.GetMapping(deserializationTemplate, serializationTemplate);
		}

	}
}
