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
		[SerializeField] public Transform buttPrefab;
		[SerializeField] public int segmentCount;
		[SerializeField] public float breakInRadius;
		[SerializeField] public float crumbDistance;
		[SerializeField] public float segmentDistance;
		[SerializeField] public float noisePitch;
		[SerializeField] public float noiseVolume;
		[SerializeField] public float cameraShakeFactor;
		[SerializeField][Serialize] public int chaseEnergy;

		[Serialize] private bool hasBrokenIn;
		[Serialize] private int _worldIdx;

		private float _minimumMarkerLength;
		private List<BodyPart> _parts;
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
		protected bool _isLeaving;
		private float _burrowedDistance;
		private int lastVisibleSegment;
		private bool allPartsBurrowed;
		private float backupLeaveCounter = 10f;

		public WormHead()
		{
			lastVisibleSegment = -1;
		}

		private class BodyPart
		{
			public Transform transform;
			public bool appeared;
			public bool burrowed;
		}

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
			lastVisibleSegment = -1;
		}

		private void CreateBodyPart()
		{
			_parts ??= [];
			var isButt = _parts.Count == 0;

			_smokeParticles = transform.Find("DustEmitter").GetComponent<ParticleSystem>();
			_debrisParticles = transform.Find("DebrisEmitter").GetComponent<ParticleSystem>();

			var part = Instantiate(isButt ? buttPrefab : segmentPrefab);
			part.gameObject.SetActive(false);
			part.transform.SetPositionAndRotation(transform.position, transform.rotation);

			if (isButt)
			{
				_parts.Add(new BodyPart()
				{
					transform = part
				});
				_butt = part.transform;
			}
			else
				_parts.Insert(_parts.Count - 1, new BodyPart()
				{
					transform = part
				});
		}

		public override void OnSpawn()
		{
			_cameraShakeID = GetInstanceID();

			_worldIdx = this.GetMyWorldId();

			_z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 2f;
			_crumbs = [];
			_erraticNoise = new PerlinNoise(Random.Range(0, int.MaxValue));

			for (int i = 0; i < segmentCount; i++)
				CreateBodyPart();

			if (trackButt)
				_butt = _parts.Last().transform;

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
			soundEventInstance = KFMOD.BeginOneShot(soundEffect, gameObject.transform.GetPosition(), noiseVolume);
			soundEventInstance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
			soundEventInstance.setPitch(noisePitch);
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
			if (_isLeaving)
			{
				if (allPartsBurrowed)
					DestroySelf();

				return;
			}

			var soundPos = transform.position.To3DAttributes();
			soundEventInstance.set3DAttributes(soundPos);

			if (Vector2.Distance(_crumbs.Last().pos, transform.position) < crumbDistance)
				return;

			AddNewMarker();
			_crumbs.RemoveAt(0);

			if (cameraShakeFactor > 0)
			{
				var isInRange = smi.sm.IsPlayerInRange(smi, trackButt, 60f);
				if (isInRange)
				{
					var pow = Mathf.Lerp(0, cameraShakeFactor, Mathf.Clamp01(1f - (_distanceToMouse / 60f)));
					pow *= Mod.Settings.CameraShake;
					AkisTwitchEvents.Instance.SetCameraShake(_cameraShakeID, pow);
				}
				else
					AkisTwitchEvents.Instance.SetCameraShake(_cameraShakeID, 0);
			}
		}


		private bool CalcPosAtDistance(float headToFirstDistance, float totalDistance, out Vector3 position, out Quaternion rotation, out float distanceToPreviousPoint)
		{
			rotation = Quaternion.identity;
			position = transform.position;
			distanceToPreviousPoint = 999f;

			var lastCrumb = _crumbs.Last();

			var distanceTraversed = headToFirstDistance + _burrowedDistance;

			if (totalDistance < distanceTraversed)
			{
				float t = headToFirstDistance == 0 ? 0 : totalDistance / headToFirstDistance;
				position = Vector2.Lerp(transform.position, lastCrumb.pos, t);
				rotation = Quaternion.Lerp(transform.rotation, lastCrumb.rot, t);
				distanceToPreviousPoint = Mathf.Clamp01(1f - t);

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
					distanceToPreviousPoint = Mathf.Clamp01(1f - t);

					return true;
				}

				distanceTraversed += _crumbs[i].distance;
			}

			return false;
		}

		public void SimEveryTick(float dt)
		{
			age += dt;

			if (_isLeaving)
			{
				_burrowedDistance += _velocity.magnitude;

				backupLeaveCounter -= dt;

				if (backupLeaveCounter <= 0)
					DestroySelf();
				else
					UpdateBodyParts();

				return;
			}

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
			else if (!smi.IsInsideState(smi.sm.leaving))
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

			var pos = transform.position + _velocity;

			var angle = Vector2.SignedAngle(Vector3.up, _velocity);
			var rot = Quaternion.Euler(new Vector3(0, 0, angle));
			transform.SetPositionAndRotation(pos, rot);

			UpdateBodyParts();

			var cell = Grid.PosToCell(transform.position);
			if (!Grid.IsValidCell(cell) || Grid.WorldIdx[cell] != _worldIdx)
				GoAway();
		}

		private void UpdateBodyParts()
		{
			float headToFirst = Vector2.Distance(transform.position, _crumbs.Last().pos);

			for (int i = 0; i < _parts.Count; i++)
			{
				var part = _parts[i];

				if (part.burrowed)
					continue;

				var visible = CalcPosAtDistance(headToFirst, (i + 1) * segmentDistance, out var pos, out var rot, out var relativeDistanceToLastPart);

				var dist = relativeDistanceToLastPart * crumbDistance;
				Log.Debug($"distance: {relativeDistanceToLastPart}/{dist}");
				if (_isLeaving && i - 1 == lastVisibleSegment && dist < 0.1f)
				{
					part.transform.gameObject.SetActive(false);
					part.burrowed = true;
					lastVisibleSegment = i;

					if (lastVisibleSegment == _parts.Count - 1)
					{
						allPartsBurrowed = true;
						return;
					}
				}

				if (visible)
				{
					if (!part.appeared)
					{
						part.transform.gameObject.SetActive(true);
						part.appeared = true;
					}

					pos.Set(pos.x, pos.y, _z + (i * 0.001f));
					part.transform.SetPositionAndRotation(pos, rot);
				}
			}
		}

		public override void OnCleanUp()
		{
			if (soundEventInstance.isValid())
				SoundEvent.EndOneShot(soundEventInstance);

			// TODO: this is terrible, and technically a mini leak, but since only 1-2 per session should exist it's "fine".
			// for some reason the sound doesn't shut up by just ending the oneshot.

			if (soundEventInstance.isValid())
				soundEventInstance.setVolume(0f);
			AkisTwitchEvents.Instance.SetCameraShake(_cameraShakeID, 0);
		}

		private void DestroySelf()
		{
			foreach (var part in _parts)
			{
				if (!part.transform.IsNullOrDestroyed())
					Util.KDestroyGameObject(part.transform.gameObject);
			}

			if (soundEventInstance.isValid())
				SoundEvent.EndOneShot(soundEventInstance);

			Util.KDestroyGameObject(gameObject);
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
			ImGui.DragFloat("damping", ref solidDamping, 0.1f, 0, 10);
			ImGui.DragFloat("erraticMovementFactor", ref erraticMovementFactor, 0.1f, 0, 10);
			ImGui.DragFloat("playerDetectionRadius", ref playerDetectionRadius, 0.1f, 0, 10);
			ImGui.DragFloat("erraticFrequency", ref erraticFrequency, 0.1f, 0, 10);
			ImGui.DragFloat("speed", ref defaultSpeed, 0.1f, 0, 10);
			ImGui.DragFloat("mass", ref mass, 0.1f, 0, 10);
			ImGui.DragFloat("airSpeedMultiplier", ref airSpeedMultiplier, 0.1f, 0, 10);
			ImGui.DragFloat("airDamping", ref airDamping, 0.1f, 0, 10);
		}

		public void GoAway()
		{
			smi.GoTo(smi.sm.leaving);
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
					.UpdateTransition(meandering, (smi, _) => IsPlayerInRange(smi, false, smi.master.approachPlayerRadius) && CanSwitchMode(smi));

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
					.UpdateTransition(meandering, (smi, _) => !IsPlayerInRange(smi, false, smi.master.playerDetectionRadius + 5f) && CanSwitchMode(smi));

				leaving
					.Enter(smi =>
					{
						smi.master.GetComponent<SpriteRenderer>().enabled = false;
						smi.master._isLeaving = true;
					})
					.Exit(smi =>
					{
						smi.master._isLeaving = false;
					});
			}

			private bool CanChase(StatesInstance smi, float _)
			{
				return smi.master.chaseEnergy > 0
					&& IsPlayerInRange(smi, false, smi.master.playerDetectionRadius)
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

			public bool IsPlayerInRange(StatesInstance smi, bool checkButt, float distance)
			{
				Vector3 mousePos = PosUtil.ClampedMouseWorldPos();
				smi.master._distanceToMouse = Vector2.Distance(smi.transform.position, mousePos);

				if (checkButt && smi.master._butt != null)
				{
					var buttDistance = Vector2.Distance(smi.master._butt.position, mousePos);
					smi.master._distanceToMouse = Mathf.Min(smi.master._distanceToMouse, buttDistance);
				}

				return smi.master._distanceToMouse < distance;
			}
		}
	}
}
