using System;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Frontend;
using KayosTech.ReelDeal.Prototype.LogSystem.Frontend;
using KayosTech.ReelDeal.Prototype.LogSystem.DataStructure;
using UnityEngine;
using UnityEngine.UI;

public class LogRenderer : MonoBehaviour
{
    [SerializeField] private GameObject _logPrefab;
    [SerializeField] private Transform _logContainer;

    private void Awake()
    {
        LogRouter.OnUpstreamCommand += AcceptCommand;

    }

    private void AcceptCommand(LogCommandDTO command)
    {
        var logDisplay = new LogDisplayPayload(
            command.Message,
            command.SecondaryColor,
            command.PrimaryColor,
            command.Duration,
            command.FadeDuration);

        HandleLogDisplay(logDisplay);

    }

    private void HandleLogDisplay(LogDisplayPayload logDisplay)
    {
        GameObject logInstance = Instantiate(_logPrefab, _logContainer);
        var handler = logInstance.GetComponent<LogDisplayHandler>();
        if (handler != null)
        {
            handler.Initialize(logDisplay);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) _logContainer);
        }
        else
        {
            Debug.LogWarning("Log prefab missing LogDisplayHandler component");
        }
    }
}
