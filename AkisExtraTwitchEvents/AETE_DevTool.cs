using FUtility;
using ImGuiNET;
using System;
using System.Collections.Generic;
using Twitchery.Content;
using Twitchery.Content.Defs;
using Twitchery.Content.Events.EventTypes;
using Twitchery.Content.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Twitchery
{
	public class AETE_DevTool : DevTool
	{
		private static float pixelationAmount;
		private static string name = "";

		public AETE_DevTool() => RequiresGameRunning = true;

		public override void RenderTo(DevPanel panel)
		{
			if (ImGui.Button("CLean save"))
			{
				for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
				{
					if (Components.LiveMinionIdentities[i].nameStringKey == TPersonalities.HULK)
						Util.KDestroyGameObject(Components.LiveMinionIdentities[i]);
				}

				for (int i = 0; i < Components.StoredMinionIdentities.Count; i++)
				{
					if (Components.StoredMinionIdentities[i].nameStringKey == TPersonalities.HULK)
						Util.KDestroyGameObject(Components.StoredMinionIdentities[i]);
				}
				for (int i = 0; i < Components.MinionIdentities.Count; i++)
				{
					if (Components.MinionIdentities[i].nameStringKey == TPersonalities.HULK)
						Util.KDestroyGameObject(Components.MinionIdentities[i]);
				}
				for (int i = 0; i < Components.MinionAssignablesProxy.Count; i++)
				{
					if (Components.MinionAssignablesProxy[i].target.GetProperName() == STRINGS.DUPLICANTS.PERSONALITIES.AKISEXTRATWITCHEVENTS_HULK.NAME)
						Util.KDestroyGameObject(Components.MinionAssignablesProxy[i]);
				}

				foreach (var cooler in UnityEngine.Object.FindObjectsOfType<WaterCooler>())
				{
					if (cooler.GetSelectedOption() == Elements.Honey.Tag)
						cooler.ChosenBeverage = SimHashes.Water.CreateTag();
				}

				foreach (var grave in Mod.graves.Items)
				{
					grave.ClearData();
				}
			}

			if (ImGui.Button("Force Redraw"))
			{
				AkisTwitchEvents.Instance.OnDraw();
			}

			var selected = SelectTool.Instance.selected;
			if (selected != null)
			{
				foreach (var imgui in selected.GetComponents<IImguiDebug>())
				{
					imgui.OnImgui();
					ImGui.Separator();
				}

				if (selected.TryGetComponent(out Switch @switch))
				{
					if (PuzzleDoor2.debugTarget == null)
					{
						if (ImGui.Button("connect"))
						{
							if (PuzzleDoor2.debugDoor != null)
							{
								PuzzleDoor2.debugDoor.SetTarget(@switch);
								PuzzleDoor2.debugDoor = null;
								PuzzleDoor2.debugTarget = null;
							}
							else
							{
								PuzzleDoor2.debugTarget = @switch;
							}
						}
					}
					else if (PuzzleDoor2.debugTarget == @switch && ImGui.Button("diconnect"))
					{
						PuzzleDoor2.debugTarget = null;
					}
				}

				if (selected.TryGetComponent(out KSelectable kSelectable))
				{
					ImGui.InputText("name", ref name, 1024);

					if (ImGui.Button("Rename"))
					{
						kSelectable.SetName(name);
						NameDisplayScreen.Instance.UpdateName(kSelectable.gameObject);
						kSelectable.Trigger((int)GameHashes.NameChanged, name);
					}
				}

				if (selected.TryGetComponent(out MinionIdentity minion))
				{
					if (ImGui.Button("Polymoprh"))
					{
						PolymorphEvent.Polymorph(true, minion, out _, out _);
					}
				}
			}

			if (ImGui.CollapsingHeader("Retro Vision Settings"))
			{
				ImGui.DragFloat("Spread", ref AETE_DitherPostFx.Instance.fullyDitheredConfig.spread, 0.01f, 0f, 1f);
				ImGui.DragInt("Color Count", ref AETE_DitherPostFx.Instance.fullyDitheredConfig.colorCount);
				ImGui.DragInt("Down Samples", ref AETE_DitherPostFx.Instance.fullyDitheredConfig.downSamples, 1, 0, 8);
				ImGui.DragInt("Bayer Level", ref AETE_DitherPostFx.Instance.fullyDitheredConfig.bayerLevel, 1, 0, 3);

				if (ImGui.Button("Dither sequence"))
					AETE_DitherPostFx.Instance.DoDither();

				if (ImGui.DragFloat("Dither", ref pixelationAmount, 0.01f, 0, 1))
					AETE_DitherPostFx.Instance.SetDitherExactly(pixelationAmount);
			}

			if (ImGui.CollapsingHeader("LOS"))
				LineOfSightTest();

			if (ImGui.CollapsingHeader("Trees"))
				TreeSpawnTest();
		}

		private int treeRange = 6;
		private int treeMinimumSteps = 4, treeMaximumSteps = 8;
		private int treeMaxDistance = 25;
		private int treeComplexity = 4;
		private float treeBranchChance = 0.3f;

		private void TreeSpawnTest()
		{
			ImGui.DragInt("Range##treeRange", ref treeRange);
			ImGui.DragInt("MaxDistance##treeMaxDistance", ref treeMaxDistance);
			ImGui.DragInt("MinimumSteps##treeMinimumSteps", ref treeMinimumSteps);
			ImGui.DragInt("MaximumSteps##treeMaximumSteps", ref treeMaximumSteps);
			ImGui.DragInt("Complexity##treeComplexity", ref treeComplexity);
			ImGui.DragFloat("Branch Chance##treeBranchChance", ref treeBranchChance);

			if (ImGui.Button("Generate"))
			{
				var cell = SelectTool.Instance.selectedCell;
				if (!Grid.IsValidCell(cell))
					return;

				var branch = FUtility.Utils.Spawn(BranchWalkerConfig.ID, Grid.CellToPos(cell));
				var branchWalker = branch.GetComponent<BranchWalker>();
				branchWalker.minimumSteps = treeMinimumSteps;
				branchWalker.maximumSteps = treeMaximumSteps;
				branchWalker.stepRange = treeRange;
				branchWalker.maxDistance = treeMaxDistance;
				branchWalker.branchOffChance = treeBranchChance;
				branchWalker.maxComplexity = treeComplexity;

				branchWalker.Generate();
			}
		}

		private int range = 3;
		private int depth = 1;
		private float startSlope = 1;
		private float endSlope = 0;
		private int directionIndex = 0;
		private List<int> losCells = new();
		private List<Text> losMarkers = new();
		private bool dirty;

		private void LineOfSightTest()
		{
			var cell = SelectTool.Instance.selectedCell;

			dirty |= ImGui.DragInt("Range##lodrange", ref range);
			dirty |= ImGui.DragInt("Depth##loddepth", ref depth);
			dirty |= ImGui.DragFloat("startSlope##lodstartSlope", ref startSlope);
			dirty |= ImGui.DragFloat("endSlope##lodendSlope", ref endSlope);

			dirty |= ImGui.Combo("Direction", ref directionIndex, Enum.GetNames(typeof(DiscreteShadowCaster.Octant)), 8);

			if (dirty)
			{
				foreach (var text in losMarkers)
					Util.KDestroyGameObject(text.gameObject);

				losMarkers.Clear();
				losCells.Clear();

				var xy = Grid.CellToXY(cell);
				BranchWalker.ScanOctant(xy, range, depth, (DiscreteShadowCaster.Octant)directionIndex, startSlope, endSlope, losCells, 50);

				for (int i = 0; i < losCells.Count; i++)
				{
					int losCell = losCells[i];
					losMarkers.Add(ModAssets.AddText(Grid.CellToPosCCC(losCell, Grid.SceneLayer.FXFront2), Color.red, i.ToString()));
				}

				dirty = false;
			}
		}
	}
}
