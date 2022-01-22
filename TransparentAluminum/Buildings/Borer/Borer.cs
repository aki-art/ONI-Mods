using FUtility;
using KSerialization;
using STRINGS;
using System;
using UnityEngine;

namespace TransparentAluminum.Buildings.Borer
{
    public class Borer : StateMachineComponent<Borer.StatesInstance>
	{
		public static Operational.Flag canDigFlag = new Operational.Flag("borer_can_dig", Operational.Flag.Type.Requirement);

		[SerializeField]
		public float maxIntegrity;

		[Serialize]
		public float integrity;

		[MyCmpGet]
		BuildingHP hp;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			if (integrity <= 0)
            {
				integrity = maxIntegrity;
            }

			smi.StartSM();
			smi.sm.IntegrityParam.Set(integrity, smi);
		
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu); 
		}

        private void OnRefreshUserMenu(object obj)
		{
			KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", "Test", () => smi.GoTo(smi.sm.drilling));
			Game.Instance.userMenu.AddButton(gameObject, button);
		}

		private void Dig(int cell, float dt)
		{
			//float damageDone = WorldDamage.Instance.ApplyDamage(cell, 0.05f, cell);
			float damageBefore = Grid.Damage[cell];
			//Diggable.DoDigTick(cell, dt);

			float approximateDigTime = Diggable.GetApproximateDigTime(cell);
			float amount = dt / approximateDigTime;
			float damage = WorldDamage.Instance.ApplyDamage(cell, amount, -1);

			float complete = Diggable.GetDiggable(cell).GetPercentComplete();

			integrity -= damage; // Grid.Damage[cell] - damageBefore;
			hp.SetHitPoints((int)(integrity / maxIntegrity) * hp.MaxHitPoints);
			Trigger((int)GameHashes.BuildingReceivedDamage, hp);
			

			smi.sm.IntegrityParam.Set(integrity, smi);
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, Borer, object>.GameInstance
		{
			public StatesInstance(Borer smi) : base(smi)
			{

			}

			public void UpdateDig(Borer borer, float dt)
            {
				int cell = Grid.PosToCell(borer);
				int cellUnder = Grid.CellBelow(cell);

				if(!Grid.IsValidCell(cellUnder) || !Grid.AreCellsInSameWorld(cell, cellUnder))
                {
					smi.GoTo(smi.sm.broken);
                }
				else if(!IsCellDiggable(cellUnder))
                {
					smi.GoTo(smi.sm.stuck);
				}
				// dig self
				else if (Grid.IsSolidCell(cell))
				{
					smi.master.Dig(cell, dt);
				}
				// dig under
				else if(Grid.IsSolidCell(cellUnder))
                {
					smi.master.Dig(cellUnder, dt);
                }
				else
				{
					smi.GoTo(smi.sm.falling);
				}
            }

            private bool IsCellDiggable(int cell)
            {
				if(Grid.Element[cell].id == SimHashes.Unobtanium)
                {
					return false;
                }

				if(Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out GameObject go))
                {
					return !go.HasTag(GameTags.Bunker);
				}

				return true;
            }


            internal bool UpdateStuck(Borer master)
			{
				int cellUnder = Grid.CellBelow(Grid.PosToCell(master));
				return IsCellDiggable(cellUnder);
			}
        }

        public class States : GameStateMachine<States, StatesInstance, Borer>
		{
			public State waitingForFuel;
			public State idle;
			public State drilling;
			public State falling;
			public State stuck;
			public State broken;

			public Signal startBoring;

			public FloatParameter IntegrityParam;

			public override void InitializeStates(out BaseState defaultState)
			{
				defaultState = idle;

				root
					.ToggleStatusItem(ModAssets.StatusItems.integrity, smi => smi.master);

				drilling
					.ToggleStatusItem(ModAssets.StatusItems.drilling)
					.ParamTransition(IntegrityParam, broken, (smi, i) => i <= 0)
					.Update((smi, dt) => smi.UpdateDig(smi.master, dt), UpdateRate.SIM_200ms);

				falling
					.Enter(smi => GameComps.Fallers.Add(smi.gameObject, Vector3.down))
					.Exit(smi =>
                    {
						if(GameComps.Fallers.Has(smi.gameObject))
                        {
							GameComps.Fallers.Remove(smi.gameObject);
                        }
                    })
					.EventTransition(GameHashes.Landed, drilling)
					.ToggleStatusItem(ModAssets.StatusItems.falling);

				stuck
					.ToggleStatusItem(ModAssets.StatusItems.stuck)
					.UpdateTransition(drilling, (smi, dt) => smi.UpdateStuck(smi.master), UpdateRate.SIM_200ms);
			}
        }
    }
}
