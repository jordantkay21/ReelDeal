using UnityEngine;
using KayosTech.ReelDeal.Prototype;
using KayosTech.ReelDeal.Prototype.LogSystem;

namespace KayosTech.ReelDeal.Prototype.SessionConfig
{
    public class ButtonHandler : MonoBehaviour, IInteractionHandler
    {
        [SerializeField] ActionType action;

        public void HandleInteraction()
        {
            IInteractionDTO intent = new RegisterDeviceIntent();
            DevLog.Highlight($"RegisterDeviceIntent DTO created and dispatched", $"Action: {ActionType.RegisterDevice}");
            EventManager.DispatchInteraction(intent);
        }
    }
}