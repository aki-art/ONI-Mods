/*using DecorPackA.Scripts;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	public class FPSMeter : KMonoBehaviour, ISim200ms
	{
		[MyCmpReq] private MoodLamp moodLamp;

		public void Sim200ms(float dt)
		{
			float t = DecorPackA_Mod.Instance.fps;
			t = Mathf.InverseLerp(20, 60, t);

			moodLamp.lampKbac.TintColour = Color.Lerp(Color.red, Color.green, t);
		}
	}
}
*/