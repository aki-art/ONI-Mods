using FUtility;
using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class FrozenEntityContainer : MidasEntityContainer, ISim1000ms
	{
		[MyCmpReq] private PrimaryElement primaryElement;

		[Serialize] private string overlayAnim;

		private static Color blue = Util.ColorFromHex("52bcff");
		private KBatchedAnimController iceCubeOverlay;
		private KBatchedAnimTracker tracker;

		public override Color GetOverlayColor() => blue;

		public override StatusItem GetStatusItem() => TStatusItems.FrozenStatus;

		protected override void OnAnimationUpdated(GameObject original)
		{
			Log.Debug("Anim updated: " + original.PrefabID());
			base.OnAnimationUpdated(original);

			if (original.IsPrefabID(MinionConfig.ID))
			{
				Log.Debug("Is minion");
				SetupIceCubeSymbol("snapto_headfx");
			}
		}

		public void SetupIceCubeSymbol(string symbolToTrack)
		{
			var iceCube = new GameObject("AETE_iceCube");
			iceCube.SetActive(false);

			var position = (Vector3)kbac
				.GetSymbolTransform(symbolToTrack, out bool _)
				.GetColumn(3) with
			{
				z = Grid.GetLayerZ(Grid.SceneLayer.Creatures) - 0.1f
			};

			iceCube.transform.SetPosition(position);
			iceCubeOverlay = iceCube.AddComponent<KBatchedAnimController>();
			iceCubeOverlay.AnimFiles = new[]
			{
				Assets.GetAnim( "aete_icecube_kanim")
			};

			tracker = iceCube.AddComponent<KBatchedAnimTracker>();
			tracker.symbol = symbolToTrack;
			tracker.forceAlwaysVisible = true;

			tracker.SetAnimControllers(iceCubeOverlay, kbac);

			iceCubeOverlay.initialAnim = "1x1a";

			iceCube.SetActive(true);
			iceCubeOverlay.Play("1x1a", KAnim.PlayMode.Paused);
		}

		public override void OnCleanUp()
		{
			if (iceCubeOverlay != null)
				Util.KDestroyGameObject(iceCubeOverlay);

			Release(true);
			base.OnCleanUp();
			Mod.midasContainers.Remove(this);
		}

		public void Sim1000ms(float dt)
		{
			if (primaryElement.Temperature > 273.15)
			{
				Release(true);
				Util.KDestroyGameObject(gameObject);
			}
		}
	}
}
