using ImGuiNET;
using Moonlet.Entities.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Moonlet.MoonletDevTools
{
	// just a log filtered to only my mods messages
	public class ConsoleDevTool : DevTool
	{
		public const int LENGTH = 512;
		private static readonly Queue<LogEntry> logEntries = new();
		private static Color debugColor = new(0.7f, 0.7f, 0.7f);
		private static Color warningColor = Util.ColorFromHex("ee7539");
		private static Color commandColor = Util.ColorFromHex("ebc21e");
		private static string command = "";
		private static List<string> previousCommands = new List<string>();
		private static int previousCommandIdx;
		private static bool should_focus_search;
		private Commands commands;
		private static bool scrollDown;

		public ConsoleDevTool()
		{
			commands = new Commands();
			commands.Init();
		}

		public static void Log(string msg) => AddToLog(LogType.Info, msg);

		public static void AddToLog(LogType type, string msg)
		{
			if (msg == null)
				return;

			var date = $"[{System.DateTime.UtcNow:HH: mm: ss.fffffff}] ";

			logEntries.Enqueue(new LogEntry()
			{
				date = date,
				type = type,
				message = msg
			});

			if (logEntries.Count > LENGTH)
				logEntries.Dequeue();

			scrollDown = true;
		}

		public override void RenderTo(DevPanel panel)
		{
			ImGui.BeginChild("beached_logentries", new Vector2(700, 350), true, ImGuiWindowFlags.AlwaysVerticalScrollbar | ImGuiWindowFlags.AlwaysAutoResize);

			foreach (var entry in logEntries)
			{
				ImGui.TextColored(debugColor, entry.date);
				ImGui.SameLine();
				switch (entry.type)
				{
					case LogType.Debug:
						ImGui.TextColored(debugColor, entry.message);
						break;
					case LogType.Info:
						ImGui.Text(entry.message);
						break;
					case LogType.Warning:
						ImGui.TextColored(warningColor, entry.message);
						break;
					case LogType.CommandLine:
						ImGui.TextColored(commandColor, entry.message);
						break;
				}
			}

			if (scrollDown)
			{
				ImGui.SetScrollHereY();
			}

			ImGui.EndChild();

			ImGui.SetNextItemWidth(700);
			if (should_focus_search)
				ImGui.SetKeyboardFocusHere();

			if (ImGui.InputTextWithHint("##command", "use `help` for help", ref command, 512, ImGuiInputTextFlags.EnterReturnsTrue))
			{
				AddToLog(LogType.CommandLine, command);
				AddToLog(LogType.Debug, commands.Process(command));
				if (previousCommands.Count == 0 || previousCommands.Last() != command)
				{
					previousCommands.Add(command);
					previousCommandIdx = previousCommands.Count - 1;
				}

				command = "";
			}

			should_focus_search = false;

			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				previousCommandIdx--;
				previousCommandIdx = Mathf.Max(previousCommandIdx, 0);

				if(previousCommands.Count > 0)
				{
					command = previousCommands[previousCommandIdx];
					should_focus_search = true;
				}
			}

			CopyButton();
		}

		private static void CopyButton()
		{
			if (ImGui.Button("Copy"))
			{
				var msg = new StringBuilder();
				foreach (var entry in logEntries)
				{
					msg.AppendLine(entry.message);
				}

				ImGui.SetClipboardText(msg.ToString());
			}
		}

		struct LogEntry
		{
			public LogType type;
			public string message;
			public string date;
		}

		public enum LogType
		{
			Debug,
			Warning,
			Info,
			CommandLine
		}
	}
}
