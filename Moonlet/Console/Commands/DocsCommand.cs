using Moonlet.DocGen;
using System;
using System.IO;
using UnityEngine;

namespace Moonlet.Console.Commands
{
	public class DocsCommand() : BaseConsoleCommand("docs")
	{
		public override CommandResult Run()
		{
			var docs = new Docs();
			var docsPath = Path.Combine(FUtility.Utils.ModPath, "docs");
			var templatePath = Path.Combine(docsPath, "template.html");

			var errors = "";

			try
			{
				docs.Generate(Path.Combine(docsPath, "pages"), templatePath);
			}
			catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
			{
				errors = $"{e.GetType().Name}: Can not write file to location. Either no authorization, disk is full, or some other issue is preventing the mod from writing to disk. {e.Message}";
			}
			catch (Exception e)
			{
				errors = $"{e.GetType().Name} {e.Message}";
			}

			var indexPath = Path.Combine(docsPath, "index.html");
			if (File.Exists(indexPath))
				Application.OpenURL(indexPath);

			return errors.IsNullOrWhiteSpace()
				? CommandResult.Success(templatePath)
				: CommandResult.Error("Could not generate docs. " + errors);
		}
	}
}
