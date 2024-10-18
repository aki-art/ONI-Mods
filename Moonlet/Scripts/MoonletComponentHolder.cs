using KSerialization;
using Moonlet.Scripts.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class MoonletComponentHolder : KMonoBehaviour
	{
		[MyCmpReq] private KPrefabID kPrefabID;
		[MyCmpReq] private KBatchedAnimController kbac;

		[SerializeField] public bool addToSandboxMenu;
		[SerializeField] public event EntityComponentFn OnDestroyFn;
		[SerializeField][Serialize] public KAnim.PlayMode initialModeOverride;
		[SerializeField] public bool destroyOnFoundationLost;
		[SerializeField] public string[] initialAnimOverrideOptions;

		[Serialize] private Dictionary<string, object> serializedData;
		[Serialize] private string initialAnimOverride;

		[Serialize] public string sourceMod;

		public delegate void EntityComponentFn(object target);

		public static class EventIds
		{
			public static readonly HashedString
				onDestroy = "OnDestroy",
				onSpawn = "OnSpawn",
				onTimer = "OnTimer";
		}

		public void SetData(string key, object data)
		{
			serializedData ??= [];
			serializedData[key] = data;
		}

		public bool TryGetData(string key, out object data)
		{
			data = default;
			return serializedData != null && serializedData.TryGetValue(key, out data);
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			UpdateInitialAnim();

			if (destroyOnFoundationLost)
				Subscribe((int)GameHashes.FoundationChanged, OnFoundationChanged);
		}

		private void OnFoundationChanged(object obj)
		{
			if (kPrefabID.HasTag(GameTags.Creatures.HasNoFoundation))
				Util.KDestroyGameObject(gameObject);
		}

		public override void OnCleanUp()
		{
			OnDestroyFn?.Invoke(gameObject);
			base.OnCleanUp();
		}

		public void OnPrefabSpawn(BaseCommand command)
		{
			var hash = new HashedString(command.At);

			var timers = new Dictionary<HashedString, Moonlet_Timer>();

			foreach (var timer in GetComponents<Moonlet_Timer>())
				timers.Add(timer.hash, timer);

			if (hash == EventIds.onDestroy)
				OnDestroyFn += command.Run;

			else if (hash == EventIds.onSpawn)
				command.Run(gameObject);

			else if (timers.TryGetValue(hash, out var timer))
				timer.OnTriggerFn += command.Run;

			else
				Subscribe(hash.HashValue, command.Run);
		}

		public void UpdateInitialAnim()
		{
			if (!initialAnimOverride.IsNullOrWhiteSpace())
			{
				kbac.initialAnim = initialAnimOverride;
				kbac.initialMode = initialModeOverride;

				kbac.Play(initialAnimOverride, initialModeOverride);

				return;
			}

			if (initialAnimOverrideOptions != null && initialAnimOverrideOptions.Length > 0)
			{
				if (initialAnimOverride.IsNullOrWhiteSpace())
					initialAnimOverride = initialAnimOverrideOptions.GetRandom();

				kbac.initialAnim = initialAnimOverride;
				kbac.initialMode = initialModeOverride;

				kbac.Play(initialAnimOverride, initialModeOverride);
			}
		}

		public struct Timer(HashedString id, float seconds)
		{
			public HashedString id = id;
			public float seconds = seconds;
		}
	}
}
