using System.Collections.Generic;
using System.Linq;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts.Touchers
{
	public class VoidToucher : Toucher
	{
		public override bool UpdateCell(int cell, float dt)
		{
			if (!IsCellAlreadyVisited(cell))
			{
				if (Grid.Element[cell].hardness == byte.MaxValue)
					return true;

				if (!AGridUtil.DestroyTile(cell))
					SimMessages.Dig(cell);

				TearOffWallPaper(cell);
				AkisTwitchEvents.Instance.AddZoneTypeOverride(cell, ProcGen.SubWorld.ZoneType.Space);

				var pooledHashSet = HashSetPool<GameObject, VoidToucher>.Allocate();

				foreach (BuildingComplete cmp in Components.BuildingCompletes.Items)
				{
					if (Grid.PosToCell(cmp) == cell)
						pooledHashSet.Add(cmp.gameObject);
				}

				foreach (GameObject building in pooledHashSet)
				{
					if (building.TryGetComponent(out Deconstructable deconstructable))
					{
						deconstructable.ForceDestroyAndGetMaterials();
					}
				}

			}

			KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Delete"));

			return true;
		}

		protected override HashSet<int> GetTilesInRadius(Vector3 position, float radius)
		{
			var circle = ProcGen.Util.GetFilledCircle(position, radius);
			return circle.Select(pos => Grid.PosToCell(pos)).ToHashSet();
		}

		protected override GameObject CreateMarker()
		{
			var marker = base.CreateMarker();
			/*			if (marker.TryGetComponent(out ParticleSystem particles))
						{
							var scale = particles.shape.scale;
							scale.x = radius * 0.5f;
							scale.y = radius * 0.5f;

							particles.shape.scale 
						}*/

			return marker;
		}

		private float TearOffWallPaper(int cell)
		{
			if (Grid.ObjectLayers[(int)ObjectLayer.Backwall].TryGetValue(cell, out var go))
			{
				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					deconstructable.ForceDestroyAndGetMaterials();
					return 1f;
				}
			}

			return 0;
		}

	}
}
