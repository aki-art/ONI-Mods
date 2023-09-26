using Backwalls.Buildings;
using Rendering;
using System;
using System.Collections.Generic;
using UnityEngine;
using Bits = Rendering.BlockTileRenderer.Bits;

namespace Backwalls.Cmps
{
	// mostly based on the game's own Tile Renderer
	public class BackwallRenderer : MonoBehaviour
	{
		[SerializeField] public bool forceRebuild;

		private readonly Dictionary<string, RenderInfo> renderInfos = new();

		public readonly Dictionary<int, Color> colorInfos = new();
		//public readonly HashSet<int> shinyDisabled = new();
		//public readonly Dictionary<int, bool> forceNoBorder = new();
		public static readonly Dictionary<string, Material> shaderOverrides = new();

		private int selectedCell = -1;
		private int highlightCell = -1;

		private Color highlightColour = new(1.25f, 1.25f, 1.25f, 1f);
		private Color selectColour = new(1.5f, 1.5f, 1.5f, 1f);

		public static Material shinyMaterial;
		public static Material material;

		private void Start()
		{
			material = new Material(Shader.Find("TextMeshPro/Sprite"))
			{
				renderQueue = RenderQueues.Liquid
			};

			shinyMaterial = new Material(ModAssets.shinyTileMaterial)
			{
				renderQueue = RenderQueues.Liquid
			};
		}

		public void LateUpdate() => Render();

		public void DebugForceRebuild(float z, int renderQueue, int zWrite)
		{
			material.renderQueue = renderQueue;
			material.SetInt("_ZWrite", zWrite);

			foreach (var renderInfo in renderInfos)
			{
				renderInfo.Value.SetZ(z);
			}

			forceRebuild = true;
		}

		private void Render()
		{
			Vector2I bottomLeft;
			Vector2I topRight;

			if (GameUtil.IsCapturingTimeLapse())
			{
				bottomLeft = new Vector2I(0, 0);
				topRight = new Vector2I(Grid.WidthInCells / 16, Grid.HeightInCells / 16);
			}
			else
			{
				var visibleArea = GridVisibleArea.GetVisibleArea();
				bottomLeft = new Vector2I(visibleArea.Min.x / 16, visibleArea.Min.y / 16);
				topRight = new Vector2I((visibleArea.Max.x + 16 - 1) / 16, (visibleArea.Max.y + 16 - 1) / 16);
			}

			foreach (var renderInfo in renderInfos)
			{
				var info = renderInfo.Value;
				for (var y = bottomLeft.y; y < topRight.y; y++)
				{
					for (var x = bottomLeft.x; x < topRight.x; x++)
					{
						info.Rebuild(this, x, y, MeshUtil.vertices, MeshUtil.uvs, MeshUtil.indices, MeshUtil.colours);
						info.Render(x, y);
					}
				}
			}

			forceRebuild = false;
		}

		public void AddBlock(int renderLayer, BackwallPattern def, int cell, bool shiny)
		{
			var id = shiny ? def.HashedIdShiny : def.HashedId;

			if (!renderInfos.TryGetValue(id, out var renderInfo))
			{
				Log.Debug($" ############# Adding renderinfo with id {id} at {cell}");
				var mat = GetMaterialForDef(def, shiny);
				renderInfo = new RenderInfo(this, cell, renderLayer, def, mat);
				renderInfos[id] = renderInfo;
			}

			Log.Debug($"renderinfo with id {id} existed {cell}");
			renderInfo.AddCell(cell);
		}

		private static Material GetMaterialForDef(BackwallPattern def, bool shiny)
		{
			if (shaderOverrides.TryGetValue(def.ID, out var overrideMaterial))
				return overrideMaterial;

			if (!Mod.Settings.EnableShinyTilesGlobal || !shiny)
				return material;

			return def.uniqueMaterial ?? (def.specularTexture != null ? shinyMaterial : material);
		}

		public void RemoveBlock(BackwallPattern pattern, int cell)
		{
			RemoveCell(cell, pattern.HashedId);
			RemoveCell(cell, pattern.HashedIdShiny);
		}

