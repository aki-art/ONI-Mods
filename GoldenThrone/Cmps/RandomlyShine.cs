namespace GoldenThrone.Cmps
{
	public class RandomlyShine : KMonoBehaviour, ISim200ms
	{
		[MyCmpReq]
		private KBatchedAnimController kbac;

		public void Sim200ms(float dt)
		{
			if (kbac.GetCurrentAnim().name == "off" && UnityEngine.Random.value < 0.1f)
			{
				kbac.Play("shine");
				kbac.Queue("off");
			}
		}
	}
}
