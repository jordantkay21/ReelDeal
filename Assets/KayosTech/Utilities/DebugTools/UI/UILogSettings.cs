using System;
using KayosTech.Utilities.DebugTools;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILogSettings : MonoBehaviour
{
    #region Events
    public static Action<bool> OnToggleInfoLog;
    public static Action<bool> OnToggleSuccessLog;
    public static Action<bool> OnToggleAlertLog;
    public static Action<bool> OnToggleErrorLog;
    public static Action OnToggleAllLogs;

    public static Action<float> OnAdjustInfoDisplayTime;
    public static Action<float> OnAdjustSuccessDisplayTime;
    public static Action<float> OnAdjustAlertDisplayTime;
    public static Action<float> OnAdjustErrorDisplayTime;

    public static Action<string> OnTestInfoLog;
    public static Action<string> OnTestSuccessLog;
    public static Action<string> OnTestAlertLog;
    public static Action<string> OnTestErrorLog;
    public static Action OnTestAllLogs;

    public static Action<bool> OnToggleSaveToFile;
    public static Action OnClearLogCache;

    #endregion

    #region Inspector Initilized Variables

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
    public UILogIcon logToggle;
    //public Toggle displayLogToggle;
    public Toggle saveToFileToggle;
    public Button testAllButton;
    public Button clearLogCacheButton;
    #endregion

    #region private variables

    /// <summary>
    /// Used for <see cref="LogNewSliderValue"/>
    /// </summary>
    private LogLevel adjustedLevel;
    /// <summary>
    /// Used for <see cref="LogNewSliderValue"/>
    /// </summary>
    private float adjustedDisplayTime;

    #endregion


    private void Awake()
    {
        testInfoButton.onClick.AddListener(() => OnTestInfoLog?.Invoke(RetrieveTestLogMessage(LogLevel.Info)));
        testSuccessButton.onClick.AddListener(() => OnTestSuccessLog?.Invoke(RetrieveTestLogMessage(LogLevel.Success)));
        testAlertButton.onClick.AddListener(() => OnTestAlertLog?.Invoke(RetrieveTestLogMessage(LogLevel.Alert)));
        testErrorButton.onClick.AddListener(() => OnTestErrorLog?.Invoke(RetrieveTestLogMessage(LogLevel.Error)));
        testAllButton.onClick.AddListener(() => OnTestAllLogs?.Invoke());


        infoToggle.onValueChanged.AddListener((isActive) => HandleInfoLogToggle(isActive));
        successToggle.onValueChanged.AddListener((isActive) => HandleSuccessLogToggle(isActive));
        alertToggle.onValueChanged.AddListener((isActive) => HandleAlertLogToggle(isActive));
        errorToggle.onValueChanged.AddListener((isActive) => HandleErrorLogToggle(isActive));
        
        logToggle.OnClicked += () => OnToggleAllLogs?.Invoke();

        infoTimerSlider.onValueChanged.AddListener(HandleInfoLogSlider);
        successTimerSlider.onValueChanged.AddListener(HandleSuccessLogSlider);
        alertTimerSlider.onValueChanged.AddListener(HandleAlertLogSlider);
        errorTimerSlider.onValueChanged.AddListener(HandleErrorLogSlider);

        saveToFileToggle.onValueChanged.AddListener(HandleToggleSaveToFile);
        clearLogCacheButton.onClick.AddListener(() => OnClearLogCache?.Invoke());
    }

    private void OnEnable()
    {
        UILogDispatcher.AdjustAllLogs += DisplayAllLogsToggle;
    }


    #region Handle Log Toggles

    private void HandleInfoLogToggle(bool isActive, bool activateEvent = true)
    {
        infoTimerSlider.interactable = isActive;
        testInfoButton.interactable = isActive;
        infoToggle.isOn = isActive;

        infoTimerLabel.text = isActive ? $"Display for <b>{infoTimerSlider.value} Seconds</b>" : "Display Off";

        if(activateEvent)
            OnToggleInfoLog?.Invoke(isActive);
    }

    private void HandleSuccessLogToggle(bool isActive, bool activateEvent = true)
    {
        successTimerSlider.interactable = isActive;
        testSuccessButton.interactable = isActive;
        successToggle.isOn = isActive;

        successTimerLabel.text = isActive ? $"Display for <b>{successTimerSlider.value} Seconds</b>" : "Display Off";

        if(activateEvent)
            OnToggleSuccessLog?.Invoke(isActive);

    }

    private void HandleAlertLogToggle(bool isActive, bool activateEvent = true)
    {
        alertTimerSlider.interactable = isActive;
        testAlertButton.interactable = isActive;
        alertToggle.isOn = isActive;

        alertTimerLabel.text = isActive ? $"Display for <b>{alertTimerSlider.value} Seconds</b>" : "Display Off";

        if(activateEvent)
            OnToggleAlertLog?.Invoke(isActive);
    }

    private void HandleErrorLogToggle(bool isActive, bool activateEvent = true)
    {
        errorTimerSlider.interactable = isActive;
        testErrorButton.interactable = isActive;
        errorToggle.isOn = isActive;


        errorTimerLabel.text = isActive ? $"Display for <b>{errorTimerSlider.value} Seconds</b>" : "Display Off";

        if(activateEvent)
            OnToggleErrorLog?.Invoke(isActive);
    }

    #endregion

    #region Handle Log Sliders

    private void HandleInfoLogSlider(float displayTime)
    {
        infoTimerLabel.text = $"Display for <b>{displayTime} Seconds<b>";
        SetLogAdjustmentData(LogLevel.Info, displayTime);
        OnAdjustInfoDisplayTime?.Invoke(displayTime);
    }

    private void HandleSuccessLogSlider(float displayTime)
    {
        successTimerLabel.text = $"Display for <b>{displayTime} Seconds<b>";
        SetLogAdjustmentData(LogLevel.Success, displayTime);
        OnAdjustSuccessDisplayTime(displayTime);
    }

    private void HandleAlertLogSlider(float displayTime)
    {
        alertTimerLabel.text = $"Display for <b>{displayTime} Seconds<b>";
        SetLogAdjustmentData(LogLevel.Alert, displayTime);
        OnAdjustAlertDisplayTime(displayTime);
    }

    private void HandleErrorLogSlider(float displayTime)
    {
        errorTimerLabel.text = $"Display for <b>{displayTime} Seconds<b>";
        SetLogAdjustmentData(LogLevel.Error, displayTime);
        OnAdjustErrorDisplayTime(displayTime);
    }

    #endregion

    #region Global Logic

    public void DisplayAllLogsToggle(bool isActive)
    {
        //LogRouter.Log("Toggle All Logs", $"Turn all Toggles {isActive}", LogLevel.Internal);

        HandleInfoLogToggle(isActive);
        HandleSuccessLogToggle(isActive);
        HandleAlertLogToggle(isActive);
        HandleErrorLogToggle(isActive);

        logToggle.ToggleActiveState();
        testAllButton.interactable = isActive;
    }

    private void HandleToggleSaveToFile(bool isActive)
    {
        saveToFileToggle.isOn = isActive;
        OnToggleSaveToFile?.Invoke(isActive);
    }
    #endregion

    #region Logging Logic
    private void SetLogAdjustmentData(LogLevel level, float displayTime)
    {
        adjustedLevel = level;
        adjustedDisplayTime = displayTime;

    }
    private string RetrieveTestLogMessage(LogLevel level)
    {
        return $"Test {level} Log";
    }

    /// <summary>
    /// Called on Slider GameObject's Event Trigger Component
    /// </summary>
    public void LogNewSliderValue()
    {
        LogRouter.Log("UIConfig", $"{adjustedLevel} Log Display Lifetime set to {adjustedDisplayTime}");
    }



    #endregion


}
