using FUtility.FUI;
using Moonlet.Console;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Database.MonumentPartResource;

namespace Moonlet.Scripts.UI
{
	public class DevConsoleScreen : KScreen
	{
		public const float SCREEN_SORT_KEY = 99999;
		new bool ConsumeMouseScroll = true; // do not remove!!!!

		private bool shown = false;
		public bool pause = true;

		public ConsoleInputField inputField;

		public LogEntry logEntryPrefab;
		public ScrollRect scrollBar;

		public FButton closeButton;

		public List<LogEntry> logEntries = new();
		public static List<string> logStrings = new();

		private VerticalLayoutGroup layout;
		private static DevConsoleScreen instance;

		private int commandPosition;

		public static DevConsoleScreen Instance
		{
			get
			{
				if (instance == null)
					Initialize(ModAssets.Prefabs.devConsolePrefab);

				return instance;
			}
		}

		public static void Initialize(GameObject prefab)
		{
			if (prefab == null)
			{
				Log.Error("No console prefab.");
				return;
			}

			if (GameScreenManager.Instance == null)
				return;

			var parent = GameScreenManager.Instance.ssOverlayCanvas;// Helper.GetACanvas("moonlet console").transform;

			var gameObject = Instantiate(prefab, parent.transform);
			gameObject.transform.position = new Vector2(Screen.width / 2f, Screen.height / 2f);

			instance = gameObject.AddComponent<DevConsoleScreen>();

			instance.ConsumeMouseScroll = false;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			Helper.ListChildren(transform);

			var resizeCorner = transform.Find("ResizeHandle").gameObject.AddComponent<ResizerCorner>();
			resizeCorner.targetTransform = GetComponent<RectTransform>();

			var header = transform.Find("Header").gameObject.AddComponent<DraggableDialog>();
			header.targetTransform = GetComponent<RectTransform>();

			inputField = transform.Find("CommandInput").gameObject.AddComponent<ConsoleInputField>();
			inputField.Text = "";

			scrollBar = transform.Find("Scroll View").GetComponent<ScrollRect>();

			layout = transform.Find("Scroll View/Viewport/Content").gameObject.GetComponent<VerticalLayoutGroup>();

			logEntryPrefab = layout.transform.Find("LogLine").gameObject.AddComponent<LogEntry>();
			logEntryPrefab.gameObject.SetActive(false);
			logEntryPrefab.text.font = ModAssets.Fonts.simplyMono;


			closeButton = transform.Find("Header/Close").gameObject.AddOrGet<FButton>();
		}

		public static void AddLogStr(string message) => logStrings.Add(message);

		public void AddLogEntry(string text)
		{
			// TODO
			var newEntry = Instantiate(logEntryPrefab);
			newEntry.gameObject.SetActive(true);
			newEntry.SetText(text);
			newEntry.transform.SetParent(logEntryPrefab.transform.parent);

			logEntries.Add(newEntry);

			scrollBar.verticalScrollbar.value = 1f;

			newEntry.gameObject.SetActive(false);
			newEntry.gameObject.SetActive(true);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			inputField.onSubmit += OnSubmit;
			closeButton.OnClick += Deactivate;

			inputField.onSelectUp += Up;
		}

		private void Up()
		{
			commandPosition++;
			var position = Mathf.Min(0, DevConsole.commandHistory.Count - commandPosition);

			if (DevConsole.commandHistory.Count > 0)
			{
				inputField.Text = DevConsole.commandHistory[position];
			}
		}

		private void OnSubmit(string arg)
		{
			if (arg.IsNullOrWhiteSpace())
				return;

			DevConsole.Log(arg, "#888888");

			commandPosition = 0;

			var result = DevConsole.ParseCommand(arg);

			if(!result.message.IsNullOrWhiteSpace())
				AddLogEntry(result.message);
		}

		public override void OnCmpEnable()
		{
			base.OnCmpEnable();
			if (CameraController.Instance != null)
				CameraController.Instance.DisableUserCameraControl = true;
		}

		public override void OnCmpDisable()
		{
			base.OnCmpDisable();

			if (CameraController.Instance != null)
				CameraController.Instance.DisableUserCameraControl = false;

			Trigger((int)GameHashes.Close, null);
		}

		public override bool IsModal() => true;

		public override float GetSortKey() => SCREEN_SORT_KEY;

		public override void OnActivate() => OnShow(true);

		public override void OnDeactivate() => OnShow(false);

		public override void OnShow(bool show)
		{
			base.OnShow(show);
			if (pause && SpeedControlScreen.Instance != null)
			{
				if (show && !shown)
				{
					SpeedControlScreen.Instance.Pause(false);
				}
				else
				{
					if (!show && shown)
						SpeedControlScreen.Instance.Unpause(false);
				}

				shown = show;
			}
		}

		public override void OnKeyDown(KButtonEvent e)
		{
			if (e.TryConsume(Action.Escape))
			{
				Deactivate();
			}
			else
			{
				base.OnKeyDown(e);
			}
		}

		public override void OnKeyUp(KButtonEvent e)
		{
			if (!e.Consumed)
			{
				var scrollRect = GetComponentInChildren<KScrollRect>();
				if (scrollRect != null)
					scrollRect.OnKeyUp(e);
			}

			e.Consumed = true;
		}
	}
}
