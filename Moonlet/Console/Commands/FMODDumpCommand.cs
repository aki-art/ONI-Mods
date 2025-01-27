using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Moonlet.Console.Commands
{
	public class FMODDumpCommand() : BaseConsoleCommand("fmoddump")
	{
		public override void SetupArguments()
		{
			expectedArgumentVariations = [[
					new ArgumentInfo("kanim", "list events tied to an animation. Use * to dump all events.", true)
					]];
		}

		public override CommandResult Run()
		{
			var stringBuilder = new StringBuilder();

			if (GetStringArgument(0, out var target))
			{
				if (target == "*")
				{
					foreach (var soundEvent in KFMOD.soundDescriptions)
					{
						stringBuilder.Append($"{Assets.GetSimpleSoundEventName(soundEvent.Value.path)} \t\tfull event path: {soundEvent.Value.path}");
					}
				}
				else
				{
					var sheets = GameAudioSheets.Get().sheets;
					var hasEntry = false;

					foreach (var sheet in sheets)
					{
						foreach (var info in sheet.soundInfos)
						{
							stringBuilder.AppendLine($"Type: {info.Type}");

							if (info.File == target)
							{
								hasEntry = true;
								if (!info.Name0.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name0}, \t\tFrame: {info.Frame0}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name1.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name1}, \t\tFrame: {info.Frame1}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name2.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name2}, \t\tFrame: {info.Frame2}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name3.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name3}, \t\tFrame: {info.Frame3}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name4.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name4}, \t\tFrame: {info.Frame4}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name5.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name5}, \t\tFrame: {info.Frame5}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name6.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name6}, \t\tFrame: {info.Frame6}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name7.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name7}, \t\tFrame: {info.Frame7}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name8.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name8}, \t\tFrame: {info.Frame8}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name9.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name9}, \t\tFrame: {info.Frame9}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name10.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name10}, \t\tFrame: {info.Frame10}, \t\tminInterval: {info.MinInterval}");
								if (!info.Name11.IsNullOrWhiteSpace())
									stringBuilder.AppendLine($"\t- {info.Name11}, \t\tFrame: {info.Frame11}, \t\tminInterval: {info.MinInterval}");
							}
						}
					}

					if (!hasEntry)
						return CommandResult.Success("No sound events for this animation.");
				}
			}

			try
			{
				var path = Path.Combine(FUtility.Utils.ModPath, "sounds.txt");
				File.WriteAllText(path, stringBuilder.ToString());
				Application.OpenURL(path);

				return CommandResult.Success($"Saved file to {path}");
			}
			catch (Exception e)
			{
				return CommandResult.Error("Could not write file. " + e.Message);
			}
		}
	}
}
