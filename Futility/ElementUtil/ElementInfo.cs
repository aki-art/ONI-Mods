﻿using FUtility;
using UnityEngine;

namespace FUtility.ElementUtil
{
	// Just a bunch of helper and convenience methods
	public class ElementInfo
	{
		public string id;
		public Element.State state;
		public string anim;
		public Color color;
		public Color uiColor;
		public Color conduitColor;
		public bool isInitialized;

		public SimHashes SimHash { get; private set; }

		public Tag Tag { get; private set; }

		public ElementInfo(string id, string anim, Element.State state, Color color)
		{
			this.id = id;
			this.anim = anim;
			this.state = state;
			this.color = color;

			SimHash = ElementUtil.RegisterSimHash(id);
			ElementUtil.elements.Add(this);

			Tag = id;
		}

		// be able to reference this class without havng to cast to (SimHashes)
		public static implicit operator SimHashes(ElementInfo info)
		{
			return info.SimHash;
		}

		// GetElement(Tag) is the fastest way to fetch an element, but i can't remember that so here is a shortcut for it
		public Element Get()
		{
			if (ElementLoader.elementTagTable == null)
			{
				Log.Warning("Trying to fetch element too early, elements are not loaded yet.");
				return null;
			}

			return ElementLoader.GetElement(Tag);
		}

		public Substance CreateSubstance(bool specular = false, Material material = null, Color? uiColor = null, Color? conduitColor = null, Color? specularColor = null, string normal = null)
		{
			Log.Debug("creating substance for " + id);
			if (material == null)
			{
				material = state == Element.State.Solid ? global::Assets.instance.substanceTable.solidMaterial : global::Assets.instance.substanceTable.liquidMaterial;
			}

			Log.Assert("material", material);

			isInitialized = true;

			return ElementUtil.CreateSubstance(SimHash, specular, anim, state, color, material, uiColor ?? color, conduitColor ?? color, specularColor, normal);
		}

		public Substance CreateSubstance(Color uiColor, Color conduitColor)
		{
			return CreateSubstance(false, null, uiColor, conduitColor);
		}

		public static ElementInfo Solid(string id, Color color)
		{
			return new ElementInfo(id, id.ToLowerInvariant() + "_kanim", Element.State.Solid, color);
		}

		public static ElementInfo Liquid(string id, Color color)
		{
			return new ElementInfo(id, "liquid_tank_kanim", Element.State.Liquid, color);
		}

		public static ElementInfo Gas(string id, Color color)
		{
			return new ElementInfo(id, "gas_tank_kanim", Element.State.Gas, color);
		}

		public override string ToString()
		{
			return SimHash.ToString();
		}
	}
}
