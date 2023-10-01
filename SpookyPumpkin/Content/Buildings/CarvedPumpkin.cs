using FUtility;
using KSerialization;
using System;

namespace SpookyPumpkinSO.Content.Buildings
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class CarvedPumpkin : KMonoBehaviour
	{
		[Serialize] public string currentFaceId;
		[Obsolete][Serialize] private int currentFace;
		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private Operational operational;

		public const int MAX_INDEX = 7;

		public CarvedPumpkin()
		{
			currentFace = -1;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (currentFace != -1)
				currentFaceId = ModDb.pumpkinFaces.GetIdForIndex(currentFace);
			else if (currentFaceId.IsNullOrWhiteSpace())
				currentFaceId = ModDb.pumpkinFaces.resources.GetRandom().Id;

			Carve(currentFaceId);
		}

		public void Carve(string faceId)
		{
			Log.Debug("carving from id " + faceId);
			var face = ModDb.pumpkinFaces.TryGet(faceId);

			if (face == null)
			{
				Log.Warning($"No pumpkin face with id {faceId}");
				return;
			}

			kbac.SwapAnims(new[] { face.animFile });
			kbac.Play(operational.IsOperational ? "on" : "off");
			currentFaceId = faceId;
			currentFace = -1;
		}
	}
}