		private void RemoveCell(int cell, string id)
		{
			if (renderInfos.TryGetValue(id, out var renderInfo))
				renderInfo.RemoveCell(cell);
		}

		[Flags]
		public enum Properties
		{
			None = 0,
			Shiny = 1,
			Borderless = 2,

			All = Shiny | Borderless
		}

		private static bool MatchesDef(GameObject go, Backwall wall)
		{
			if (go == null || wall == null)
			{
				return false;
			}

			var wall2 = go.GetComponent<Backwall>();

			if (wall2 == null)
			{
				return false;
			}

			return wall.Matches(wall2);
		}

		public virtual Bits GetConnectionBits(int x, int y, int query_layer)
		{
			Bits bits = 0;
			var gameObject = Grid.Objects[y * Grid.WidthInCells + x, query_layer];
			var wall = gameObject.GetComponent<Backwall>();

			if (wall == null)
			{
				return 0;
			}

			if (y > 0)
			{
				var cell = (y - 1) * Grid.WidthInCells + x;
				if (x > 0 && MatchesDef(Grid.Objects[cell - 1, query_layer], wall))
				{
					bits |= Bits.DownLeft;
				}
				if (MatchesDef(Grid.Objects[cell, query_layer], wall))
				{
					bits |= Bits.Down;
				}
				if (x < Grid.WidthInCells - 1 && MatchesDef(Grid.Objects[cell + 1, query_layer], wall))
				{
					bits |= Bits.DownRight;
				}
			}

			var num2 = y * Grid.WidthInCells + x;

			if (x > 0 && MatchesDef(Grid.Objects[num2 - 1, query_layer], wall))
			{
				bits |= Bits.Left;
			}
			if (x < Grid.WidthInCells - 1 && MatchesDef(Grid.Objects[num2 + 1, query_layer], wall))
			{
				bits |= Bits.Right;
			}
			if (y < Grid.HeightInCells - 1)
			{
				var num3 = (y + 1) * Grid.WidthInCells + x;
				if (x > 0 && MatchesDef(Grid.Objects[num3 - 1, query_layer], wall))
				{
					bits |= Bits.UpLeft;
				}
				if (MatchesDef(Grid.Objects[num3, query_layer], wall))
				{
					bits |= Bits.Up;
				}
				if (x < Grid.WidthInCells + 1 && MatchesDef(Grid.Objects[num3 + 1, query_layer], wall))
				{
					bits |= Bits.UpRight;
				}
			}

			return bits;
		}

		public void SelectCell(int cell, bool enabled)
		{
			UpdateCellStatus(ref selectedCell, cell, enabled);
		}

		public void HighlightCell(int cell, bool enabled)
		{
			UpdateCellStatus(ref highlightCell, cell, enabled);
		}

		private void UpdateCellStatus(ref int cellStatus, int cell, bool enabled)
		{
			if (enabled)
			{
				if (cell == cellStatus)
				{
					return;
				}

				if (cellStatus != -1)
				{
					foreach (var info in renderInfos)
					{
						info.Value.MarkDirtyIfOccupied(cellStatus);
					}
				}

				cellStatus = cell;

				foreach (var info in renderInfos)
				{
					info.Value.MarkDirtyIfOccupied(cellStatus);
				}

				return;
			}

			if (cellStatus == cell)
			{
				foreach (var keyValuePair3 in renderInfos)
				{
					keyValuePair3.Value.MarkDirty(cellStatus);
				}

				cellStatus = -1;
			}

			forceRebuild = true;
		}

		public void Rebuild(int cell)
		{
			foreach (var info in renderInfos)
			{
				info.Value.MarkDirty(cell);
			}
		}

		private Color GetCellColour(int num, float biomeTint)
		{
			var baseColor = Color.white;

			if (colorInfos.ContainsKey(num))
			{
				baseColor = colorInfos[num];
			}

			if (biomeTint == 0)
			{
				return baseColor;
			}

			var zoneType = World.Instance.zoneRenderData.GetSubWorldZoneType(num);
			var zoneColor = World.Instance.zoneRenderData.zoneColours[(int)zoneType];
			Color color = new Color32(zoneColor.r, zoneColor.g, zoneColor.b, 255);

			color = Color.Lerp(baseColor, color, biomeTint);
			color.a = baseColor.a;

			var selectionColor = num == selectedCell ? selectColour : num == highlightCell ? highlightColour : Color.white;
			return selectionColor * color;
		}

