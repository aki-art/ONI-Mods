using KSerialization;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs.Foods;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Radish : StateMachineComponent<Radish.SMInstance>
	{
		[SerializeField] public Storage raddishStorage;
		[SerializeField] public int consumedStages;
		[Serialize] public bool hasBeenFilled;
		[Serialize] public bool hasLanded;

		public override void OnSpawn()
		{
			if (!hasBeenFilled)
			{
				var item = FUtility.Utils.Spawn(RawRadishConfig.ID, gameObject);
				item.GetComponent<PrimaryElement>().Mass = raddishStorage.capacityKg;
				raddishStorage.Store(item, true);
				hasBeenFilled = true;
			}

			GetComponent<KSelectable>().AddStatusItem(TStatusItems.CalorieStatus, this);
			GetComponent<KBatchedAnimController>().Offset = new Vector3(0, -3);

			smi.StartSM();
		}

		public class States : GameStateMachine<States, SMInstance, Radish>
		{
			public State arriveCheck;
			public State arriving;
			public State arrivingLanding;
			public State idle;

			private static readonly CellOffset[] offsetsForCellsUnder = new[]
			{
				new CellOffset(-1, -4),
				new CellOffset(0, -4),
				new CellOffset(1, -4)
			};

			private static CellOffset groundCheckOffset = new(0, -1);

			private static readonly HashedString[] animations =
			{
				"idle5",
				"idle4",
				"idle3",
				"idle2",
				"idle1",
			};

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = arriveCheck;

				arriveCheck
					.Enter(smi =>
					{
						var cellBelow = Grid.OffsetCell(Grid.PosToCell(smi), groundCheckOffset);

						if (!Grid.IsValidCell(cellBelow))
						{
							smi.GoTo(smi.sm.arrivingLanding);
						}

						var isFoundationEmpty = GridUtil.IsCellFoundationEmpty(cellBelow);
						/*						ModAssets.AddText(Grid.CellToPosCCC(cellBelow, Grid.SceneLayer.FXFront2), Color.yellow, isFoundationEmpty? "E" : "B");*/
						smi.GoTo(isFoundationEmpty
							? smi.sm.arriving
							: smi.sm.arrivingLanding);
					});
				arriving
					.ToggleGravity(arrivingLanding)
					.PlayAnim("falling");

				arrivingLanding
					.Enter(DamageTilesUnder)
					.PlayAnim("arrive", KAnim.PlayMode.Once)
					.QueueAnim("idle1", false)
					.OnAnimQueueComplete(idle);

				idle
					.Enter(OnStorageChange) // trigger it once anyway
					.EventHandler(GameHashes.OnStorageChange, OnStorageChange);
			}

			private void DamageTilesUnder(SMInstance smi)
			{
				if (smi.master.hasLanded)
					return;

				smi.master.hasLanded = true;
				var originCell = Grid.PosToCell(smi);

				foreach (var offset in offsetsForCellsUnder)
				{
					var cell = Grid.OffsetCell(originCell, offset);

					if (Grid.IsValidCell(cell))
						WorldDamage.Instance.ApplyDamage(cell, 0.33f, -1);
				}
			}

			private void OnStorageChange(SMInstance smi)
			{
				var storage = smi.master.raddishStorage;

				if (storage.IsEmpty())
				{
					Util.KDestroyGameObject(smi.gameObject);
					return;
				}

				var stagesCount = smi.master.consumedStages;

				var progress = storage.MassStored() / storage.capacityKg;
				progress = Mathf.Clamp01(progress);

				var currentStage = Mathf.FloorToInt(progress * stagesCount);
				currentStage = Mathf.Clamp(currentStage, 0, animations.Length - 1);

				smi.kbac.Play(animations[currentStage], KAnim.PlayMode.Paused);
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, Radish, object>.GameInstance
		{
			public KBatchedAnimController kbac;
			public Vector3 velocity;
			public float fallSpeed;

			public static float gasSpeed = 0.1f;
			public static float liquidSpeed = 0.05f;

			public SMInstance(Radish master) : base(master)
			{
				kbac = master.GetComponent<KBatchedAnimController>();
			}
		}
	}
}
