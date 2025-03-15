using FMOD.Studio;
using FMODUnity;
using ImGuiNET;
using KSerialization;
using ONITwitchLib.Utils;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Twitchery.Content.Scripts.Worm
{
	public class WormHead : StateMachineComponent<WormHead.StatesInstance>, ISimEveryTick, ISim33ms, ISim200ms, IImguiDebug
	{
		[MyCmpReq] public KBatchedAnimController kbac;
		[MyCmpReq] private TileEater _tileEater;

		private EventInstance soundEventInstance;
		private string soundEffect;

		[Serialize] public float age;

		public static readonly HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

		[SerializeField] public float lifeTime;
		[SerializeField] public float mawRadius;
		[SerializeField] public float mawStrength;
		[SerializeField] public float playerDetectionRadius;
		[SerializeField] public Vector3 gravity = new(0, -1f);
		[SerializeField] public float defaultSpeed, chaseSpeed, maximumSpeed;
		[SerializeField] public float maxTurnSpeed;
		[SerializeField] public float erraticMovementFactor;
		[SerializeField] public float erraticFrequency;
		[SerializeField] public float solidDamping, airDamping;
		[SerializeField] public float airSpeedMultiplier;
		[SerializeField] public float mass;
		[SerializeField] public float approachPlayerRadius;
		[SerializeField] public bool trackButt;
		[SerializeField] public Transform segmentPrefab;
		[SerializeField] public int segmentCount;
		[SerializeField] public float breakInRadius;
		[SerializeField] public float crumbDistance;
		[SerializeField] public float segmentDistance;
		[SerializeField] public float cameraShakeFactor;
		[SerializeField][Serialize] public int chaseEnergy;

		[Serialize] private bool hasBrokenIn;

		private float _minimumMarkerLength;
		private List<Transform> _parts;
		private Vector3 _velocity;
		private Vector3 _targetPosition;
		private PerlinNoise _erraticNoise;
		private bool _chasingTarget;
		private bool _isInsideSolid;
		private List<Breadcrumb> _crumbs;
		private ParticleSystem _smokeParticles;
		private ParticleSystem _debrisParticles;
		private bool _wasInsideSolid;
		private float _z;
		private float _distanceToMouse;
		private int _cameraShakeID;
		private Transform _butt;

		private class Breadcrumb
		{
			public Vector2 pos;
			public Quaternion rot;
			public float distance;
		}

		public override void OnPrefabInit()
		{
			soundEffect = GlobalAssets.GetSound("meteor_lp");
			base.OnPrefabInit();
		}

		public override void OnCleanUp()
		{
			foreach (var part in _parts)
				Destroy(part.gameObject);

			SoundEvent.EndOneShot(soundEventInstance);
		}

		private void CreateBodyPart()
		{
			_parts ??= [];
			_smokeParticles = transform.Find("DustEmitter").GetComponent<ParticleSystem>();
			_debrisParticles = transform.Find("DebrisEmitter").GetComponent<ParticleSystem>();

			var part = Instantiate(segmentPrefab);
			part.gameObject.SetActive(false);
			part.transform.SetPositionAndRotation(transform.position, transform.rotation);
			_parts.Add(part);
		}

		public override void OnSpawn()
		{
			_cameraShakeID = GetInstanceID();

			_z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 2f;
			_crumbs = [];
			_erraticNoise = new PerlinNoise(Random.Range(0, int.MaxValue));

			for (int i = 0; i < segmentCount; i++)
				CreateBodyPart();

			if (trackButt)
				_butt = _parts.Last();

			StartLoopingSound();

			smi.StartSM();


			_minimumMarkerLength = (((segmentDistance * segmentCount)) / crumbDistance) + 2;
			for (int i = 0; i < _minimumMarkerLength; i++)
				AddNewMarker();

			UpdateSolidSubmersion(true);

			transform.position = transform.position with { z = _z };

			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);

			_tileEater.isActive = true;
		}

		private void StartLoopingSound()
		{
			soundEventInstance = KFMOD.BeginOneShot(soundEffect, gameObject.transform.GetPosition(), 2f);
			soundEventInstance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
			soundEventInstance.setPitch(0.7f);
			soundEventInstance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, 80f);
			if (soundEventInstance.isValid())
			{
				SoundEvent.EndOneShot(soundEventInstance);
			}
		}

		private void AddNewMarker()
		{
			var lastPos = _crumbs.Count > 0 ? _crumbs.Last().pos : (Vector2)transform.position;

			_crumbs.Add(new Breadcrumb()
			{
				pos = transform.position,
				rot = transform.rotation,
				distance = Vector2.Distance(transform.position, lastPos)
			});
		}

		private void OnFollowCam()
		{
			if (CameraController.Instance.followTarget == transform)
				CameraController.Instance.ClearFollowTarget();
			else
				CameraController.Instance.SetFollowTarget(transform);
		}

		private void OnRefreshUserMenu(object _)
		{
			Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo(
				"action_follow_cam",
				global::STRINGS.UI.USERMENUACTIONS.FOLLOWCAM.NAME,
				OnFollowCam,
				tooltipText: global::STRINGS.UI.USERMENUACTIONS.FOLLOWCAM.TOOLTIP), 0.3f);
		}

		public void Sim33ms(float dt)
		{
			var soundPos = transform.position.To3DAttributes();
			soundEventInstance.set3DAttributes(soundPos);

			if (Vector2.Distance(_crumbs.Last().pos, transform.position) < crumbDistance)
				return;

			AddNewMarker();
			_crumbs.RemoveAt(0);

			if (cameraShakeFactor > 0)
			{
				var pow = Mathf.Lerp(0, cameraShakeFactor, Mathf.Clamp01(1f - (_distanceToMouse / 60f)));
				AkisTwitchEvents.Instance.SetCameraShake(_cameraShakeID, pow);
			}
		}


		private bool CalcPosAtDistance(float headToFirstDistance, float totalDistance, out Vector3 position, out Quaternion rotation)
		{
			rotation = Quaternion.identity;
			position = transform.position;

			var lastCrumb = _crumbs.Last();

			var distanceTraversed = headToFirstDistance;

			if (totalDistance < distanceTraversed)
			{
				float t = headToFirstDistance == 0 ? 0 : totalDistance / headToFirstDistance;
				position = Vector2.Lerp(transform.position, lastCrumb.pos, t);
				rotation = Quaternion.Lerp(transform.rotation, lastCrumb.rot, t);

				return true;
			}

			for (int i = _crumbs.Count - 1; i > 0; i--)
			{
				var nextDist = distanceTraversed + _crumbs[i].distance;

				if (nextDist > totalDistance)
				{
					float remaining = nextDist - totalDistance;
					var t = remaining / _crumbs[i].distance;
					position = Vector2.Lerp(_crumbs[i - 1].pos, _crumbs[i].pos, t);
					rotation = Quaternion.Lerp(_crumbs[i - 1].rot, _crumbs[i].rot, t);

					return true;
				}

				distanceTraversed += _crumbs[i].distance;
			}

			return false;
		}

		public void SimEveryTick(float dt)
		{
			age += dt;

			var damping = _isInsideSolid ? solidDamping : airDamping;
			var maxSpeed = _chasingTarget ? chaseSpeed : maximumSpeed;

			if (!_isInsideSolid)
				maxSpeed *= airSpeedMultiplier;

			if (_chasingTarget)
			{
				Vector3 force = _targetPosition - transform.position;
				force.Normalize();
				force *= defaultSpeed;

				_velocity *= damping;
				_velocity += (mass * dt * force);
			}
			else
			{
				float time = age * erraticFrequency;

				float noiseX = (float)_erraticNoise.Noise(time, 0, 0);
				float noiseY = (float)_erraticNoise.Noise(0, time, 0);

				var desiredDirection = new Vector3(noiseX, noiseY) * erraticMovementFactor;

				_velocity *= damping;
				_velocity += (mass * dt * desiredDirection);
			}

			_velocity = Vector2.ClampMagnitude(_velocity, maxSpeed);

			if (!_isInsideSolid)
				_velocity += gravity * dt;

			transform.position += _velocity;

			var angle = Vector2.SignedAngle(Vector3.up, _velocity);
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

			UpdateBodyParts();
		}

		private void UpdateBodyParts()
		{
			float headToFirst = Vector2.Distance(transform.position, _crumbs.Last().pos);

			for (int i = 0; i < _parts.Count; i++)
			{
				var visible = CalcPosAtDistance(headToFirst, (i + 1) * segmentDistance, out var pos, out var rot);
				if (visible)
				{
					pos.Set(pos.x, pos.y, _z + (i * 0.001f));
					_parts[i].gameObject.SetActive(true);
					_parts[i].SetPositionAndRotation(pos, rot);
				}
			}
		}

		public void Sim200ms(float dt)
		{
			if (age > lifeTime)
				smi.GoTo(smi.sm.leaving);

			if (_chasingTarget)
				_targetPosition = PosUtil.ClampedMouseWorldPos();

			UpdateSolidSubmersion(false);
		}

		private void UpdateSolidSubmersion(bool forceParticleUpdate)
		{
			var cell = Grid.PosToCell(this);

			if (Grid.IsValidCell(cell))
			{
				_isInsideSolid = !GridUtil.IsCellEmpty(cell);
				if (_isInsideSolid)
					WorldDamage.Instance.ApplyDamage(cell, mawStrength, -1);

				if (_isInsideSolid != _wasInsideSolid || forceParticleUpdate)
				{
					var emission = _smokeParticles.emission;
					emission.enabled = _isInsideSolid;
					var emission2 = _debrisParticles.emission;
					emission2.enabled = _isInsideSolid;

					_wasInsideSolid = _isInsideSolid;
				}
			}
		}

		private int renderQueue = 4503;
		private Material headMat;

		public void OnImgui()
		{
			ImGui.DragFloat("z", ref _z, 0.1f, 0, -60);

			if (ImGui.DragInt("renderQueue", ref renderQueue, 1, 3500, 5000))
			{
				headMat ??= GetComponent<SpriteRenderer>().material;
				headMat.renderQueue = renderQueue;
			}

			ImGui.Separator();

			ImGui.DragFloat("maxTurnSpeed", ref maxTurnSpeed, 0.1f, 0, 1);
			ImGui.DragFloat("maxSpeed", ref maximumSpeed, 0.1f, 0, 10);
			ImGui.DragFloat("lifeTime", ref lifeTime, 0.1f, 0, 600);
			ImGui.DragFloat("mawRadius", ref mawRadius, 0.1f, 0, 1);
			ImGui.DragFloat("mawStrength", ref mawStrength, 0.1f, 0, 1);
			ImGui.DragFloat("damping", ref solidDamping, 0.1f, 0, 10);
			ImGui.DragFloat("erraticMovementFactor", ref erraticMovementFactor, 0.1f, 0, 10);
			ImGui.DragFloat("playerDetectionRadius", ref playerDetectionRadius, 0.1f, 0, 10);
			ImGui.DragFloat("erraticFrequency", ref erraticFrequency, 0.1f, 0, 10);
			ImGui.DragFloat("speed", ref defaultSpeed, 0.1f, 0, 10);
			ImGui.DragFloat("mass", ref mass, 0.1f, 0, 10);
			ImGui.DragFloat("airSpeedMultiplier", ref airSpeedMultiplier, 0.1f, 0, 10);
			ImGui.DragFloat("airDamping", ref airDamping, 0.1f, 0, 10);
		}

		public class StatesInstance(WormHead master) : GameStateMachine<States, StatesInstance, WormHead, object>.GameInstance(master)
		{
			public float modeSwitchCooldown;
		}

		public class States : GameStateMachine<States, StatesInstance, WormHead>
		{
			public State appearing;
			public State approaching;
			public State meandering;
			public State chasing;
			public State leaving;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = appearing;

				root
					.Update((smi, dt) => smi.modeSwitchCooldown += dt);

				appearing
					.Enter(BustBackwallIn)
					.GoTo(approaching);

				approaching
					.Enter(smi =>
					{
						smi.modeSwitchCooldown = 0;
						smi.master._chasingTarget = true;
					})
					.UpdateTransition(meandering, (smi, _) => IsPlayerInRange(smi, smi.master.approachPlayerRadius) && CanSwitchMode(smi));

				meandering
					.Enter(smi =>
					{
						smi.modeSwitchCooldown = 0;
						smi.master._chasingTarget = false;
					})
					.UpdateTransition(chasing, CanChase);

				chasing
					.Enter(smi =>
					{
						smi.modeSwitchCooldown = 0;
						smi.master._chasingTarget = true;
						smi.master.chaseEnergy--;
					})
					.UpdateTransition(meandering, (smi, _) => !IsPlayerInRange(smi, smi.master.playerDetectionRadius + 5f) && CanSwitchMode(smi));
			}

			private bool CanChase(StatesInstance smi, float _)
			{
				return smi.master.chaseEnergy > 0
					&& IsPlayerInRange(smi, smi.master.playerDetectionRadius)
					&& CanSwitchMode(smi);
			}

			private void BustBackwallIn(StatesInstance smi)
			{
				if (smi.master.hasBrokenIn)
					return;

				BustWall(smi);

				smi.master.hasBrokenIn = true;
			}

			private static void BustWall(StatesInstance smi)
			{
				if (smi.master.breakInRadius > 0)
				{
					var cells = ProcGen.Util.GetFilledCircle(smi.transform.position, smi.master.breakInRadius);

					foreach (var pos in cells)
					{
						var cell = Grid.PosToCell(pos);
						if (AGridUtil.Destroyable(cell, false))
						{
							AGridUtil.DestroyTile(cell);
							AGridUtil.Vacuum(cell);
							AkisTwitchEvents.Instance.AddZoneTypeOverride(cell, ProcGen.SubWorld.ZoneType.Space);
						}
					}
				}
			}

			private bool CanSwitchMode(StatesInstance smi) => smi.modeSwitchCooldown > 10f;

			private bool IsPlayerInRange(StatesInstance smi, float distance)
			{
				Vector3 mousePos = PosUtil.ClampedMouseWorldPos();
				smi.master._distanceToMouse = Vector2.Distance(smi.transform.position, mousePos);

				if (smi.master.trackButt && smi.master._butt != null)
				{
					var buttDistance = Vector2.Distance(smi.master._butt.position, mousePos);
					smi.master._distanceToMouse = Mathf.Min(smi.master._distanceToMouse, buttDistance);
				}

				return smi.master._distanceToMouse < distance;
			}
		}
	}
}
