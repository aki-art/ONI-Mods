using UnityEngine;

namespace Moonlet.Utils
{
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

		public ElementInfo(string id, string anim, Element.State state, Color color, bool registerSimhash)
		{
			this.id = id;
			this.anim = anim;
			Log.Debug("element info anim " + anim);
			this.state = state;
			this.color = color;

			if (registerSimhash)
			{
				SimHash = ElementUtil.RegisterSimHash(id);
				ElementUtil.elements.Add(this);
			}

			Tag = id;
		}

		// be able to reference this class without havng to cast to (SimHashes)
		public static implicit operator SimHashes(ElementInfo info) => info.SimHash;

		// GetElement(Tag) is the fastest way to fetch an element, but i can't remember that so here is a shortcut for it
		public Element Get()
		{
			if (ElementLoader.elementTagTable == null)
			{
				Log.Warn("Trying to fetch element too early, elements are not loaded yet.");
				return null;
			}

			return ElementLoader.GetElement(Tag);
		}

		public Substance CreateSubstance(string assetsPath, bool specular = false, Material material = null, Color? uiColor = null, Color? conduitColor = null, Color? specularColor = null, string normal = null)
		{
			if (material == null)
				material = state == Element.State.Solid ? Assets.instance.substanceTable.solidMaterial : Assets.instance.substanceTable.liquidMaterial;

			isInitialized = true;

			return ElementUtil.CreateSubstance(SimHash, specular, assetsPath, anim, state, color, material, uiColor ?? color, conduitColor ?? color, specularColor, normal);
		}
	}
}
