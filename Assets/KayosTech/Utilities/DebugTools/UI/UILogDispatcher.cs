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
        public static Action<bool> AdjustAllLogs;

        [Header("UI Components")] 
        [SerializeField] private UILogSettings logSettings;

        [Header("Message Prefabs")] 
        [SerializeField] private UILogMessage infoMessagePrefab;
        [SerializeField] private UILogMessage successMessagePrefab;
        [SerializeField] private UILogMessage alertMessagePrefab;
        [SerializeField] private UILogMessage errorMessagePrefab;

        [Header("Parent Object")] 
        [SerializeField] private Transform messageParent;

        [Header("Log Display")] 
        public bool displayAllLogs = true;
        public bool displayInfoLog = true;
        public bool displaySuccessLog = true;
        public bool displayAlertLog = true;
        public bool displayErrorLog = true;

        [Header("Log Display Times")]
        public float infoTimer = 4;
        public float successTimer = 3;
        public float alertTimer = 5;
        public float errorTimer = 6;

        private void Awake()
        {
            

            UILogSettings.OnToggleInfoLog += (isActive) => HandleToggleAdjustment(LogLevel.Info, isActive);
            UILogSettings.OnToggleSuccessLog += (isActive) => HandleToggleAdjustment(LogLevel.Success, isActive);
            UILogSettings.OnToggleAlertLog += (isActive) => HandleToggleAdjustment(LogLevel.Alert, isActive);
            UILogSettings.OnToggleErrorLog += (isActive) => HandleToggleAdjustment(LogLevel.Error, isActive);

            UILogSettings.OnToggleAllLogs += ToggleDisplayAllLogs;

            UILogSettings.OnAdjustInfoDisplayTime += (displayTime) => HandleLogTimeAdjustment(LogLevel.Info, displayTime);
            UILogSettings.OnAdjustSuccessDisplayTime += (displayTime) => HandleLogTimeAdjustment(LogLevel.Success, displayTime);
            UILogSettings.OnAdjustAlertDisplayTime += (displayTime) => HandleLogTimeAdjustment(LogLevel.Alert, displayTime);
            UILogSettings.OnAdjustErrorDisplayTime += (displayTime) => HandleLogTimeAdjustment(LogLevel.Error, displayTime);

            UILogSettings.OnTestInfoLog += (message) => HandleTestLog(LogLevel.Info, message);
            UILogSettings.OnTestSuccessLog += (message) => HandleTestLog(LogLevel.Success, message);
            UILogSettings.OnTestAlertLog += (message) => HandleTestLog(LogLevel.Alert, message);
            UILogSettings.OnTestErrorLog += (message) => HandleTestLog(LogLevel.Error, message);
            UILogSettings.OnTestAllLogs += HandleTestAllLog;

            UILogSettings.OnToggleSaveToFile += ToggleSaveToFile;
            UILogSettings.OnClearLogCache += HandleClearLogCache;

        }

        private void ToggleDisplayAllLogs()
        {
            bool newState = !displayAllLogs;
            string stateLabel = newState ? "ENABLED" : "DISABLED";


            //Temporarily allow UI log through 
            displayAllLogs = true;
            LogRouter.Log("UIConfig", $"All Log Displays are <size=110%><b>{stateLabel}</b></size>", LogLevel.ByPass);

            displayAllLogs = newState;
            AdjustAllLogs?.Invoke(displayAllLogs);
        }

        private void OnEnable()
        {
            LogRouter.OnLogReceived += DisplayLog;
        }

        private void OnDisable()
        {
            LogRouter.OnLogReceived -= DisplayLog;
        }


        #region UI Log Settings Logic

        private void HandleToggleAdjustment(LogLevel level, bool isActive)
        {
            string displayStatus = isActive ? "enabled" : "disabled";
            LogRouter.Log("UIConfig", $"{level} log display set to {displayStatus}");

            switch (level)
            {
                case LogLevel.Internal:
                case LogLevel.Info:
                    displayInfoLog = isActive;
                    break;
                case LogLevel.Success:
                    displaySuccessLog = isActive;
                    break;
                case LogLevel.Alert:
                    displayAlertLog = isActive;
                    break;
                case LogLevel.Error:
                    displayErrorLog = isActive;
                    break;
            }

        }

        private void HandleLogTimeAdjustment(LogLevel level, float value)
        {
            switch (level)
            {
                case LogLevel.Internal:
                case LogLevel.Info:
                    infoTimer = value;
                    break;
                case LogLevel.Success:
                    successTimer = value;
                    break;
                case LogLevel.Alert:
                    alertTimer = value;
                    break;
                case LogLevel.Error:
                    errorTimer = value;
                    break;
            }
        }

        private void HandleTestLog(LogLevel level, string message)
        {
            LogRouter.Log("UIConfig", message,level);
        }

        private void HandleTestAllLog()
        {
            LogLevel[] logLevels = new[] { LogLevel.Info, LogLevel.Success, LogLevel.Alert, LogLevel.Error };

            foreach (LogLevel level in logLevels)
            {
                LogRouter.Log("UIConfig", $"Test {level} Log", level);
            }
        }
        private void ToggleSaveToFile(bool saveLog)
        {
            string cacheLog = saveLog ? "ENABLED" : "DISABLED";

            LogRouter.Log("UIConfig", $"Cache Logs is <size=110%><b>{cacheLog}</b></size>", LogLevel.Info);
            LogRouter.SaveToFile = saveLog;
        }

        private void HandleClearLogCache()
        {
            LogRouter.ClearLogCache();
        }
        #endregion


        private void DisplayLog(LogEntry log)
        {
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
                LogLevel.Info =>  displayInfoLog && displayAllLogs
                    ? Instantiate(infoMessagePrefab, messageParent) 
                    : null,
                LogLevel.Success => displaySuccessLog && displayAllLogs
                    ? Instantiate(successMessagePrefab, messageParent) 
                    : null,
                LogLevel.Alert => displayAlertLog && displayAllLogs
                    ? Instantiate(alertMessagePrefab, messageParent)
                    : null,
                LogLevel.Error => displayErrorLog && displayAllLogs
                    ? Instantiate(errorMessagePrefab, messageParent) 
                    : null,
                LogLevel.ByPass => Instantiate(alertMessagePrefab, messageParent),
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
