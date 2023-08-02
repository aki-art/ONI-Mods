using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class MoonletEntityComponent : KMonoBehaviour
	{
		[SerializeField] public List<EntityComponentFn> onDestroyActions;
		[Serialize] public Dictionary<string, object> serializedData;

		public delegate void EntityComponentFn(GameObject target);

		public MoonletEntityComponent()
		{
			serializedData = new Dictionary<string, object>();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (this.HasTag(MTags.DestroyWithoutFoundation))
				Subscribe((int)GameHashes.FoundationChanged, OnFoundationChanged);
		}

		private void OnFoundationChanged(object _)
		{
			if (this.HasTag(MTags.DestroyWithoutFoundation) && this.HasTag(GameTags.Creatures.HasNoFoundation))
				Util.KDestroyGameObject(this.gameObject);
		}

		public void AddOnDestroyFn(EntityComponentFn command)
		{
			onDestroyActions ??= new();
			onDestroyActions.Add(command);
		}

		public override void OnCleanUp()
		{
			Log.Debuglog("Destroying " + this.PrefabID());
			base.OnCleanUp();
			RunActions(onDestroyActions);
		}

		private void RunActions(List<EntityComponentFn> actions)
		{
			if (actions == null)
				return;

			foreach (var action in actions)
				action?.Invoke(gameObject);
		}
	}
}
