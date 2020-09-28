using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entropea.WorldTraits
{
	class EarthQuake : MonoBehaviour
    {
		bool quaking = true;
		float elapsedTime;
		float duration;
		float magnitude;
		bool easingIn;
		bool easingOut;

        public void Begin()
        {
            ShakeCamera();
        }

        private void ShakeCamera()
        {
			StartCoroutine(ShakeIt(0.5f));
        }

		private IEnumerator ShakeIt(float range = 1f)
		{
			while(quaking)
			{
				elapsedTime += Time.unscaledDeltaTime;
				Vector3 offset = Vector3.zero;
				offset.x = UnityEngine.Random.Range(-range, range);
				offset.y = UnityEngine.Random.Range(-range, range);
				Vector3 target = CameraController.Instance.transform.GetPosition() + offset;
				var ortographicSize = CameraController.Instance.cameras[0].orthographicSize;
				SetCameraTargetPos(target, range, elapsedTime < 5);
				yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.05f));
			}

			yield return null;
		}

		private void SetCameraTargetPos(Vector3 pos, float range, bool starting)
		{
			if (!starting && Vector3.Distance( CameraController.Instance.transform.GetPosition(), CameraController.Instance.targetPos) > range) 
				return; 

			CameraController.Instance.isTargetPosSet = true;
			typeof(CameraController).GetProperty("targetPos").SetValue(CameraController.Instance, pos, null);
			CameraController.Instance.SetOverrideZoomSpeed(10);
		}
	}
}