		private class RenderInfo
		{
			private int queryLayer = (int)ObjectLayer.Backwall;
			private int renderLayer;

			private Vector3 rootPosition;
			private MaterialPropertyBlock materialProperties;

			private float zOffset;

			private Mesh[,] meshChunks;
			private bool[,] dirtyChunks;

			private Vector2 trimUVSize = new Vector2(0.03125f, 0.03125f);

			private AtlasInfo[] atlasInfo;

			private HashSet<int> occupiedCells = new HashSet<int>();

			private float biomeTint = 0.2f;
			private bool shiny;
			private Color specularColor;

			private Material material;

			public void SetZ(float z)
			{
				rootPosition.z = z;
			}

			public RenderInfo(BackwallRenderer backwallRenderer, int cell, int renderLayer, BackwallPattern variant, Material material)
			{
				this.renderLayer = renderLayer;

				this.material = material;

				biomeTint = variant.biomeTint;
				zOffset = Mod.z;// Grid.GetLayerZ(Mod.sceneLayer) - 0.1f;

				Log.Debug("z: " + zOffset);

				//zOffset += 0.000001f * cell; // some order to rendering. sometimes won't be right because of float rounding, but generally helps

				rootPosition = new Vector3(0f, 0f, zOffset);

				materialProperties = new MaterialPropertyBlock();
				materialProperties.SetTexture("_MainTex", variant.atlas.texture);

				if (variant.specularTexture != null)
				{
					shiny = true;
					materialProperties.SetTexture("_SpecularTex", variant.specularTexture);
					//bool isDulled = Mod.renderer.shinyDisabled.Contains(cell);
					var color = variant.specularColor;
					if (Mod.Settings.Shiny == Settings.Config.ShinySetting.Dull)
						color *= 0.5f;

					materialProperties.SetColor("_ShineColour", color);
				}

				var x = Grid.WidthInCells / 16 + 1;
				var y = Grid.HeightInCells / 16 + 1;

				meshChunks = new Mesh[x, y];
				dirtyChunks = new bool[x, y];

				for (var i = 0; i < y; i++)
				{
					for (var j = 0; j < x; j++)
					{
						dirtyChunks[j, i] = true;
					}
				}

				var num3 = variant.atlas.items[0].name.Length - 4 - 8;
				var startIndex = num3 - 1 - 8;
				atlasInfo = new AtlasInfo[variant.atlas.items.Length];

				for (var k = 0; k < atlasInfo.Length; k++)
				{
					var item = variant.atlas.items[k];

					var value = item.name.Substring(startIndex, 8);
					var value2 = item.name.Substring(num3, 8);

					var requiredConnections = Convert.ToInt32(value, 2);
					var forbiddenConnections = Convert.ToInt32(value2, 2);

					atlasInfo[k].requiredConnections = (Bits)requiredConnections;
					atlasInfo[k].forbiddenConnections = (Bits)forbiddenConnections;

					atlasInfo[k].uvBox = item.uvBox;
					atlasInfo[k].name = item.name;
				}
			}

			public void MarkDirtyIfOccupied(int cell)
			{
				if (occupiedCells.Contains(cell))
				{
					MarkDirty(cell);
				}
			}

			public void AddCell(int cell)
			{
				occupiedCells.Add(cell);
				MarkDirty(cell);
			}

			public void MarkDirty(int cell)
			{
				var chunkIdx = BlockTileRenderer.GetChunkIdx(cell);
				dirtyChunks[chunkIdx.x, chunkIdx.y] = true;
			}

			public void RemoveCell(int cell)
			{
				occupiedCells.Remove(cell);
				MarkDirty(cell);
			}

