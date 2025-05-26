using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace KayosTech.Utilities.DebugTools
{
    /// <summary>
    /// Listens to log events and visually displays them via prefab instantiation.
    /// </summary>
    public class UILogDispatcher : MonoBehaviour
    {
        public static Action<bool> OnLogDisplaySync;

        [Header("UI Components")] 
        [SerializeField] private UILogIcon logIcon;
        [SerializeField] private UILogSettings logSettings;

        [Header("Message Prefabs")] 
        [SerializeField] private UILogMessage infoMessagePrefab;
        [SerializeField] private UILogMessage successMessagePrefab;
        [SerializeField] private UILogMessage alertMessagePrefab;
        [SerializeField] private UILogMessage errorMessagePrefab;

        [Header("Parent Object")] 
        [SerializeField] private Transform messageParent;

        [Header("Log Display")] 
        public bool displayLogs = true;
        public bool displayInfoLog = true;
        public bool displaySuccessLog = true;
        public bool displayAlertLog = true;
        public bool displayErrorLog = true;


        [Header("Log Display Times")]
        public float infoTimer = 4;
        public float successTimer = 3;
        public float alertTimer = 5;
        public float errorTimer = 6;


        public void ToggleDisplayLogs(bool display)
        {
            Debug.Log("LogButton Clicked");
            displayLogs = display;
            logIcon.SetNewColor(displayLogs);
        }

        private void OnEnable()
        {
            UILogIcon.OnClicked += () => ToggleDisplayLogs(!displayLogs);
            LogRouter.OnLogReceived += DisplayLog;


            OnLogDisplaySync += (value) =>
            {
                string state = value ? "active" : "inactive";
                LogRouter.Log("UIConfig", $"All Log Displays are set to {value}");
            };
        }

        private void OnDisable()
        {
            LogRouter.OnLogReceived -= DisplayLog;
        }

        public void ToggleSaveToFile(bool saveLog)
        {
            LogRouter.Log("UIConfig", $"Save to file set to {saveLog}");
            LogRouter.SaveToFile = saveLog;
        }

        private void DisplayLog(LogEntry log)
        {
            if (displayLogs == false) return;

            float timer = log.Level switch
            {
                LogLevel.Info => infoTimer,
                LogLevel.Success => successTimer,
                LogLevel.Alert => alertTimer,
                LogLevel.Error => errorTimer,
                _ => 3.0f
            };

            log.DisplayTime = timer;

            var newMsg = log.Level switch
            {
                LogLevel.Internal => null,
                LogLevel.Info =>  displayInfoLog 
                    ? Instantiate(infoMessagePrefab, messageParent) 
                    : null,
                LogLevel.Success => displaySuccessLog 
                    ? Instantiate(successMessagePrefab, messageParent) 
                    : null,
                LogLevel.Alert => displayAlertLog 
                    ? Instantiate(alertMessagePrefab, messageParent)
                    : null,
                LogLevel.Error => displayErrorLog 
                    ? Instantiate(errorMessagePrefab, messageParent) 
                    : null,
                _ => HandleUnknownLevel(log)
            };

            if (newMsg == null) return;

            newMsg.Initialize(log);

            //Force layout rebuild so it positions properly on first message
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)messageParent);

        }

        private static UILogMessage HandleUnknownLevel(LogEntry log)
        {
            LogRouter.Log("LogHandler", $"Unknown log level '{log.Level}' encountered. This log will not be shown in the UI.");
            return null;
        }
    }
}
