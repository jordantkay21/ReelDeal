using KayosTech.ReelDeal.Prototype.Core.Event;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Frontend
{
    [RequireComponent(typeof(Button))]
    public class ButtonHandler : MonoBehaviour
    {
        [Tooltip("What action this button represents")]
        public ActionType actionType;

        [Tooltip("Optional additional data")]
        [TextArea] public string rawData = "";

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            EventManager.DispatchInteraction(actionType, rawData);
        }
    }
}
