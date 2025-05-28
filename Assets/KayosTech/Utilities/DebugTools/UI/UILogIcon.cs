using System;
using KayosTech.Utilities.DebugTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UILogIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<bool> OnHoverState;
    public event Action OnClicked;
    public bool currentActiveState;


    [Header("Components")]
    [SerializeField] private RawImage iconImage;

    [Header("State Colors")]
    [SerializeField] private Color hoverStateColor;
    [SerializeField] private Color activeStateColor;
    [SerializeField] private Color inactiveStateColor;

    private Color _currentColor;

    private void Awake()
    {
        if (iconImage == null)
        {
            iconImage = GetComponent<RawImage>();
        }

        _currentColor = activeStateColor;
    }

    private void OnEnable()
    {
        OnHoverState += HandleHoverState;
    }

    private void HandleHoverState(bool isHovering)
    {
        iconImage.color = isHovering ? hoverStateColor : _currentColor;
    }

    #region Event Triggers

    public void OnPointerEnter(PointerEventData eventData) => OnHoverState?.Invoke(true);

    public void OnPointerExit(PointerEventData eventData) => OnHoverState?.Invoke(false);

    public void OnPointerClick(PointerEventData eventData) => HandleOnClick();

    #endregion

    private void HandleOnClick()
    {
        //LogRouter.Log("OnClick Invoked", "ToggleActiveStateCalled");
        OnClicked?.Invoke();
    }
    public void ToggleActiveState()
    {
        bool newState = !currentActiveState;

        string logStatus = newState ? "active" : "inactive";
        _currentColor = newState ? activeStateColor : inactiveStateColor;
        
        currentActiveState = newState;
        iconImage.color = _currentColor;

    }

}