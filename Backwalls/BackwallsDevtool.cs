#if DEVTOOLS
using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Backwalls
{
	public class BackwallsDevtool : DevTool
	{
		private static float z = Grid.GetLayerZ(Grid.SceneLayer.Backwall);
		private static int renderQueue = 3000;
		private static bool zWrite = true;

		public override void RenderTo(DevPanel panel)
		{
			ImGui.DragFloat("Z layer", ref z);
			ImGui.DragInt("Render Queue", ref renderQueue);
			ImGui.Checkbox("ZWrite", ref zWrite);

			if (ImGui.Button("Apply"))
			{
				Mod.renderer.DebugForceRebuild(z, renderQueue, zWrite ? 1 : 0);
			}

			ImGui.Spacing();
			ImGui.Text("DefaultPattern :" + Mod.Settings.DefaultPattern);
			ImGui.TextColored(Util.ColorFromHex(Mod.Settings.DefaultColor), "DefaultColor :" + Mod.Settings.DefaultColor);

			ImGui.Spacing();

			if (ImGui.Button("Export basic atlas data"))
			{
				TextureAtlas atlas = Assets.GetTextureAtlas("tiles_solid");
				var serialized = new SerializableAtlas()
				{
					scaleFactor = atlas.scaleFactor,
					items = new List<Item>()
				};

				foreach (var item in atlas.items)
				{
					serialized.items.Add(new Item()
					{
						name = item.name,
						uvBox = item.uvBox,
						vertices = item.vertices.Select(v => new SerializableVector(v.x, v.y, v.z)).ToArray(),
						uvs = item.uvs.Select(v => new SerializableVector(v.x, v.y)).ToArray(),
					});
				}

				var json = JsonConvert.SerializeObject(serialized);
				File.WriteAllText(Path.Combine(Utils.ModPath, "atlas.json"), json);

				Application.OpenURL(Utils.ModPath);
			}
		}

		[Serializable]
		public class SerializableAtlas
		{
			public List<Item> items;
			public float scaleFactor;
		}

		[Serializable]
		public struct Item
		{
			public string name;

			public SerializableVector uvBox;

			public SerializableVector[] vertices;

			public SerializableVector[] uvs;

			public int[] indices;
		}

		[Serializable]
		public struct SerializableVector(float x, float y, float z = 0, float w = 0)
		{
			public float x = x;
			public float y = y;
			public float z = z;
			public float w = w;

			public static implicit operator SerializableVector(Vector2 v) => new(v.x, v.y);

			public static implicit operator SerializableVector(Vector3 v) => new(v.x, v.y, v.z);

			public static implicit operator SerializableVector(Vector4 v) => new(v.x, v.y, v.z, v.w);
		}
	}
}
#endif