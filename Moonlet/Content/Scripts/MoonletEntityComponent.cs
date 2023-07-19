using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Moonlet.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class MoonletEntityComponent : KMonoBehaviour
	{
		[SerializeField] [SerializeReference] public List<ActionData> onSpawnActions;
		[SerializeField] public string test;
		[Serialize] public Dictionary<string, object> serializedData;

		public delegate bool EntityComponentFn(MoonletEntityComponent target, Dictionary<string, object> serializedData);

		public override void OnSpawn()
		{
			base.OnSpawn();
			Log.Debuglog("Run Actions" + name);
			RunActions(onSpawnActions);
		}

		public void AddOnSpawnFn(EntityComponentFn action, int priority = 0)
		{
			Log.Debuglog("Add on spawn fn to " + name);
			onSpawnActions ??= new();

			var item = new ActionData()
			{
				priority = priority
			};

			//item.Fn += action;

			onSpawnActions.Add(item);
		}

		private void RunActions(List<ActionData> actions)
		{
			Log.Debuglog($"tesst: {test}");
			Log.Assert("actions", actions);
			Log.Assert("onSpawnActions", onSpawnActions);
			if (actions == null)
				return;

			actions.OrderByDescending(a => a.priority);

			foreach (var action in actions)
			{
/*				if (action.Run(this, serializedData))
					return;*/
			}
		}

		[Serializable]
		public class ActionData
		{
			//public event EntityComponentFn Fn;
			[SerializeField] public int priority;

			/// <returns> Return true to stop execution of further actions </returns>
/*			public bool Run(MoonletEntityComponent target, Dictionary<string, object> serializedData)
			{
				if (Fn == null)
					return false;

				return Fn(target, serializedData);
			}*/
		}
	}
}
