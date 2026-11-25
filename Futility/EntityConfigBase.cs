using System;
using UnityEngine;

namespace FUtility
{
	public abstract class EntityConfigBase : IEntityConfig
	{
		public abstract GameObject CreatePrefab();

		public virtual void OnPrefabInit(GameObject inst)
		{
		}

		public virtual void OnSpawn(GameObject inst)
		{
		}

		[Obsolete]
		public virtual string[] GetDlcIds() => null;

		protected GameObject CreateBasicTemporary(string ID, string animFile = "barbeque_kanim")
		{
			var prefab = EntityTemplates.CreateEntity(ID, ID, false);

			prefab.AddComponent<StateMachineController>();

			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles = new[] { global::Assets.GetAnim("barbeque_kanim") };
			kbac.initialAnim = "none";
			kbac.initialMode = KAnim.PlayMode.Paused;
			kbac.SetVisiblity(false);

			return prefab;
		}

		protected GameObject CreateBasic(string ID, string animFile = "barbeque_kanim")
		{
			var prefab = CreateBasicTemporary(ID, animFile);
			prefab.AddOrGet<SaveLoadRoot>();

			return prefab;
		}
	}
}
