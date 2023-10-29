using FUtility;
using ONITwitchLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Events.EventTypes;
using Twitchery.Content.Scripts.UI;
using UnityEngine;

namespace Twitchery.Content.Scripts.Touchers
{
	public class ChaosToucher : Toucher
	{
		public Dictionary<SimHashes, SimHashes> cellTransformLookup;
		public HashSet<SimHashes> ignoreElements;

		public float delay;
		public Func<ToucherVisualizer> CreateVisualizerFn;

		public Color color;

		private float elapsedTime;
		private bool hasRolled;

		private SpinnyWheel wheel;
		private SimHashes[] options = new SimHashes[4];

		public override void OnSpawn()
		{
			base.OnSpawn();
			paused = true;
		}

		private void OnRolled(int index)
		{
			var targetId = options[index];
			var element = ElementLoader.FindElementByHash(targetId);
			markerColor = element.substance.uiColour;

			if (element.state == Element.State.Solid)
			{
				AddTagToSimhash(GameTags.Solid, targetId);
				AddTagToSimhash(GameTags.Liquid, targetId);
			}
			else
			{
				AddTagToSimhash(GameTags.Gas, targetId);
				AddTagToSimhash(GameTags.Liquid, targetId);
			}

			paused = false;
			hasRolled = true;
		}

		public void AddTagToSimhash(Tag tag, SimHashes target)
		{
			foreach (var element in ElementLoader.elements)
			{
				if (ignoreElements != null && ignoreElements.Contains(element.id))
					continue;

				if (element.oreTags != null && element.oreTags.Contains(tag))
				{
					cellTransformLookup ??= new();
					cellTransformLookup[element.id] = target;
				}
			}
		}

		public override bool UpdateCell(int cell, float dt)
		{
			if (!hasRolled)
				return false;

			elapsedTime += dt;

			if (elapsedTime <= delay)
				return false;

			var element = Grid.Element[cell];

			if (ignoreElements != null && ignoreElements.Contains(element.id))
				return false;

			if (cellTransformLookup != null && cellTransformLookup.TryGetValue(element.id, out SimHashes hash))
			{
				var targetElement = ElementLoader.FindElementByHash(hash);

				var massMultiplier = 1f;

				if (targetElement.IsGas)
				{
					massMultiplier = element.IsGas ? 1 : element.IsLiquid ? 0.1f : 0.01f;
				}
				else if (targetElement.IsLiquid)
				{
					massMultiplier = element.IsSolid ? 0.5f : 1f;
				}

				var danger = (Danger)(long)ONITwitchLib.Core.TwitchSettings.GetSettingsDictionary()["MaxDanger"];
				float? temperature = targetElement.defaultValues.temperature;

				if (danger <= Danger.Medium)
				{
					temperature = null;
				}

				ReplaceElement(cell, element, hash, true, massMultiplier, tempOverride: temperature);

				AudioUtil.PlaySound(ModAssets.Sounds.WOOD_THUNK, Grid.CellToPos(cell), ModAssets.GetSFXVolume());

				return true;
			}

			return false;
		}

		public void Roll()
		{
			CreateWheel();

			if (ChaosTouchEvent.targetElements == null || ChaosTouchEvent.targetElements.Count == 0)
				ChaosTouchEvent.SetElements();

			for (int i = 0; i < 4; i++)
			{
				var element = ChaosTouchEvent.targetElements.GetRandom();
				wheel.SetOption(i, ElementLoader.FindElementByHash(element).tag.ProperNameStripLink());
				options[i] = element;

				Log.Debug($"rolled: {i} - {element}");
			}

			wheel.Roll();
		}

		private void CreateWheel()
		{
			if (wheel == null)
			{
				var canvas = FUtility.FUI.Helper.GetACanvas("AETE_Spinny");
				var gameObject = Instantiate(ModAssets.Prefabs.spinnyWheel, canvas.transform);

				gameObject.SetActive(true);
				wheel = gameObject.AddOrGet<SpinnyWheel>();
				wheel.OnRoll += OnRolled;
			}
		}

		public override void OnCleanUp()
		{
			Util.KDestroyGameObject(wheel);
			base.OnCleanUp();
		}

		public delegate bool TransformationActionDelegate(int cell);
		public delegate bool TransformationConditionDelegate(int cell, Element element);

		public class Transformation
		{
			public TransformationConditionDelegate condition;
			public TransformationActionDelegate action;
		}
	}
}
