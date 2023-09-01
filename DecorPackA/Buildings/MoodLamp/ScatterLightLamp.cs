using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	public class ScatterLightLamp : KMonoBehaviour
	{
		[MyCmpReq] private Operational operational;
		[MyCmpReq] private MoodLamp moodLamp;

		private GameObject lightOverlay;

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)GameHashes.OperationalChanged, OnOperationalChanged);
			Subscribe(ModEvents.OnMoodlampChanged, OnMoodlampChanged);

			OnMoodlampChanged(moodLamp.currentVariantID);
		}

		private void OnMoodlampChanged(object data)
		{
			Log.Debuglog("moodlamp changed");

			if (data is string id)
				CreateParticles(id);
			else Log.Debuglog($"not a string: {data}");
		}

		private void DestroyParticles()
		{
			if(lightOverlay != null)
				Util.KDestroyGameObject(lightOverlay);

			lightOverlay = null;
		}

		private void CreateParticles(string id)
		{
			Log.Debuglog("CreateParticles");

			if (ModAssets.Prefabs.scatterLampPrefabs.TryGetValue(id, out var particles))
			{
				Log.Debuglog("particles");
				DestroyParticles();

				lightOverlay = Instantiate(particles);
				lightOverlay.transform.SetParent(transform);
				lightOverlay.transform.position = transform.position with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };
				lightOverlay.transform.position += Vector3.up;

				lightOverlay?.SetActive(enabled && operational.IsOperational);
			}
			else
			{
				Log.Warning($"Could not create particle system, no particles registered with the id of {moodLamp.currentVariantID}");
			}
		}

		private void OnOperationalChanged(object obj)
		{
			lightOverlay?.SetActive(enabled && operational.IsOperational);
		}

		public override void OnCmpEnable()
		{
			Log.Debuglog("OnCmpEnable");
			base.OnCmpEnable();
			lightOverlay?.SetActive(operational.IsOperational);
		}

		public override void OnCmpDisable()
		{
			base.OnCmpDisable();
			lightOverlay?.SetActive(false);
		}
	}
}
