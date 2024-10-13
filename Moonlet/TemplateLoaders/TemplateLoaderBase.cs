using Moonlet.Templates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using YamlDotNet.Serialization.Utilities;

namespace Moonlet.TemplateLoaders
{
	public abstract class TemplateLoaderBase
	{
		public string id;
		public string sourceMod;
		public bool isActive;
		public bool isValid;
		public int priority;
		public string relativePath;
		public string path;
		public bool usePathAsId;
		public bool isOverridingVanillaContent;

		public abstract void RegisterTranslations();

		public virtual string GetTranslationKey(string partialKey) => partialKey;

		public virtual int GetPriority(List<string> clusterTags) => priority;

		public virtual void Initialize()
		{

		}

		public virtual void Validate()
		{

		}

		public void Issue(string message) => Log.Warn($"Issue with {path} {id}: {message}", sourceMod);

		public void Warn(string message) => Log.Warn(message, sourceMod);

		public void Debug(object message) => Log.Debug(message?.ToString(), sourceMod);

		public void Info(string message) => Log.Info(message, sourceMod);

		public void Error(string message) => Log.Error(message, sourceMod);

		public void AddString(string key, string value)
		{
			if (value == null)
			{
				Log.Debug($"Missing translation: {key}");
				value = "MISSING.";
			}

			if (!value.StartsWith("STRINGS."))
				Mod.translationsLoader.Add(sourceMod, key, value);
		}
	}

	public interface IMergeable
	{
		public void MergeInto(IMergeable other);
	}

	/// <summary>
	/// Holds content loaded by a single mod
	/// </summary>
	/// <typeparam name="TemplateType">The template describing the YAML file</typeparam>
	public abstract class TemplateLoaderBase<TemplateType> : TemplateLoaderBase, IMergeable where TemplateType : class, ITemplate
	{
		public TemplateType template;

		public string FormatAsLink(string text, string id = null)
		{
			text = global::STRINGS.UI.StripLinkFormatting(text);

			if (id.IsNullOrWhiteSpace())
			{
				id = text;
				id = id.Replace(" ", "");
			}

			id = id.ToUpperInvariant();
			id = id.Replace("_", "");

			return $"<link=\"{id}\">{text}</link>";
		}

		public TemplateLoaderBase(TemplateType template, string sourceMod)
		{
			this.template = template;
			this.sourceMod = sourceMod;
			id = template.Id;

			isValid = template != null;
			isActive = true;
		}


		public OriginalType CopyProperties<OriginalType>(bool forceNull = false, bool log = false) where OriginalType : class, new()
		{
			if (template == null)
			{
				Log.Warn($"Cannot convert {path} template to type, template is null.");
				return null;
			}

			var result = Activator.CreateInstance(typeof(OriginalType));

			var targetProperties = typeof(OriginalType).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

			var templateType = typeof(TemplateType);

			foreach (var originalProperty in targetProperties)
			{
				if (log)
					Log.Debug("original property: " + originalProperty.Name);

				var templateProperty = templateType.GetProperty(originalProperty.Name.ToPascalCase());

				if (log)
					Log.Debug("expected property: " + originalProperty.Name.ToPascalCase());

				if (templateProperty != null && templateProperty.PropertyType == originalProperty.PropertyType)
				{
					var templateValue = templateProperty.GetValue(template);
					if (templateValue != null || forceNull)
					{
						originalProperty.SetValue(result, templateValue);
						if (log)
							Log.Debug($"Copied field {templateProperty.Name} to {originalProperty.Name}");
					}
				}
			}

			return (OriginalType)result;
		}

		public override void Initialize()
		{
			Validate();

			if (isValid)
			{
				RegisterTranslations();
				Log.Debug($"Created template {sourceMod}: {id}");
			}
		}

		public override void Validate()
		{
			if (template.Id.IsNullOrWhiteSpace())
			{
				Warn("Template must have an ID defined!");
				isValid = false;
				return;
			}

			if (!DlcManager.IsDlcListValidForCurrentContent(template.GetDlcIds()))
			{
				isValid = false;
				return;
			}

			foreach (var mod in template.GetModIds())
			{
				if (!Mod.loadedModIds.Contains(mod))
				{
					isValid = false;
					return;
				}
			}

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

		private bool GetHighestPriority(List<string> clusterTags, out int priority)
		{
			priority = 0;

			if (clusterTags == null)
				return false;

			if (template.PriorityPerClusterTag == null)
				return false;

			var result = 0;
			var hasClusterDefined = false;

			foreach (var clusterTag in clusterTags)
			{
				if (template.PriorityPerClusterTag.TryGetValue(clusterTag, out var clusterPriority))
				{
					if (int.TryParse(clusterPriority, out var clusterPriorityNum))
						result = Mathf.Max(result, clusterPriorityNum);

					hasClusterDefined = true;
				}
			}

			return hasClusterDefined;
		}

		public override int GetPriority(List<string> clusterTags)
		{
			GetHighestPriority(clusterTags, out var clusterPriority);
			int.TryParse(template.Priority, out var basePriority);

			return Mathf.Max(basePriority, clusterPriority);
		}

		public virtual void MergeInto(IMergeable other)
		{
			if (other.GetType() != GetType())
				return;

			var to = (template as TemplateLoaderBase<TemplateType>).template;
			var from = template;

			var targetProperties = typeof(TemplateType)
				.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

			var templateType = typeof(TemplateType);

			foreach (var originalProperty in targetProperties)
			{
				var templateProperty = templateType.GetProperty(originalProperty.Name);

				if (templateProperty != null && templateProperty.PropertyType == originalProperty.PropertyType)
				{
					var templateValue = templateProperty.GetValue(from);

					if (templateValue != null)
						originalProperty.SetValue(to, templateValue);
				}
			}
		}
	}
}
