using System;

namespace Moonlet.Templates.SubTemplates
{
	public interface IShadowTypeBase<T>
	{
		public T Convert(Action<string> log = null);
	}
}
