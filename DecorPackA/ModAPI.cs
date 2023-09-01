using DecorPackA.Buildings.MoodLamp;
using DecorPackA.Buildings.StainedGlassTile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA
{
	public class ModAPI
	{
		// methods that are promised to not change signature or disappear, so they are safe to reflect for by other mods

		/// <summary>
		/// Add a tile configuration
		/// </summary>
		/// <param name="elementID">String id of the element (SimHashes.ToString())</param>
		/// <param name="specularColor">Color.white for default</param>
		/// <param name="isSolid">true for solids, false for gases, liquids or specials.</param>
		/// <param name="dlcIds">leave null for all, <see cref="DlcManager.AVAILABLE_EXPANSION1_ONLY"/></param>
		/// <param name="main">Texture of the body of the tiles</param>
		/// <param name="top">top texture</param>
		/// <param name="place">white outline for placing. only the body, the top uses the default glass.</param>
		/// <param name="specular">specular texture. leave null if none.</param>
		public static void AddTile(
			string elementID,
			Color specularColor,
			bool isSolid,
			string[] dlcIds,
			Texture2D main,
			Texture2D top,
			Texture2D place,
			Texture2D specular)
		{
			var info = new StainedGlassTiles.TileInfo(elementID);

			if (specular != null)
				info.SpecColor(specularColor);

			if (!isSolid)
				info.NotSolid();

			if (dlcIds != null)
				info.DLC(dlcIds);

			StainedGlassTiles.tileInfos.Add(info);

			var id = elementID.ToLowerInvariant();

			if (main != null)
				TextureLoader.textureRegistry.Add($"{id}_glass_tiles", main);

			if (top != null)
				TextureLoader.textureRegistry.Add($"{id}_glass_tiles_tops", top);

			if (place != null)
				TextureLoader.textureRegistry.Add($"{id}_glass_tiles_place", place);

			if (specular != null)
				TextureLoader.textureRegistry.Add($"{id}_glass_tiles_spec", specular);
		}

		/// <summary>
		/// Add a new Moodlamp
		/// </summary>
		/// <param name="ID">Unique ID if your lamp</param>
		/// <param name="name">Display name</param>
		/// <param name="kAnimFile">kanim file. expects an "on", "off" and "ui" animations to exist</param>
		/// <param name="color">Light color</param>
		/// <param name="playModeWhenOn"></param>
		/// <param name="components">Types of components to enable on this lamp. For existing components, see below. If you want your custom 
		/// components to serialize don't forget to add them to the moodlamp prefab, with enabled = false by default.</param>
		/// <returns>The lamp variant config. Not part of the database until Db.Initialize post</returns>
		public static LampVariant AddMoodLamp(string ID, string name, string kAnimFile, Color color, KAnim.PlayMode playModeWhenOn, List<Type> components = null)
		{
			var lamp = new LampVariant(ID, name, color.r, color.g, color.b, kAnimFile, playModeWhenOn)
			{
				componentTypes = components
			};

			LampVariants.modAddedMoodlamps ??= new();
			LampVariants.modAddedMoodlamps.Add(lamp);

			return lamp;
		}

		/// <summary>
		/// Enable the rainbow lights components on this moodlamp
		/// </summary>
		public static void RainbowLight(string ID) => AddComponentInternal<GlitterLight2D>(ID);

		/// <summary>
		/// This lamp will shift back and forth between the 2 colors given
		/// </summary>
		/// <param name="color2">Second color</param>
		/// <param name="shiftDurationSeconds"></param>
		public static void AddShiftyMoodLamp(string ID, Color color2, float shiftDurationSeconds)
		{
			var lamp = AddComponentInternal<ShiftyLight2D>(ID);
			lamp.color2 = color2;
			lamp.shiftDuration = shiftDurationSeconds;
		}

		/// <summary>
		/// Add a particle system to this lamp while turned on
		/// </summary>
		/// <param name="particles">A prefab to appear</param>
		public static LampVariant ScatterLight(string ID, GameObject particles) 
		{
			var lamp = LampVariants.modAddedMoodlamps.Find(l => l.Id == ID);
			if (lamp == null)
				Log.Warning($"No lamp with ID {ID}. Register the lamp before trying to add components to is.");

			lamp.ToggleComponent<ScatterLightLamp>();
			ModAssets.Prefabs.scatterLampPrefabs[ID] = particles;

			return lamp;
		}

		/// <summary>
		/// Just a helper
		/// </summary>
		internal static LampVariant AddComponentInternal<T>(string ID) where T : KMonoBehaviour
		{
			var lamp = LampVariants.modAddedMoodlamps.Find(l => l.Id == ID);
			if (lamp == null)
				Log.Warning($"No lamp with ID {ID}. Register the lamp before trying to add components to is.");

			lamp.ToggleComponent<T>();

			return lamp;
		}
	}
}
