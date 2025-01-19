using System.Collections.Generic;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content
{
	public class Elements
	{
		public static ElementInfo
			Jello = new("AETE_Jello", "aete_jello_kanim", Element.State.Liquid, Color.green),
			EarWax = ElementInfo.Solid("AETE_EarWax", Util.ColorFromHex("c2b691")),
			FrozenJello = ElementInfo.Solid("AETE_FrozenJello", Color.green),
			FakeLumber = new("AETE_FakeLumber", "wood_kanim", Element.State.Solid, Color.green),
			PinkSlime = new("AETE_PinkSlime", "aete_pinkslime_kanim", Element.State.Liquid, Util.ColorFromHex("ff63bc")),
			FrozenPinkSlime = ElementInfo.Solid("AETE_FrozenPinkSlime", Util.ColorFromHex("ff63bc")),
			Honey = ElementInfo.Liquid("AETE_Honey", Util.ColorFromHex("ff8f17")),
			FrozenHoney = ElementInfo.Solid("AETE_FrozenHoney", Util.ColorFromHex("ff8f17"));

		public static void RegisterSubstances(List<Substance> list)
		{
			var frozenJello = FrozenJello.CreateSubstance();
			frozenJello.material.SetFloat("_WorldUVScale", 2.5f);
			list.Add(frozenJello);

			var earWax = EarWax.CreateSubstance();
			earWax.material.SetFloat("_WorldUVScale", 2.5f);
			list.Add(earWax);

			foreach (var element in ElementUtil.elements)
				if (!element.isInitialized)
					list.Add(element.CreateSubstance());
		}

		public static ElementsAudio.ElementAudioConfig[] CreateAudioConfigs(ElementsAudio instance)
		{
			return
			[
				ElementUtil.CopyElementAudioConfig(SimHashes.SlimeMold, Jello),
				ElementUtil.CopyElementAudioConfig(SimHashes.Polypropylene, FakeLumber),
			];
		}
	}
}
