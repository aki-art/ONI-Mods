
using DecorPackA.Scripts;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using static DecorPackA.STRINGS.UI.BUILDINGEFFECTS;

namespace DecorPackA.Buildings.StainedGlassTile
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DyeInsulator : KMonoBehaviour, IGameObjectEffectDescriptor
	{
		[MyCmpReq] private Deconstructable deconstructable;
		[MyCmpReq] private Building building;

		public float Modifier { get; private set; } = 1f;

		public override void OnSpawn()
		{
			var dye = deconstructable.constructionElements[1];
			var isAbyssalite = dye == SimHashes.Katairite.CreateTag();

			var TCTransparent = GetThermalConductivity(0);
			var TCDye = GetThermalConductivity(1);
			var ratio = Mod.Settings.GlassTile.DyeRatio;
			ratio = Mathf.Clamp01(ratio);

			if (isAbyssalite && !DecorPackA_Mod.Instance.hasAskedUserAboutAbyssalite)
				AskAboutAbyssalite();

			var isNerfedAbyssalite = Mod.Settings.GlassTile.NerfAbyssalite && isAbyssalite;

			Modifier = isNerfedAbyssalite
				? 1f
				: Mathf.Pow(TCDye, ratio) * Mathf.Pow(TCTransparent, 1f - ratio) / TCTransparent;

			SetInsulation(Modifier);
		}

		private void AskAboutAbyssalite()
		{
			DecorPackA_Mod.Instance.hasAskedUserAboutAbyssalite = true;

			if (!Mod.Settings.GlassTile.UseDyeTC)
				return;

			var screen = Util.KInstantiateUI<InfoDialogScreen>(
				ScreenPrefabs.Instance.InfoDialogScreen.gameObject,
				FUtility.FUI.Helper.GetACanvas("abyssalite nerf").gameObject,
				true)
				.SetHeader("Decor Pack I")
				.AddPlainText($"Abyssalite Stained Glass Tiles are being nerfed in this update, and will no longer act as hyper insulators, which is really overpowered and unintended.")
				.AddSpacer(10)
				.AddPlainText("You appear to be using some of these tiles. Do you want to keep legacy behavior for now? (nothing will change)")
				.AddOption("Update", screen =>
				{
					Mod.Settings.GlassTile.NerfAbyssalite = true;
					screen.Deactivate();
				})
				.AddOption("Keep old behavior", screen =>
				{
					Mod.Settings.GlassTile.NerfAbyssalite = false;
					screen.Deactivate();
				});
		}

		private float GetThermalConductivity(int index) => ElementLoader.GetElement(deconstructable.constructionElements[index]).thermalConductivity;

		private string GetElementName(int index) => deconstructable.constructionElements[index].ProperNameStripLink();

		// reset insulation over this tile
		public override void OnCleanUp() => SetInsulation(1f);

		// sets insulation for the sim, because this tile is no using the standard thermal conduction rules
		private void SetInsulation(float value) => SimMessages.SetInsulation(building.GetCell(), value);

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			if (Modifier == 1f)
				return null;

			var list = new List<Descriptor>();

			var item = new Descriptor();

			var percent = GameUtil.GetFormattedPercent(Modifier * 100f - 100f, GameUtil.TimeSlice.None);
			string comparator = Modifier < 1f ? TOOLTIP.LOWER : TOOLTIP.HIGHER;

			item.SetupDescriptor(
				string.Format(THERMALCONDUCTIVITYCHANGE, percent),
				TOOLTIP.THERMALCONDUCTIVITYCHANGE
					.Replace("{dyeElement}", GetElementName(1))
					.Replace("{higherOrLower}", comparator)
					.Replace("{baseElement}", GetElementName(0))
					.Replace("{percent}", percent),
				Descriptor.DescriptorType.Effect);

			list.Add(item);

			return list;
		}
	}
}
