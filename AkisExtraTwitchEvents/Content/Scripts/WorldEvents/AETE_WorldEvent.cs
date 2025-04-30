using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts.WorldEvents
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AETE_WorldEvent : KMonoBehaviour, ISaveLoadable
	{
		// TODO: serialize less, most of this can be inferred
		[SerializeField] public bool randomize = true;
		[SerializeField] public Dictionary<int, float> affectedCells;
		[Tooltip("This event is large scale and is exclusive to spawn with other big events on the same asteroid.")]
		[SerializeField] public bool bigEvent;

		public SchedulerHandle schedule;

		[SerializeField][Serialize] public bool immediateStart;
		[SerializeField][Serialize] public float power;
		[SerializeField][Serialize] public float durationInSeconds;
		[Serialize] public int radius;
		[Serialize] public bool showOnOverlay;
		[Serialize] public float elapsedTime;
		[Serialize] public WorldEventStage stage;

		public WorldEventStage Stage
		{
			get => stage; protected set => stage = value;
		}

		public float GetProgress01() => Mathf.Clamp01(elapsedTime / durationInSeconds);

		public float StartingIn => schedule.IsValid ? schedule.TimeRemaining : float.PositiveInfinity;

		public virtual float Power
		{
			get => power;
			set => power = value;
		}


		public float DurationInCycles
		{
			get => durationInSeconds / 600f;
			set => durationInSeconds = value * 600f;
		}

		private static readonly SimHashes crackedNeutronium = (SimHashes)Hash.SDBMLower("Beached_CrackedNeutronium");

		protected bool IsCrushable(Element element, out SimHashes crushed)
		{
			crushed = SimHashes.CrushedRock;

			switch (element.id)
			{
				case SimHashes.Ice:
					crushed = SimHashes.CrushedIce;
					return true;
				case SimHashes.CrushedIce:
					crushed = SimHashes.Snow;
					return true;
				case SimHashes.CrushedRock:
					crushed = SimHashes.Sand;
					return true;
				case SimHashes.Unobtanium:
					crushed = crackedNeutronium;
					return true;
				default:
					return element.HasTag(GameTags.Crushable) && element.hardness < 100;
			}
		}

		public float DamageTile(int cell, float inputDamage, bool crushing = false, float crushChance = 1f, bool spawnFX = true)
		{
			if (!Grid.IsValidCell(cell)
				|| Grid.Element[cell].id == SimHashes.Unobtanium) return 0;

			var gameObject = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];

			var damageMultiplier = 1f;

			if (gameObject != null)
			{
				if (gameObject.HasTag(GameTags.Window))
					damageMultiplier = 2f;
			}

			var element = Grid.Element[cell];

			if (crushing && IsCrushable(element, out SimHashes crushed) && Random.value <= crushChance)
			{
				SimMessages.ReplaceElement(cell, crushed, null, Grid.Mass[cell], Grid.Temperature[cell], Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell]);
				Game.Instance.SpawnFX(SpawnFXHashes.BuildingLeakGas, Grid.CellToPos(cell), 0f);
				return 0;
			}

			if (element.strength == 0f) return 0f;

			var damage = inputDamage * damageMultiplier / element.strength;

			PlayTileDamageSound(element, Grid.CellToPos(cell));
			Game.Instance.SpawnFX(SpawnFXHashes.BuildingLeakGas, Grid.CellToPos(cell), 0f);

			if (damage == 0f) return 0f;

			float dealtdamage;

			dealtdamage = WorldDamage.Instance.ApplyDamage(cell, damage, cell, "Sinkhole", "Sinkhole");

			return inputDamage * (1f - (float)(dealtdamage / damage));
		}

		private void PlayTileDamageSound(Element element, Vector3 pos)
		{
			string text = element.substance.GetMiningBreakSound();

			if (text == null)
			{
				if (element.HasTag(GameTags.RefinedMetal))
				{
					text = "RefinedMetal";
				}
				else if (element.HasTag(GameTags.Metal))
				{
					text = "RawMetal";
				}
				else
				{
					text = "Rock";
				}
			}

			text = GlobalAssets.GetSound("MeteorDamage_" + text);
			if (CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, text))
			{
				KFMOD.PlayOneShot(text, CameraController.Instance.GetVerticallyScaledPosition(pos), 0.7f);
			}
		}

		public virtual void Begin()
		{
			Stage = WorldEventStage.Active;
			schedule.ClearScheduler();
			AETE_WorldEventsManager.Instance.OnEventBegin(this);
		}

		public virtual void End()
		{
			Stage = WorldEventStage.Finished;
			AETE_WorldEventsManager.Instance.OnEventEnd(this);
		}

		public virtual void OnImguiDraw()
		{
		}

		protected virtual void Initialize()
		{
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			Initialize();
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			if (schedule.IsValid)
				schedule.ClearScheduler();
		}

		public enum WorldEventStage
		{
			Spawned,
			Active,
			Finished
		}
	}
}
