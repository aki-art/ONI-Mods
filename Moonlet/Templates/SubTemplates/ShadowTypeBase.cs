using System;

namespace Moonlet.Templates.SubTemplates
{
	public abstract class ShadowTypeBase<T>
	{
		public abstract T Convert(Action<string> log = null);
	}
}
