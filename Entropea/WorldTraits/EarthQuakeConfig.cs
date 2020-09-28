using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entropea.WorldTraits
{
    class EarthQuakeConfig : IEntityConfig
	{
		public static string ID = "WTP_EarthQuake";
		GameObject gameObject = EntityTemplates.CreateEntity(ID, STRINGS.WORLDS., true); B File Offset: 0x0000338B
	public void OnPrefabInit(GameObject go)
		{
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x0000518B File Offset: 0x0000338B
		public void OnSpawn(GameObject go)
		{
		}

		// Token: 0x04002357 RID: 9047
	}
}
