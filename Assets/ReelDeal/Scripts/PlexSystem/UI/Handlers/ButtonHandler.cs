using KayosTech.ReelDeal.Prototype.Core;
using UnityEngine;
using UnityEngine.UI;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.UI.Handler
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
