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

		public void AddOnDestroyFn(EntityComponentFn command)
		{
			onDestroyActions ??= new();
			onDestroyActions.Add(command);
		}

		public override void OnCleanUp()
		{
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
