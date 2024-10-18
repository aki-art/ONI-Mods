using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Moonlet.Console.Commands
{
	public class DumpIdsCommand() : BaseConsoleCommand("dumpids")
	{
		public override CommandResult Run()
		{
			var db = Db.Get();
			var buffer = new StringBuilder();
			var contentType = argumentStrs[1].ToLowerInvariant();
			bool success = false;

			if (contentType == "resource")
			{
				foreach (var resourceType in db.ResourceTable)
				{
					if (resourceType is ResourceSet)
					{
						buffer.Append(resourceType.Name);
						buffer.Append(", ");
						buffer.Append(resourceType.Id);
						buffer.AppendLine();
					}
				}

				success = true;
			}
			else if (contentType == "element")
			{
				foreach (var element in ElementLoader.elements)
				{
					buffer.Append(element.tag.ProperNameStripLink());
					buffer.Append(", ");
					buffer.Append(element.id);
					buffer.AppendLine();
				}

				success = true;
			}
			else if (contentType == "building")
			{
				foreach (var building in Assets.BuildingDefs)
				{
					buffer.Append(Util.StripTextFormatting(building.Name));
					buffer.Append(", ");
					buffer.Append(building.PrefabID);
					buffer.AppendLine();
				}

				success = true;
			}
			else if (contentType == "prefab")
			{
				foreach (var building in Assets.Prefabs)
				{
					buffer.Append(Util.StripTextFormatting(building.GetProperName()));
					buffer.Append(", ");
					buffer.Append(building.PrefabID());
					buffer.AppendLine();
				}

				success = true;
			}
			else
			{
				foreach (var resourceType in db.ResourceTable)
				{
					Log.Debug(resourceType.Id);
					Log.Debug(resourceType.Name);
					Log.Debug(resourceType.GetType());
					if (resourceType.Id.ToLowerInvariant() == argumentStrs[1] && resourceType is ResourceSet set)
					{
						for (int i = 0; i < set.Count; i++)
						{
							var resource = set.GetResource(i);
							if (resource != null)
							{
								buffer.Append(resource.Name);
								buffer.Append(", ");
								buffer.Append(resource.Id);
								buffer.AppendLine();
							}
						}

						success = true;
						break;
					}
				}
			}

			if (!success)
				return CommandResult.Error("No content type with " + argumentStrs[1]);

			var path = argumentStrs.Length > 2 ? argumentStrs[2] : FUtility.Utils.ModPath;

			try
			{
				File.WriteAllText(Path.Combine(path, $"{argumentStrs[1]}_ids.txt"), buffer.ToString());
			}
			catch (DirectoryNotFoundException ex)
			{
				return CommandResult.Error("Directory not found. " + ex.Message);
			}
			catch (IOException ex)
			{
				return CommandResult.Error("IO Error. " + ex.Message);
			}
			catch (UnauthorizedAccessException ex)
			{
				return CommandResult.Error("No permission to write file. " + ex.Message);
			}
			catch (Exception ex)
			{
				return CommandResult.Error("Something went wrong. " + ex.Message);
			}

			Application.OpenURL(path);

			return CommandResult.success;
		}
	}
}
