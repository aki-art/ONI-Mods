using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Linq;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class JailEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Jail";

		private MinionIdentity currentTarget;
		private string currentTargetName;

		public override bool Condition()
		{
			if (currentTarget == null || !IsJailable(currentTarget))
				OnDraw();

			return currentTarget != null;
		}

		private bool IsJailable(MinionIdentity currentTarget)
		{
			if (currentTarget.IsNullOrDestroyed())
				return false;

			return currentTarget.GetComponent<AETE_MinionStorage>().IsTargetableByEvents();
		}

		public override void OnDraw()
		{
			base.OnDraw();
			GetIdentity(out var newIdentity, false);
			SetTarget(newIdentity);
		}

		public override void OnGameLoad()
		{
			base.OnGameLoad();
			OnDraw();
		}

		private bool GetIdentity(out MinionIdentity identity, bool pushWarning)
		{
			if (currentTarget != null && !currentTarget.HasTag(GameTags.Dead))
			{
				identity = currentTarget;
				return true;
			}

			var targets = Components.LiveMinionIdentities.items
				.Where(IsJailable);

			if (targets.Count() == 0)
			{
				if (pushWarning)
					ToastManager.InstantiateToast("Warning", "No Jailable duplicants around, cannot execute event.");

				identity = null;
				return false;
			}

			identity = targets.GetRandom();
			return false;
		}

		private void SetTarget(MinionIdentity identity)
		{
			currentTarget = identity;

			// dont want color tag here because this is sent to the twitch chat
			currentTargetName = identity == null ? null : Util.StripTextFormatting(identity.GetProperName());

			SetName(STRINGS.AETE_EVENTS.JAIL.TOAST.Replace("{Name}", currentTargetName ?? "N/A"));
		}


		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			if (Components.LiveMinionIdentities.Count == 0)
				return;

			var isOriginalTarget = true;
			var previousname = currentTargetName ?? "Duplicant";

			if (currentTarget == null)
				isOriginalTarget = GetIdentity(out currentTarget, true);

			if (currentTarget == null)
				return;

			JailDupe(currentTarget);

			if (!isOriginalTarget)
			{
				ToastManager.InstantiateToastWithGoTarget(
					STRINGS.AETE_EVENTS.JAIL.TOAST.Replace("{Name}", currentTargetName ?? "Duplicant"),
					STRINGS.AETE_EVENTS.JAIL.DESC_NOTFOUND
						.Replace("{Previous}", previousname)
						.Replace("{Name}", currentTarget.GetProperName()),
					currentTarget.gameObject);
			}
			else
				ToastManager.InstantiateToastWithGoTarget(
					STRINGS.AETE_EVENTS.JAIL.TOAST,
					STRINGS.AETE_EVENTS.JAIL.DESC.Replace("{Name}", currentTarget.GetProperName()),
					currentTarget.gameObject);

			SetTarget(null);
		}

		private void JailDupe(MinionIdentity currentTarget)
		{
			var footCell = Grid.PosToCell(currentTarget);

			var tileDef = Assets.GetBuildingDef(MeshTileConfig.ID);
			Tag[] elements = [SimHashes.IronOre.CreateTag()];

			for (var x = -1; x <= 1; x++)
			{
				for (var y = -1; y <= 1; y++)
				{
					if (!(x == 0 && y == 0))
						BuildTile(tileDef, elements, Grid.OffsetCell(footCell, x, y));
				}
			}

			currentTarget.GetComponent<KPrefabID>().AddTag(TTags.jailed, true);

			currentTarget.transform.position = Grid.CellToPosCBC(footCell, Grid.SceneLayer.Creatures) with { z = currentTarget.transform.position.z };
		}

		private void BuildTile(BuildingDef def, Tag[] element, int cell)
		{
			if (GridUtil.IsCellFoundationEmpty(cell))
				def.Build(cell, Orientation.Neutral, null, element, 300);
		}
	}
}
