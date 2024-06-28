using FUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TrueTiles.Datagen
{
	// Massively overcomplicated class to generate me a json file i could have just handwritten. But where would be the fun in that..
	// Used to generate my json before release, and when the user manually "resets" mod data
	// don't patch extra tiles in here from other mods, this is not neccessarily called in every game launch!
	public class DataGen
	{
		protected string path;

		public DataGen(string path)
		{
			this.path = path;
		}

		public static void Write<T>(string path, string filename, T data)
		{
			try
			{
				path = FileUtil.GetOrCreateDirectory(path);
				path = Path.Combine(path, filename + ".json");
				var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore,
					DefaultValueHandling = DefaultValueHandling.Ignore,
					Converters = new List<JsonConverter> { new StringEnumConverter() }
				});

				File.WriteAllText(path, json);
			}
			catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
			{
				Log.Warning("Datagen: Could not write bundle file: " + e.Message);
			}
		}

		public class ElementOverrides
		{
			public string id;
			public Dictionary<string, TileData> items;

			public ElementOverrides(string buildingDef)
			{
				id = buildingDef;
				items = new Dictionary<string, TileData>();
			}

			public ElementOverrides Add(SimHashes element, TileData data)
			{
				items.Add(element.ToString(), data);
				return this;
			}

			public ElementOverrides Add(string element, TileData data)
			{
				items.Add(element, data);
				return this;
			}

			public ElementOverrides AddSimpleTile(string prefix, string element, bool top = true)
			{
				Add(element, new TileDataBuilder()
					.MainTex($"{prefix}_{element.ToLower()}_main", top ? $"{prefix}_{element.ToLower()}_top" : null)
					.Build());

				return this;
			}

			public ElementOverrides AddShinyTile(string prefix, string element, bool specularTop = true, bool top = true)
			{
				Add(element, new TileDataBuilder()
					.MainTex($"{prefix}_{element.ToLower()}_main", top ? $"{prefix}_{element.ToLower()}_top" : null)
					.Specular($"{prefix}_{element.ToLower()}_spec", specularTop ? $"{prefix}_{element.ToLower()}_spec_top" : null)
					.Build());

				return this;
			}
		}

		public class TileDataBuilder
		{
			private string main;
			private string top;
			private string spec;
			private string topSpec;
			private string normalTex;
			private float[] specColor;
			private float[] topSpecColor;
			private float frequency;
			private bool transparent;

			public TileDataBuilder()
			{
			}

			public TileDataBuilder(string prefix, string element, bool top = true)
			{
				MainTex($"{prefix}_{element.ToLower()}_main", top ? $"{prefix}_{element.ToLower()}_top" : null);
			}

			public TileDataBuilder(string prefix, SimHashes element, bool top = true) : this(prefix, element.ToString(), top)
			{
			}

			public TileDataBuilder MainTex(string texture, string top = null)
			{
				main = texture;
				this.top = top;
				return this;
			}

			public TileDataBuilder Specular(string texture, string top = null)
			{
				spec = texture;
				topSpec = top;
				return this;
			}

			public TileDataBuilder SpecularColor(Color color)
			{
				specColor = new float[] { color.r, color.g, color.b, color.a };
				return this;
			}

			public TileDataBuilder TopSpecularColor(Color color)
			{
				topSpecColor = new float[] { color.r, color.g, color.b, color.a };
				return this;
			}

			public TileDataBuilder Frequency(float value)
			{
				frequency = value;
				return this;
			}

			public TileDataBuilder Transparent()
			{
				transparent = true;
				return this;
			}
			public TileDataBuilder Normal(string texture)
			{
				normalTex = texture;
				return this;
			}

			public TileData Build()
			{
				return new TileData()
				{
					MainTex = main,
					TopTex = top,
					MainSpecular = spec,
					TopSpecular = topSpec,
					TopSpecularColor = topSpecColor,
					MainSpecularColor = specColor,
					NormalTex = normalTex,
					Transparent = transparent,
					Frequency = frequency
				};
			}
		}
	}
}