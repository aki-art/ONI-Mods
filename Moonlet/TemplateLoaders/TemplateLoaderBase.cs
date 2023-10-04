using Moonlet.Templates;
using System;
using System.Reflection;
using UnityEngine;

namespace Moonlet.TemplateLoaders
{
	public abstract class TemplateLoaderBase
	{
		public string id;
		public string sourceMod;
		public bool isActive;
		public int priority;

		public abstract void RegisterTranslations();

		public virtual string GetTranslationKey(string partialKey) => partialKey;

		public virtual int GetPriority(string clusterId) => priority;

		public virtual void Validate()
		{

		}

		public void Warn(string message) => Log.Warn(message, sourceMod);

		public void Debug(string message) => Log.Debug(message, sourceMod);

		public void Info(string message) => Log.Info(message, sourceMod);

		public void Error(string message) => Log.Error(message, sourceMod);
	}

	/// <summary>
	/// Holds content loaded by a single mod
	/// </summary>
	/// <typeparam name="TemplateType">The template describing the YAML file</typeparam>
	public abstract class TemplateLoaderBase<TemplateType> : TemplateLoaderBase where TemplateType : ITemplate
	{
		public TemplateType template;

		public TemplateLoaderBase(TemplateType template)
		{
			this.template = template;
			isActive = true;
			id = template.Id;

			Validate();
		}

		public override void Validate()
		{
			foreach (var property in typeof(TemplateType).GetProperties())
			{
				foreach (var attribute in Attribute.GetCustomAttributes(property))
				{
					if (attribute is Utils.RangeAttribute range)
					{
						var propertyType = property.PropertyType;
						var val = property.GetValue(template);

						CheckRanges(property, range, propertyType, val);
					}
				}
			}
		}

		private void CheckRanges(PropertyInfo property, Utils.RangeAttribute range, Type propertyType, object val)
		{
			if (propertyType == typeof(float?))
			{
				var floatValue = (float?)val;

				if (floatValue.HasValue)
				{
					var newValue = Mathf.Clamp(floatValue.Value, range.min, range.max);
					property.SetValue(template, new float?(newValue));
				}
			}
			else if (propertyType == typeof(int?))
			{
				var intValue = (int?)val;

				if (intValue.HasValue)
				{
					var newValue = (int)Mathf.Clamp(intValue.Value, range.min, range.max);
					property.SetValue(template, new int?(newValue));
				}
			}
			else if (propertyType == typeof(float))
			{
				var floatValue = (float)val;
				var newValue = Mathf.Clamp(floatValue, range.min, range.max);
				property.SetValue(template, newValue);
			}
			else if (propertyType == typeof(int))
			{
				var intValue = (int)val;
				var newValue = (int)Mathf.Clamp(intValue, range.min, range.max);
				property.SetValue(template, newValue);
			}
		}

		public override int GetPriority(string clusterId)
		{
			if (clusterId != null
				&& template.PriorityPerCluster != null
				&& template.PriorityPerCluster.TryGetValue(clusterId, out var priority))
				return priority;

			return template.Priority;
		}
	}
}
