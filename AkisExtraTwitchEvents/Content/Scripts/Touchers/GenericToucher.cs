using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Scripts.Touchers
{
	public class GenericToucher : Toucher
	{
		public Dictionary<SimHashes, SimHashes> cellTransformLookup;
		public List<Transformation> transformations;
		public HashSet<SimHashes> debrisLookup;
		public HashSet<SimHashes> ignoreElements;

		public float delay;
		public Func<ToucherVisualizer> CreateVisualizerFn;

		public Color color;

		private float elapsedTime;

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
			elapsedTime += dt;

			if (elapsedTime <= delay)
				return false;

			var element = Grid.Element[cell];

			if (ignoreElements != null && ignoreElements.Contains(element.id))
				return false;

			if (cellTransformLookup != null && cellTransformLookup.TryGetValue(element.id, out SimHashes hash))
			{
				ReplaceElement(cell, element, hash);
				return true;
			}

			if (transformations != null)
			{
				bool transformed = false;
				foreach (var transformation in transformations)
				{
					if (transformation.condition(cell, element))
					{
						transformed = true;

						if (transformation.action(cell))
							break;
					}
				}

				if (transformed)
					return true;
			}

			return false;
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
