using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace KayosTech.Utilities.DebugTools
{
    public class UILogDispatcher : MonoBehaviour
    {
        [Header("Message Prefabs")] 
        [SerializeField] private UILogMessage infoMessagePrefab;
        [SerializeField] private UILogMessage successMessagePrefab;
        [SerializeField] private UILogMessage alertMessagePrefab;
        [SerializeField] private UILogMessage errorMessagePrefab;

        [Header("Parent Object")] 
        [SerializeField] private Transform messageParent;


        private void OnEnable()
        {
            LogRouter.OnLogReceived += DisplayLog;
        }

        private void OnDisable()
        {
            LogRouter.OnLogReceived -= DisplayLog;
        }

        private void DisplayLog(LogEntry log)
        {
            var newMsg = log.Level switch
            {
                LogLevel.Internal => null,
                LogLevel.Info => Instantiate(infoMessagePrefab, messageParent),
                LogLevel.Success => Instantiate(successMessagePrefab, messageParent),
                LogLevel.Alert => Instantiate(alertMessagePrefab, messageParent),
                LogLevel.Error => Instantiate(errorMessagePrefab, messageParent),
                _ => HandleUnknownLevel(log)
            };

            if (newMsg == null) return;

            newMsg.Initialize(log.Message, log.Level);

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
