﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moonlet.Utils
{
	public class EnumUtils
	{
		//public static Dictionary<Type, EnumExtension<Enum>> extensions = [];

		public static T ParseOrHash<T>(string value, Dictionary<string, T> extraLookup = null, Action<string> logFn = null) where T : Enum
		{
			if (value.IsNullOrWhiteSpace())
				return (T)(object)0;

			try
			{
				return (T)Enum.Parse(typeof(T), value);
			}
			catch
			{
				if (extraLookup != null && extraLookup.TryGetValue(value, out var val))
					return val;

				return (T)(object)Hash.SDBMLower(value);
			}
		}

		public static T ParseOrDefault<T>(string value, T defaultValue = default, Dictionary<string, T> extraLookup = null, Action<string> logFn = null) where T : Enum
		{
			if (value.IsNullOrWhiteSpace())
				return defaultValue;

			try
			{
				return (T)Enum.Parse(typeof(T), value);
			}
			catch
			{
				if (extraLookup != null && extraLookup.TryGetValue(value, out var val))
					return val;

				var possibleOptions = Enum.GetNames(typeof(T)).ToList();

				if (extraLookup != null)
					possibleOptions.AddRange(extraLookup.Keys);

				logFn?.Invoke($"Invalid value \"{value}\" for {typeof(T)}, possible options are: {possibleOptions.Join()}");
				return defaultValue;
			}
		}
	}
}