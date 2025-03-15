using HarmonyLib;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class AnimationMimic : KMonoBehaviour
	{
		[MyCmpReq] protected KBatchedAnimController kbac;
		[MyCmpReq] protected SymbolOverrideController controller;
		[MyCmpReq] protected KBoxCollider2D collider;

		[Serialize] public string sprite;
		[Serialize] public bool storedItem;
		[Serialize] public bool storedMinion;
		[Serialize] public HashedString currentAnim;
		[Serialize] public float positionPercent;
		[Serialize] public float rotation;
		[Serialize] public float scale;
		[Serialize] public bool flipX;
		[Serialize] public bool flipY;
		[Serialize] public Vector3 offset;

		[Serialize] public List<string> animFiles;
		[Serialize] public List<AnimFileOverrideAnimData> overrideAnimFiles;
		[Serialize] public List<SymbolOverrideAnimData> overrideSymbols;
		[Serialize] public HashSet<KAnimHashedString> hiddenSymbols;

		public struct AnimFileOverrideAnimData
		{
			public string file;
			public float priority;
		}

		public struct SymbolOverrideAnimData
		{
			public HashedString source;
			public KAnimHashedString symbol;
			public KAnimHashedString symbolAnim;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (storedItem)
				ApplyAnimation();
		}

		public void ApplyAnimation()
		{
			var anims = new List<KAnimFile>();
			foreach (var animFile in animFiles)
			{
				if (Assets.TryGetAnim(animFile, out var anim))
					anims.Add(anim);
			}

			kbac.SwapAnims([.. anims]);

			if (overrideAnimFiles != null)
			{
				foreach (var animOverride in overrideAnimFiles)
				{
					if (Assets.TryGetAnim(animOverride.file, out var anim))
						kbac.AddAnimOverrides(anim, animOverride.priority);
				}
			}

			if (overrideSymbols != null)
				foreach (var symbol in overrideSymbols)
				{
					if (Assets.TryGetAnim(symbol.symbolAnim, out var sourceAnim))
					{
						controller.AddSymbolOverride(symbol.source, sourceAnim.GetData().build.GetSymbol(symbol.symbol));
					}
				}

			if (animFiles != null)
			{
				foreach (var anim in kbac.animFiles)
				{
					if (anim?.data.name == anim.name)
					{
						var build = anim.GetData().build;

						foreach (var symbol in build.symbols)
							kbac.SetSymbolVisiblity(symbol.hash, !hiddenSymbols.Contains(symbol.hash));

						break;
					}
				}
			}

			kbac.Play(currentAnim, KAnim.PlayMode.Paused);
			kbac.SetPositionPercent(positionPercent);

			kbac.animScale = scale;
			kbac.offset = offset;
			kbac.FlipX = flipX;
			kbac.FlipY = flipY;
			kbac.Rotation = rotation;
		}

		public void ScribeAnimation(GameObject original)
		{
			Log.Debug("charring animation");

			original.TryGetComponent(out KBatchedAnimController originalKbac);

			animFiles = originalKbac.animFiles.Select(a => a.name).ToList();

			Log.Debug($"animFiles: {animFiles.Join()}");
			original.TryGetComponent(out KBatchedAnimController mKbac);
			original.TryGetComponent(out SymbolOverrideController mController);

			if (mKbac.OverrideAnimFiles != null)
			{
				overrideAnimFiles = mKbac.overrideAnimFiles.Select(d => new AnimFileOverrideAnimData()
				{
					file = d.file.name,
					priority = d.priority
				}).ToList();
				Log.Debug($"overrideAnimFiles: {overrideAnimFiles.Join(o => o.file)}");
			}

			overrideSymbols = mController.symbolOverrides.Select(s => new SymbolOverrideAnimData()
			{
				source = s.targetSymbol,
				symbolAnim = s.sourceSymbol.build.fileHash,
				symbol = s.sourceSymbol.hash
			}).ToList();


			Log.Debug($"overrideSymbols: {overrideSymbols.Join(o => HashCache.Get().Get(o.symbolAnim))}");

			hiddenSymbols = mKbac.hiddenSymbolsSet;

			if (!currentAnim.IsValid)
			{
				currentAnim = originalKbac.currentAnim;
				positionPercent = originalKbac.GetPositionPercent();
			}

			scale = originalKbac.animScale;
			rotation = originalKbac.rotation;
			flipX = originalKbac.FlipX;
			flipY = originalKbac.FlipY;
			offset = originalKbac.Offset;

			storedItem = true;
		}

	}
}
