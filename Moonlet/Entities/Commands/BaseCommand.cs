using Moonlet.Content.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Entities.Commands
{
	public abstract class BaseCommand
	{
		public string DlcId { get; set; }

		public string[] ModIds { get; set; }

		public float Chance { get; set; }

		public abstract void Run(GameObject go);
	}
}
