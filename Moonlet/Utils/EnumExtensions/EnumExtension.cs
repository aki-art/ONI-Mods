/*using System;
using System.Collections.Generic;

namespace Moonlet.Utils.EnumExtensions
{
	public abstract class EnumExtension
	{
		public abstract EnumType Parse<EnumType>(string value) where EnumType : struct, Enum;


	}

	public class EnumExtension<EnumType>(Dictionary<string, EnumType> lookup) where EnumType : struct, Enum
	{
		private readonly Dictionary<string, EnumType> lookup = lookup;

		public EnumType Parse(string value)
		{
			try
			{
				var result = (EnumType)Enum.Parse(typeof(EnumType), value); // using Parse instead of TryParse because mods tend to only patch Parse
				return result;
			}
			catch
			{
				return ParseAdditional(value, out var result) ? result : default;
			}
		}

		protected virtual bool ParseAdditional(string value, out EnumType result)
		{
			return lookup.TryGetValue(value, out result);
		}
	}
}
*/