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

		public FInputField2 inputField;

		public LogEntry logEntryPrefab;
		public ScrollRect scrollBar;

		public List<LogEntry> logEntries = new();
		public static List<string> logStrings = new();

		private static DevConsoleScreen instance;

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

			foreach(var logStr in logStrings)
			{
				instance.AddLogEntry(logStr);
			}

			logStrings.Clear();
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			Helper.ListChildren(transform);

			var resizeCorner = transform.Find("ResizeHandle").gameObject.AddComponent<ResizerCorner>();
			resizeCorner.targetTransform = GetComponent<RectTransform>();

			var header = transform.Find("Header").gameObject.AddComponent<DraggableDialog>();
			header.targetTransform = GetComponent<RectTransform>();

			inputField = transform.Find("CommandInput").gameObject.AddComponent<FInputField2>();
			inputField.Text = "";

			scrollBar = transform.Find("Scroll View").GetComponent<ScrollRect>();

			logEntryPrefab = transform.Find("Scroll View/Viewport/Content/LogLine").gameObject.AddComponent<LogEntry>();
			logEntryPrefab.gameObject.SetActive(false);
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
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			inputField.inputField.onSubmit.AddListener(OnInputchanged);
		}

		private void OnInputchanged(string arg)
		{
			Log.Debug("input submitted");

			var result = DevConsole.ParseCommand(arg);

			if(!result.message.IsNullOrWhiteSpace())
			{
				AddLogEntry(result.message);
			}
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
