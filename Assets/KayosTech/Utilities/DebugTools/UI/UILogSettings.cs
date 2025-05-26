using System;
using KayosTech.Utilities.DebugTools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILogSettings : MonoBehaviour
{
    [Header("References")] 
    public UILogDispatcher dispatcher;
    
    [Header("Test Buttons")] 
    public Button testInfoButton;
    public Button testSuccessButton;
    public Button testAlertButton;
    public Button testErrorButton;

    [Header("Display Timer Sliders")] 
    public Slider infoTimerSlider;
    public Slider successTimerSlider;
    public Slider alertTimerSlider;
    public Slider errorTimerSlider;

    [Header("Display Timer Labels")] 
    public TextMeshProUGUI infoTimerLabel;
    public TextMeshProUGUI successTimerLabel;
    public TextMeshProUGUI alertTimerLabel;
    public TextMeshProUGUI errorTimerLabel;

    [Header("Log Visible Toggles")] 
    public Toggle infoToggle;
    public Toggle successToggle;
    public Toggle alertToggle;
    public Toggle errorToggle;

    [Header("Global Toggles")] 
    public Toggle displayLogToggle;
    public Toggle saveToFileToggle;
    public Button testAllButton;
    public Button clearLogCacheButton;

    private LogLevel adjustedLevel;
    private float adjustedValue;

    private void Awake()
    {
        testInfoButton.onClick.AddListener(() => LogRouter.TestLog(LogLevel.Info));
        testSuccessButton.onClick.AddListener(() => LogRouter.TestLog(LogLevel.Success));
        testAlertButton.onClick.AddListener(() => LogRouter.TestLog(LogLevel.Alert));
        testErrorButton.onClick.AddListener(() => LogRouter.TestLog(LogLevel.Error));
        testAllButton.onClick.AddListener(() => LogRouter.TestLog(LogLevel.Info,true));

        infoToggle.onValueChanged.AddListener((value) =>AdjustLogDisplay(LogLevel.Info, value));
        successToggle.onValueChanged.AddListener((value) => AdjustLogDisplay(LogLevel.Success, value));
        alertToggle.onValueChanged.AddListener((value) => AdjustLogDisplay(LogLevel.Alert, value));
        errorToggle.onValueChanged.AddListener((value) => AdjustLogDisplay(LogLevel.Error, value));
        displayLogToggle.onValueChanged.AddListener((value) => AdjustLogDisplay(LogLevel.Info, value, true));

        infoTimerSlider.onValueChanged.AddListener((value) => AdjustDisplayTime(LogLevel.Info, value, infoTimerLabel));
        successTimerSlider.onValueChanged.AddListener((value) => AdjustDisplayTime(LogLevel.Success, value, successTimerLabel));
        alertTimerSlider.onValueChanged.AddListener((value) => AdjustDisplayTime(LogLevel.Alert, value, alertTimerLabel));
        errorTimerSlider.onValueChanged.AddListener((value) => AdjustDisplayTime(LogLevel.Error, value, errorTimerLabel));

        saveToFileToggle.onValueChanged.AddListener((value) => dispatcher.ToggleSaveToFile(value));
        clearLogCacheButton.onClick.AddListener(LogRouter.ClearLogCache);
    }

    private void AdjustDisplayTime(LogLevel level, float newValue, TextMeshProUGUI label)
    {
        switch (level)
        {
            case LogLevel.Internal:
            case LogLevel.Info:
                dispatcher.infoTimer = newValue;
                break;
            case LogLevel.Success:
                dispatcher.successTimer = newValue;
                break;
            case LogLevel.Alert:
                dispatcher.alertTimer = newValue;
                break;
            case LogLevel.Error:
                dispatcher.errorTimer = newValue;
                break;
            default:
                LogRouter.Log($"Error", "Invalid Log Level Detected", LogLevel.Error);
                break;
        }

        label.text = $"{newValue} Seconds";

        adjustedLevel = level;
        adjustedValue = newValue;

    }

    /// <summary>
    /// Called on Slider GameObject's Event Trigger Component
    /// </summary>
    public void LogNewValue()
    {
        LogRouter.Log("UIConfig", $"{adjustedLevel} Log Display Lifetime set to {adjustedValue}");
    }

    private void AdjustLogDisplay(LogLevel level, bool newValue, bool adjustAll = false)
    {
        string state = newValue ? "active" : "inactive";

        if (!adjustAll)
        {
            switch (level)
            {
                case LogLevel.Internal:
                case LogLevel.Info:
                    dispatcher.displayInfoLog = newValue;
                    testInfoButton.interactable = newValue;
                    break;
                case LogLevel.Success:
                    dispatcher.displaySuccessLog = newValue;
                    testSuccessButton.interactable = newValue;
                    break;
                case LogLevel.Alert:
                    dispatcher.displayAlertLog = newValue;
                    testAlertButton.interactable = newValue;
                    break;
                case LogLevel.Error:
                    dispatcher.displayErrorLog = newValue;
                    testErrorButton.interactable = newValue;
                    break;
                default:
                    LogRouter.Log($"Error", "Invalid Log Level Detected", LogLevel.Error);
                    break;
            }

            if (dispatcher.displayInfoLog && dispatcher.displaySuccessLog && dispatcher.displayAlertLog &&
                dispatcher.displayErrorLog)
            {
                UILogDispatcher.OnLogDisplaySync?.Invoke(true);
            }
            else if (!dispatcher.displayInfoLog && !dispatcher.displaySuccessLog && !dispatcher.displayAlertLog &&
                     !dispatcher.displayErrorLog)
            {
                UILogDispatcher.OnLogDisplaySync?.Invoke(false);
            }

            LogRouter.Log("UIConfig", $"{level} Log Display is {state}");
        }
        else
        {
            testInfoButton.interactable = newValue;
            infoToggle.isOn = newValue;

            testSuccessButton.interactable = newValue;
            successToggle.isOn = newValue;

            testAlertButton.interactable = newValue;
            alertToggle.isOn = newValue;

            testErrorButton.interactable = newValue;
            errorToggle.isOn = newValue;

            dispatcher.ToggleDisplayLogs(newValue);

            LogRouter.Log("UIConfig", $"All Log Displays are set to {state}");
        }

    }

}
