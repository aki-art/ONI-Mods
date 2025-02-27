using System;
using System.Collections.Generic;

namespace Moonlet.Registry
{
	public class ComplexFeatureDefinition
	{
		public string id;
		public Action<WorldContainer, int, Dictionary<string, dynamic>> onPlaceFn;

		public virtual void OnPlaceFeature(WorldContainer world, int originCell, Dictionary<string, dynamic> data)
		{
			onPlaceFn?.Invoke(world, originCell, data);
		}

		public void CreateFromType(Type type)
		{
			var methodInfo = type.GetMethod(nameof(OnPlaceFeature),
			[
				typeof(WorldContainer),
				typeof(int),
				typeof(Dictionary<string, dynamic>)
			]);

			if (methodInfo != null)
			{
				onPlaceFn = (Action<WorldContainer, int, Dictionary<string, dynamic>>)Delegate.CreateDelegate(typeof(Action<WorldContainer, int, Dictionary<string, dynamic>>), methodInfo);
			}
		}
	}
}
