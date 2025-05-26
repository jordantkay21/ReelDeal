using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UILogIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<bool> OnHoverState;
    public static event Action OnClicked;

    [Header("Components")]
    [SerializeField] private RawImage iconImage;

    [Header("State Colors")]
    [SerializeField] private Color hoverStateColor;
    [SerializeField] private Color activeStateColor;
    [SerializeField] private Color inactiveStateColor;

    private Color _currentColor;
    //private bool _displayLog = true;

    private void Awake()
    {
        if (iconImage == null)
        {
            iconImage = GetComponent<RawImage>();
        }

    }

    private void OnEnable()
    {
        OnHoverState += HandleHoverState;
    }

    private void HandleHoverState(bool isHovering)
    {
        Debug.Log($"LogButton in Hover State {isHovering}");
        iconImage.color = isHovering ? hoverStateColor : _currentColor;
    }

    #region Event Triggers

    public void OnPointerEnter(PointerEventData eventData) => OnHoverState?.Invoke(true);

    public void OnPointerExit(PointerEventData eventData) => OnHoverState?.Invoke(false);

    public void OnPointerClick(PointerEventData eventData) => OnClicked?.Invoke();

    #endregion

    public void SetNewColor(bool isActive)
    {
        _currentColor = isActive ? activeStateColor : inactiveStateColor;

        iconImage.color = _currentColor;
    }
}
