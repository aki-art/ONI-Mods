using KSerialization;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Events.EventTypes;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Pimple : StateMachineComponent<Pimple.StatesInstance>, ISim33ms
	{
		[MyCmpReq] public KBatchedAnimController kbac;

		[SerializeField] public float minLiquidMult;
		[SerializeField] public float maxLiquidMult;
		[SerializeField] public float liquidLossPerSecond;

		[Serialize] private float mass;
		[Serialize] private SimHashes fluid;
		[Serialize] private bool initialized;

		private float maxMass;
		private float starterScale;

		public static HashSet<SimHashes> elementOptions;

		public static HashSet<Tag> ignoredElementIds = [
			"ITCE_Inverse_Water_Placeholder",
			"ITCE_VoidLiquid",
		];

		public static HashSet<Tag> ignoredOnDeadlyIds = [
			"ITCE_CreepyLiquid",
			"Beached_SulfurousWater"
		];

		public static readonly Dictionary<SimHashes, (float mass, float temperature)> spawnOverrides = new()
		{
			{ SimHashes.ViscoGel, (100f, 300f) } // spawns frozen by default
		};

		public override void OnSpawn()
		{
			if (elementOptions == null)
				PopulateOptions();

			if (!initialized)
			{
				fluid = elementOptions.GetRandom();
				mass = -1;
				starterScale = kbac.animScale;
				initialized = true;
			}


			var element = ElementLoader.FindElementByHash(fluid);

			if (mass == -1)
				mass = GetMass(element) * Random.Range(minLiquidMult, maxLiquidMult);

			maxMass = GetMass(element) * maxLiquidMult;

			var color = element.substance.colour with { a = byte.MaxValue };
			kbac.SetSymbolTint("juice", color);
			kbac.SetSymbolTint("splash", color);

			var name = string.Format(STRINGS.MISC.AKISEXTRATWITCHEVENTS_PIMPLE.NAME, element.tag.ProperName());
			smi.kSelectable.SetName(name);

			kbac.Offset = new Vector3(0, 0.5f);

			UpdateScale();

			Mod.pimples.Add(this);

			smi.StartSM();
		}

		private float GetMass(Element element) => spawnOverrides.TryGetValue(element.id, out var overrideData) ? overrideData.mass : element.maxMass;

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.pimples.Remove(this);
		}

		public void Sim33ms(float dt)
		{
			if (!initialized)
				return;

			mass -= liquidLossPerSecond * dt;

			if (mass < 0.002f)
			{
				Util.KDestroyGameObject(gameObject);
				return;
			}

			UpdateScale();
		}

		private void UpdateScale()
		{
			var relativeMass = mass / maxMass;
			kbac.animScale = relativeMass * starterScale * 1.25f;

			kbac.SetDirty();
			kbac.UpdateAnim(0);
		}

		private void PopulateOptions()
		{
			elementOptions = ElementLoader.elements
				.Where(IsValidElement)
				.Select(e => e.id)
				.ToHashSet();

		}

		private bool IsValidElement(Element element)
		{
			if (!element.IsLiquid)
				return false;

			if (ignoredElementIds.Contains(element.tag))
				return false;

			switch (AkisTwitchEvents.MaxDanger)
			{
				case < ONITwitchLib.Danger.Extreme:
					if (element.lowTemp > 1200)
						return false;
					break;
				case < ONITwitchLib.Danger.Deadly:
					if (element.lowTemp > 570 || ignoredOnDeadlyIds.Contains(element.tag))
						return false;
					break;
			}

			return true;
		}

		private void Burst()
		{
			var volume = Mathf.Lerp(0.3f, 1.2f, Mathf.Clamp01(mass / maxMass)) * 8f;

			AudioUtil.PlaySound(ModAssets.Sounds.PLOP_SOUNDS.GetRandom(), transform.position, volume * ModAssets.GetSFXVolume());

			var element = ElementLoader.FindElementByHash(fluid);

			var temperature = spawnOverrides.TryGetValue(fluid, out var overrideData) ? overrideData.temperature : element.defaultValues.temperature;

			SimMessages.ReplaceAndDisplaceElement(Grid.PosToCell(this), fluid, RockPaperScissorsEvent.spawnEvent, mass, temperature);
		}

		public class StatesInstance(Pimple master) : GameStateMachine<States, StatesInstance, Pimple, object>.GameInstance(master)
		{
			public KSelectable kSelectable = master.GetComponent<KSelectable>();
		}

		public class States : GameStateMachine<States, StatesInstance, Pimple>
		{
			public State idle;
			public State hovered;
			public State hoveredOff;
			public State bursting;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				root
					.EventTransition(GameHashes.SelectObject, bursting);

				idle
					.EventHandlerTransition(GameHashes.HighlightObject, hovered, IsHovered);

				hovered
					.PlayAnim("hover_pre", KAnim.PlayMode.Once)
					.EventHandlerTransition(GameHashes.HighlightObject, hoveredOff, (smi, data) => !IsHovered(smi, data));

				hoveredOff
					.PlayAnim("hover_pst")
					.EventTransition(GameHashes.AnimQueueComplete, idle);

				bursting
					.PlayAnim("burst")
					.Enter(smi => smi.master.Burst());

			}

			private bool IsHovered(StatesInstance _, object data) => (bool)data;
		}
	}
}