			internal void Rebuild(BackwallRenderer renderer, int chunkX, int chunkY, List<Vector3> vertices, List<Vector2> uvs, List<int> indices, List<Color> colors)
			{
				if (!dirtyChunks[chunkX, chunkY] && !renderer.forceRebuild)
				{
					return;
				}

				dirtyChunks[chunkX, chunkY] = false;

				vertices.Clear();
				uvs.Clear();
				indices.Clear();
				colors.Clear();

				for (var x = chunkY * 16; x < chunkY * 16 + 16; x++)
				{
					for (var y = chunkX * 16; y < chunkX * 16 + 16; y++)
					{
						var cell = x * Grid.WidthInCells + y;
						if (occupiedCells.Contains(cell))
						{
							var connectionBits = renderer.GetConnectionBits(y, x, queryLayer);
							for (var k = 0; k < atlasInfo.Length; k++)
							{
								var flag = (atlasInfo[k].requiredConnections & connectionBits) == atlasInfo[k].requiredConnections;
								var flag2 = (atlasInfo[k].forbiddenConnections & connectionBits) > 0;

								if (flag && !flag2)
								{
									var cellColour = renderer.GetCellColour(cell, biomeTint);
									AddVertexInfo(atlasInfo[k], trimUVSize, y, x, connectionBits, cellColour, vertices, uvs, indices, colors);

									break;
								}
							}
						}
					}
				}

				var mesh = meshChunks[chunkX, chunkY];

				if (vertices.Count > 0)
				{
					if (mesh == null)
					{
						mesh = new Mesh
						{
							name = "BackWall"
						};

						meshChunks[chunkX, chunkY] = mesh;
					}

					mesh.Clear();
					mesh.SetVertices(vertices);
					mesh.SetUVs(0, uvs);
					mesh.SetColors(colors);
					mesh.SetTriangles(indices, 0);
				}
				else if (mesh != null)
				{
					meshChunks[chunkX, chunkY] = null;
				}
			}

			private void AddVertexInfo(AtlasInfo atlas_info, Vector2 uv_trim_size, int x, int y, Bits connection_bits, Color color, List<Vector3> vertices, List<Vector2> uvs, List<int> indices, List<Color> colours)
			{
				var vector = new Vector2(x, y);
				var vector2 = vector + new Vector2(1f, 1f);
				var vector3 = new Vector2(atlas_info.uvBox.x, atlas_info.uvBox.w);
				var vector4 = new Vector2(atlas_info.uvBox.z, atlas_info.uvBox.y);

				if ((connection_bits & Bits.Left) == 0)
				{
					vector.x -= 0.25f;
				}
				else
				{
					vector3.x += uv_trim_size.x;
				}
				if ((connection_bits & Bits.Right) == 0)
				{
					vector2.x += 0.25f;
				}
				else
				{
					vector4.x -= uv_trim_size.x;
				}
				if ((connection_bits & Bits.Up) == 0)
				{
					vector2.y += 0.25f;
				}
				else
				{
					vector4.y -= uv_trim_size.y;
				}
				if ((connection_bits & Bits.Down) == 0)
				{
					vector.y -= 0.25f;
				}
				else
				{
					vector3.y += uv_trim_size.y;
				}

				var count = vertices.Count;

				vertices.Add(vector);
				vertices.Add(new Vector2(vector2.x, vector.y));
				vertices.Add(vector2);
				vertices.Add(new Vector2(vector.x, vector2.y));

				uvs.Add(vector3);
				uvs.Add(new Vector2(vector4.x, vector3.y));
				uvs.Add(vector4);
				uvs.Add(new Vector2(vector3.x, vector4.y));

				indices.Add(count);
				indices.Add(count + 1);
				indices.Add(count + 2);
				indices.Add(count);
				indices.Add(count + 2);
				indices.Add(count + 3);

				colours.Add(color);
				colours.Add(color);
				colours.Add(color);
				colours.Add(color);
			}

			internal void Render(int x, int y)
			{
				if (meshChunks[x, y] != null)
				{
					Graphics.DrawMesh(meshChunks[x, y], rootPosition, Quaternion.identity, material, renderLayer, null, 0, materialProperties);
				}
			}

			private struct AtlasInfo
			{
				public Bits requiredConnections;
				public Bits forbiddenConnections;
				public Vector4 uvBox;
				public string name;
			}
		}
	}
}