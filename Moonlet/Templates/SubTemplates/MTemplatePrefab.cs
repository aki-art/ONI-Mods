extern alias YamlDotNetButNew;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using TemplateClasses;
using YamlDotNetButNew.YamlDotNet.Serialization;
using static TemplateClasses.Prefab;

namespace Moonlet.Templates.SubTemplates
{
	public class MTemplatePrefab : ShadowTypeBase<Prefab>
	{
		public string Id { get; set; }

		[YamlMember(Alias = "location_x", ApplyNamingConventions = false)] // Klei inconsitent name
		public int LocationX { get; set; }

		[YamlMember(Alias = "location_y", ApplyNamingConventions = false)] // Klei inconsitent name
		public int LocationY { get; set; }

		public string Element { get; set; }

		public float Temperature { get; set; }

		public float Units { get; set; }

		public string DiseaseName { get; set; }

		public int DiseaseCount { get; set; }

		public Orientation RotationOrientation { get; set; }

		public List<StorageItem> Storage { get; set; }

		public Prefab.Type Type { get; set; }

		public int Connections { get; set; }

		public Rottable Rottable { get; set; }

		public List<TemplateFloatData> Amounts { get; set; }

		[YamlMember(Alias = "other_values", ApplyNamingConventions = false)]
		public List<TemplateFloatData> OtherValues { get; set; }

		public MTemplatePrefab()
		{
		}
		public override Prefab Convert(Action<string> log = null)
		{
			return new Prefab(Id,
				Type,
				LocationX,
				LocationY,
				ElementUtil.GetSimhashSafe(Element), // TODO
				Temperature,
				Units,
				DiseaseName,
				DiseaseCount,
				RotationOrientation,
				ShadowTypeUtil.CopyList<template_amount_value, TemplateFloatData>(Amounts, log)?.ToArray(),
				ShadowTypeUtil.CopyList<template_amount_value, TemplateFloatData>(OtherValues, log)?.ToArray(),
				Connections);
		}

		public class TemplateFloatData : ShadowTypeBase<template_amount_value>
		{
			public string Id { get; set; }

			public FloatNumber Value { get; set; }

			public override template_amount_value Convert(Action<string> log = null) => new template_amount_value(Id, Value.CalculateOrDefault(0));
		}

		public class TemplateStringData
		{
			public string Id { get; set; }

			public string Value { get; set; }

			public int AsInt()
			{
				var expression = new IntNumber();
				expression.SetExpression(Value);
				return expression.CalculateOrDefault(0);
			}

			public float AsFloat()
			{
				var expression = new FloatNumber();
				expression.SetExpression(Value);
				return expression.CalculateOrDefault(0f);
			}
		}
	}
}
