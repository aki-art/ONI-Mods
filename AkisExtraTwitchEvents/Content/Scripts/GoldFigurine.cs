using FUtility;
using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class GoldFigurine : KMonoBehaviour
	{
		[MyCmpReq] private PrimaryElement primaryElement;
		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private SymbolOverrideController controller;

		[Serialize] public string originalPrefabId;
		[Serialize] public HashedString currentAnim;
		[Serialize] public float positionPercent;

		public void SetPrefab(GameObject target)
		{
			if (target == null)
				return;

			var prefabId = target.PrefabID().ToString();

			if (!target.TryGetComponent(out PrimaryElement targetPrimaryElement))
				Failed(prefabId);

			originalPrefabId = prefabId;
			primaryElement.AddDisease(targetPrimaryElement.DiseaseIdx, targetPrimaryElement.DiseaseCount, "Midas Touch spawn");
			primaryElement.SetMassTemperature(targetPrimaryElement.Mass, targetPrimaryElement.Temperature);

			CopyAnim(target);
		}

		private void Failed(string prefabId)
		{
			Log.Warning($"Gold Figurine cannot find apply prefab ID {prefabId}");
			SpawnGold();
		}

		private void CopyAnim(GameObject original)
		{
			original.TryGetComponent(out KBatchedAnimController originalKbac);

			kbac.SwapAnims(originalKbac.animFiles);

			original.TryGetComponent(out KBatchedAnimController mKbac);
			original.TryGetComponent(out SymbolOverrideController mController);

			if (mKbac.OverrideAnimFiles != null)
			{
				foreach (var animOverride in mKbac.OverrideAnimFiles)
				{
					kbac.AddAnimOverrides(animOverride.file, animOverride.priority);
				}
			}

			foreach (var symbol in mController.symbolOverrides)
			{
				controller.AddSymbolOverride(symbol.targetSymbol, symbol.sourceSymbol);
			}

			if (mKbac.animFiles != null)
			{
				foreach (var mAnim in mKbac.animFiles)
				{
					foreach (var anim in kbac.animFiles)
					{
						if (anim?.data.name == anim.name)
						{
							var build = anim.GetData().build;

							foreach (var symbol in build.symbols)
							{
								kbac.SetSymbolVisiblity(symbol.hash, mKbac.GetSymbolVisiblity(symbol.hash));
							}

							break;
						}
					}
				}
			}

			if (!currentAnim.IsValid)
			{
				currentAnim = originalKbac.currentAnim;
				positionPercent = originalKbac.GetPositionPercent();
			}

			kbac.Play(currentAnim, KAnim.PlayMode.Paused);
			kbac.SetPositionPercent(positionPercent);
			kbac.animScale = originalKbac.animScale;
			kbac.TintColour = Color.yellow;
			kbac.offset = originalKbac.offset;
			kbac.FlipX = originalKbac.flipX;
			kbac.FlipY = originalKbac.flipY;
			kbac.Rotation = originalKbac.Rotation;
		}

		private void SpawnGold()
		{
			if (primaryElement.Mass > 0)
			{
				ElementLoader.FindElementByHash(SimHashes.Gold).substance.SpawnResource(
					transform.position,
					primaryElement.Mass,
					primaryElement.Temperature,
					primaryElement.DiseaseIdx,
					primaryElement.DiseaseCount);
			}

			Util.KDestroyGameObject(this);
		}
	}
}
