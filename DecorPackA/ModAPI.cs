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
		/// <param name="uiName">Override for the tooltip in the lamp selector sidescreen, leave null if none.</param>
		/// <param name="kAnimFile">kanim file. expects an "on", "off" and "ui" animations to exist</param>
		/// <param name="color">Light color</param>
		/// <param name="playModeWhenOn"></param>
		/// <param name="components">Types of components to enable on this lamp. For existing components, see below. If you want your custom 
		/// components to serialize don't forget to add them to the moodlamp prefab, with enabled = false by default.</param>
		/// <returns>The lamp variant config. Not part of the database until Db.Initialize post</returns>
		public static object AddMoodLamp(string ID, string name, string uiName, string category, string kAnimFile, Color color, KAnim.PlayMode playModeWhenOn, HashSet<HashedString> tags = null)
		{
			category ??= LampVariants.MISC;

			var lamp = new LampVariant(ID, name, color.r, color.g, color.b, category, kAnimFile, playModeWhenOn, tags: tags)
			{
				uiName = uiName
			};

			LampVariants.modAddedMoodlamps ??= [];
			LampVariants.modAddedMoodlamps.Add(lamp);

			return lamp;
		}

		[Obsolete("Use the one with uiName instead.")]
		public static object AddMoodLamp(string ID, string name, string category, string kAnimFile, Color color, KAnim.PlayMode playModeWhenOn, HashSet<HashedString> tags = null)
		{
			return AddMoodLamp(ID, name, null, category, kAnimFile, color, playModeWhenOn, tags);
		}

		public static void AddComponentToMoodlampPrefab(Type type)
		{
			Log.Debug("adding component type");
			MoodLampConfig.RegisterType(type);
		}
	}
}
