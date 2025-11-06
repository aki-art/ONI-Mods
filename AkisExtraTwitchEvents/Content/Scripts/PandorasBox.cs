using ImGuiNET;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Defs.Critters;
using Twitchery.Content.Events.EventTypes.PandoraSubEvents;
using UnityEngine;

namespace Twitchery.Content.Scripts;

public class PandorasBox : KMonoBehaviour, ISidescreenButtonControl, ISim200ms, IRenderEveryTick, IImguiDebug
{
	[Serialize] public bool started;
	[Serialize] public float lifeTime;
	[SerializeField] public float maxLifeTime;
	[SerializeField] public float frequencyMin, frequencyMax, shakeSpeed, amplitude;


	[MyCmpReq] public KBatchedAnimController kbac;
	[MyCmpReq] public KSelectable kSelectable;

	private List<PandoraEventBase> _pandoraEvents;
	private PandoraFireworksEvent _fireworks;
	private PandoraRewardEvent _rewards;
	private System.Guid _statusItem = System.Guid.Empty;
	private float _frequency, _shakeElapsed;
	private float _shakePower;

	public string SidescreenButtonText => STRINGS.ITEMS.AKISEXTRATWITCHEVENTS_PANDORASBOX.NAME;

	public string SidescreenButtonTooltip => STRINGS.ITEMS.AKISEXTRATWITCHEVENTS_PANDORASBOX.DESC;

	public int HorizontalGroupID() => -1;

	public int ButtonSideScreenSortOrder() => 0;

	public void OnSidescreenButtonPressed()
	{
		Begin();
	}

	private void InitEvents()
	{
		_fireworks = new PandoraFireworksEvent(0.0f, 40, 70, 12, SpeedControlScreen.Instance.speed == 3 ? 30f : 10f);

		_rewards = new PandoraRewardEvent(0.0f, 10f);

		_pandoraEvents = [
			new PandoraDangerousLiquidFloodEvent(1.0f).ExtraMean(),
			new PandoraExplodeEvent(1.0f).ExtraMean(),
			new PandoraShrapnelEvent(1.0f, 10.0f),
			new PandoraBigWormEvent(0.5f).ExtraMean(),
			new PandoraEntitySpamEvent(1.0f,  TinyAngryCrabConfig.ID, 20, 30),
			new PandoraEntitySpamEvent(1.0f,  GlomConfig.ID, 10, 20),
			_fireworks,
			_rewards
		];

		if (DlcManager.IsExpansion1Active() && DlcManager.FeatureRadiationEnabled())
		{
			_pandoraEvents.Add(new PandoraEntitySpamEvent(1.0f, BeeConfig.ID, 8, 16).ExtraMean());
		}

		if (DlcManager.IsContentSubscribed(DlcManager.DLC4_ID))
		{
			_pandoraEvents.Add(new PandoraEntitySpamEvent(1.0f, MosquitoConfig.ID, 10, 20));
		}
	}

	private void Begin()
	{
		if (_pandoraEvents == null)
			InitEvents();

		_shakePower = 0.0f;

		if (GameComps.Fallers.Has(gameObject))
			GameComps.Fallers.Remove(gameObject);

		GetComponent<KBatchedAnimController>().Play("open");

		var isSevere = lifeTime > maxLifeTime;

		var eventCount = isSevere ? 3 : 1;

		var currentEvents = new List<PandoraEventBase>();

		var potentialEvents = isSevere ? _pandoraEvents : _pandoraEvents.Where(ev => !ev.extraMean).ToList();

		if (potentialEvents.Count == 0)
		{
			Log.Debug("no potential events");
		}

		for (var i = 0; i < eventCount; i++)
		{
			var ev = potentialEvents.GetWeightedRandom();
			currentEvents.Add(ev);
		}

		Log.Debug("starting pandora. events: " + currentEvents.Count);
		foreach (var ev in currentEvents)
		{
			Log.Debug(ev.GetType().Name);
			ev.Run(this);
		}

		_fireworks.Run(this);
		_rewards.Run(this);

		started = true;
		kbac.Offset = Vector3.zero;

		if (CameraController.Instance != null && CameraController.Instance.followTarget == transform)
		{
			CameraController.Instance.ClearFollowTarget();
			CameraController.Instance.SetPosition(transform.position);
		}
	}

	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

	public bool SidescreenButtonInteractable() => !started;

	public bool SidescreenEnabled() => !started;

	public override void OnSpawn()
	{
		if (started)
		{
			Begin();
		}
	}

	public float TimeRemaining => maxLifeTime - lifeTime;

	public void Sim200ms(float dt)
	{
		lifeTime += dt;

		if (!started)
		{
			if (lifeTime > maxLifeTime)
			{
				Log.Debug("time is up, beginning now");
				Begin();
			}
			/*
						if (TimeRemaining < 120f && _statusItem == System.Guid.Empty)
						{
							Log.Debug("adding status utem");
							_statusItem = kSelectable.AddStatusItem(TStatusItems.PandoraImminent);
						}*/

			return;
		}

		if (_pandoraEvents == null)
			InitEvents();

		var anyOngoingEvents = false;
		foreach (var ev in _pandoraEvents)
		{
			if (ev == null)
			{
				Log.Warning("null ev");
				continue;
			}

			if (ev.isActive)
				ev.Tick(dt);

			if (ev.isActive && !ev.IsFinished)
				anyOngoingEvents = true;
		}

		if (!anyOngoingEvents)
		{
			Log.Debug("destroying self");
			Destroy(gameObject);
		}
	}

	private Vector2 _targetOffset = Vector2.zero;

	public void RenderEveryTick(float dt)
	{
		if (started)
			return;

		var originalPosition = kbac.Offset;

		_shakeElapsed += dt;

		_shakePower = Mathf.Clamp01((lifeTime / maxLifeTime));

		if (_shakeElapsed > _frequency)
		{
			_frequency = Random.Range(frequencyMin, frequencyMax);
			_shakeElapsed = 0;
			_targetOffset = Random.insideUnitCircle * amplitude * _shakePower;
		}

		var offset = Vector3.MoveTowards(originalPosition, _targetOffset, shakeSpeed);

		_shakeElapsed += dt;
		kbac.Offset = offset;
	}

	public void OnImgui()
	{
		ImGui.DragFloat("Pandoras box shake Frequency Min", ref frequencyMin);
		ImGui.SameLine();
		ImGui.DragFloat("Pandoras box shake Frequency Max", ref frequencyMax);

		ImGui.DragFloat("Pandoras box shake Amplitude", ref amplitude);
		ImGui.DragFloat("Pandoras box shake Speed", ref shakeSpeed);

		if (started)
		{
			Log.Debug("currently running events:");
			foreach (var ev in _pandoraEvents)
			{
				if (ev.isActive && !ev.IsFinished)
				{
					ImGui.Text(ev.GetType().Name);
				}
			}
		}

		if (ImGui.Button("Reset offset"))
		{
			kbac.Offset = Vector3.zero;
			_shakeElapsed = 999f;
		}

		if (_pandoraEvents != null)
		{
			foreach (var ev in _pandoraEvents)
			{
				if (ImGui.Button(ev.GetType().Name))
					ev.Run(this);
			}
		}
		else
		{
			if (ImGui.Button("Init Pandora Events"))
				InitEvents();
		}
	}

	public string GetStatusString(string str)
	{
		return str.Replace("{Time}", GameUtil.GetFormattedTime(TimeRemaining));
	}
}
