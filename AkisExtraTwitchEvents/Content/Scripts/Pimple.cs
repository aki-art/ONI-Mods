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
		[SerializeField] public float minDangerousLiquidMult;
		[SerializeField] public float maxDangerousLiquidMult;
		[SerializeField] public float durationSeconds;
		[SerializeField] public bool allowDangerousElements;

		[Serialize] public float mass;
		[Serialize] private SimHashes fluid;
		[Serialize] private bool initialized;
		[Serialize] private float starterScale;

		private float maxMass;
		private float massLossPerSecond;
		private static readonly float minSafeTemperature = GameUtil.GetTemperatureConvertedToKelvin(-40, GameUtil.TemperatureUnit.Celsius);
		private static readonly float maxSafeTemperature = GameUtil.GetTemperatureConvertedToKelvin(80, GameUtil.TemperatureUnit.Celsius);
		private static readonly float gasBombPreventTemp = GameUtil.GetTemperatureConvertedToKelvin(-20, GameUtil.TemperatureUnit.Celsius);

		public HashSet<SimHashes> elementOptions;

		public static HashSet<Tag> ignoredElementIds = [
			"ITCE_Inverse_Water_Placeholder",
			"ITCE_VoidLiquid",
		];

		public static HashSet<Tag> deadlyIds = [
			"ITCE_CreepyLiquid",
			"Beached_SulfurousWater"
		];

		private static string redColor;

		public static readonly Dictionary<SimHashes, (float maxMass, float temperature)> spawnOverrides = new()
		{
			{ SimHashes.ViscoGel, (100f, 300f) } // spawns frozen by default

			// lower gas amounts
		};

		public override void OnSpawn()
		{
			PopulateOptions();
			redColor ??= ((Color)GlobalAssets.Instance.colorSet.decorNegative).ToHexString();

			if (!initialized)
			{
				fluid = elementOptions.GetRandom();
				starterScale = kbac.animScale;
			}

			var element = ElementLoader.FindElementByHash(fluid);
			var isDangerous = IsDangerousElement(element);

			var defaultMass = GetElementDefaultMass(element);

			if (!initialized)
			{
				mass = isDangerous
					? defaultMass * Random.Range(minDangerousLiquidMult, maxDangerousLiquidMult)
					: defaultMass * Random.Range(minLiquidMult, maxLiquidMult);
			}

			maxMass = defaultMass * (isDangerous ? maxDangerousLiquidMult : maxLiquidMult);

			massLossPerSecond = maxMass / durationSeconds;

			var color = element.substance.colour with { a = byte.MaxValue };
			kbac.SetSymbolTint("juice", color);
			kbac.SetSymbolTint("splash", color);

			var name = string.Format(STRINGS.MISC.AKISEXTRATWITCHEVENTS_PIMPLE.NAME, element.tag.ProperNameStripLink());

			if (isDangerous)
			{
				name = $"<color=#{redColor}>{name}</color>";
			}

			name = $"{name}";

			smi.kSelectable.SetName(name);

			kbac.Offset = new Vector3(0, 0.5f);
			kbac.Play("idle");

			UpdateScale();

			Mod.pimples.Add(this);

			smi.StartSM();
			initialized = true;
		}

		private bool IsDangerousElement(Element element)
		{
			return element.lowTemp > maxSafeTemperature || element.highTemp < minSafeTemperature || deadlyIds.Contains(element.tag);
		}

		private float GetElementDefaultMass(Element element)
		{
			if (spawnOverrides.TryGetValue(element.id, out var overrideData))
				return overrideData.maxMass;

			var result = Mathf.Min(element.maxMass, element.defaultValues.mass);

			if (element.highTemp < gasBombPreventTemp)
				result /= 100f;

			result = Mathf.Clamp(result, 1f, 2000f);

			return result;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.pimples.Remove(this);
		}

		public void Sim33ms(float dt)
		{
			if (!initialized)
				return;

			mass -= massLossPerSecond * dt;

			if (mass < 0.002f)
			{
				CreatureHelpers.DeselectCreature(gameObject);
				Util.KDestroyGameObject(gameObject);
				return;
			}

			UpdateScale();
		}

		private void UpdateScale()
		{
			var relativeMass = mass / maxMass;
			var scale = Mathf.Lerp(0f, starterScale * 1.25f, relativeMass);

			kbac.animScale = scale;

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

			if (element.HasTag(TTags.useless))
				return false;

			if (ignoredElementIds.Contains(element.tag))
				return false;

			if (!allowDangerousElements)
			{
				if (IsDangerousElement(element))
					return false;
			}

			return true;
		}

		private void Burst()
		{
			CreatureHelpers.DeselectCreature(gameObject);
			smi.kSelectable.IsSelectable = false;

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
			public State burstPst;

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
					.EventHandler(GameHashes.AnimQueueComplete, Remove)
					.Enter(smi => smi.master.Burst());


			}

			private void Remove(StatesInstance smi)
			{
				// just in case
				CreatureHelpers.DeselectCreature(smi.gameObject);
				Util.KDestroyGameObject(smi.gameObject);
			}

			private bool IsHovered(StatesInstance _, object data) => (bool)data;
		}
	}
}
