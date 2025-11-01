using KSerialization;
using ProcGen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts;

internal class PandorasBox : KMonoBehaviour, ISidescreenButtonControl, ISim200ms
{
	[Serialize] private bool started;
	[Serialize] private float lifeTime;
	[SerializeField] public float maxLifeTime;

	public string SidescreenButtonText => STRINGS.ITEMS.AKISEXTRATWITCHEVENTS_PANDORASBOX.NAME;

	private static List<ElementOption> elementOptions =
	[
		new ElementOption(SimHashes.MoltenAluminum),
		new ElementOption(SimHashes.MoltenIron),
		new ElementOption(SimHashes.MoltenGlass),
		new ElementOption(SimHashes.MoltenGold),
		new ElementOption(SimHashes.MoltenLead),
		new ElementOption(SimHashes.MoltenUranium),
		new ElementOption(SimHashes.NuclearWaste, 600.0f),
		new ElementOption(SimHashes.Steam, 400.0f),
		new ElementOption(SimHashes.RockGas),
		new ElementOption(Elements.PinkSlime, mass: 10000),
		new ElementOption("ITCE_CreepyLiquid"),
		new ElementOption("ITCE_Inverse_Water", mass: 4000),
		new ElementOption("Beached_SulfurousWater")
	];

	public string SidescreenButtonTooltip => STRINGS.ITEMS.AKISEXTRATWITCHEVENTS_PANDORASBOX.DESC;

	public int HorizontalGroupID() => -1;

	public int ButtonSideScreenSortOrder() => 0;

	public void OnSidescreenButtonPressed()
	{
		Begin();
	}

	public struct ElementOption(string id, float temperature = -1, float mass = -1, float weight = 1.0f) : IWeighted
	{
		public float weight { get; set; } = weight;
		public string id = id;
		public float temperature = temperature;
		public float mass = mass;

		public ElementOption(SimHashes id, float temperature = -1, float mass = -1, float weight = 1.0f) : this(id.ToString(), temperature, mass, weight)
		{
		}
	}

	private void Begin()
	{
		elementOptions.RemoveAll(o => ElementLoader.FindElementByTag(o.id) == null);

		var isSevere = lifeTime > maxLifeTime;

		if (!isSevere)
		{
			MildEvent();
		}

		var cell = Grid.PosToCell(this);

		FloodWithDangerLiquidSmall(cell);

		started = true;
		if (CameraController.Instance != null && CameraController.Instance.followTarget == transform)
		{
			CameraController.Instance.ClearFollowTarget();
			CameraController.Instance.SetPosition(transform.position);
		}
	}

	private void FloodWithDangerLiquidSmall(int cell)
	{
		var elementOption = elementOptions.GetWeightedRandom();
		var element = ElementLoader.FindElementByTag(elementOption.id);

		Flood(
			cell,
			element,
			elementOption.temperature == -1 ? element.defaultValues.temperature : elementOption.temperature,
			elementOption.mass == -1 ? element.defaultValues.mass : elementOption.mass,
			5,
			this.GetMyWorldId());
	}

	private void Flood(int cell, Element element, float temperature, float mass, int tiles, int world)
	{
		var valid_cells = HashSetPool<int, PandorasBox>.Allocate();
		var visited_cells = HashSetPool<int, PandorasBox>.Allocate();
		var queue = QueuePool<GameUtil.FloodFillInfo, PandorasBox>.Allocate();

		queue.Enqueue(new GameUtil.FloodFillInfo()
		{
			cell = cell,
			depth = 0
		});

		bool condition(int cell2) => Grid.IsValidCellInWorld(cell2, world) && !Grid.Solid[cell2];

		GameUtil.FloodFillConditional(queue, condition, visited_cells, valid_cells, tiles);

		foreach (var gameCell in (HashSet<int>)valid_cells)
		{
			if (tiles > 0)
			{
				SimMessages.AddRemoveSubstance(gameCell, element.id, CellEventLogger.Instance.ElementEmitted, mass, temperature, byte.MaxValue, 0);
				--tiles;
			}
			else
				break;
		}
	}

	private void MildEvent()
	{
		StartCoroutine(SpawnGifts());
	}

	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

	public bool SidescreenButtonInteractable() => !started;

	public bool SidescreenEnabled() => !started;

	public override void OnSpawn()
	{
		if (started)
			Begin();
	}

	private void EvilFloow()
	{

	}

	private IEnumerator SpawnGifts()
	{
		GetComponent<KBatchedAnimController>().Play("open");
		var spawnCount = Random.Range(5, 11);

		var prefabPool = Assets.GetPrefabsWithTag(TTags.pandorasBoxSpawnableMild);

		var position = (transform.position + new Vector3(0, 0.5f)) with
		{
			z = Grid.GetLayerZ(Grid.SceneLayer.Move)
		};

		for (var idx = 0; idx < spawnCount; idx++)
		{
			var randPrefab = prefabPool.GetRandom();


			var go = GameUtil.KInstantiate(randPrefab, position, Grid.SceneLayer.Move);
			go.SetActive(true);

			FUtility.Utils.YeetRandomly(go, true, 1, 3, false);

			yield return new WaitForSeconds(Random.Range(0.75f, 2.0f));
		}

		Destroy(gameObject);
	}

	public void Sim200ms(float dt)
	{
		lifeTime += dt;

		if (!started && lifeTime > maxLifeTime)
			Begin();
	}
}