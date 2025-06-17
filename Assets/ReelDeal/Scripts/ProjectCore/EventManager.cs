using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic;
using System;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.Core
{
    public static class EventManager
    {
        static EventManager() {}

        #region User Interaction Event
        public delegate void InteractionDispatched(ActionType type, string rawData);

        public static event InteractionDispatched OnInteractionDispatched;

        public static void DispatchInteraction(ActionType type, string rawData = null)
        {
            OnInteractionDispatched?.Invoke(type, rawData);
        }
        #endregion

        #region Action Event

        public delegate void ActionDispatched(IActionPayload payload);

        public static event ActionDispatched OnActionDispatched;

        public static void DispatchActionPayload(IActionPayload payload)
        {
            OnActionDispatched?.Invoke(payload);
        }

        #endregion

        #region Command Event

        public delegate void ServiceCommandDispatched(IServiceCommand command);

        public static event ServiceCommandDispatched OnServiceCommandDispatched;

        public static void DispatchServiceCommand(IServiceCommand command)
        {
            OnServiceCommandDispatched?.Invoke(command);
        }
        #endregion

        #region Response Event

        public delegate void ResponseDispatched(IResponsePayload response);

        public static event ResponseDispatched OnResponsePayloadDispatched;

        public static void DispatchResponsePayload(IResponsePayload response)
        {
            OnResponsePayloadDispatched?.Invoke(response);
        }

        #endregion

        #region

        public delegate void DisplayDispatched(IDisplayCommand display);

        public static event DisplayDispatched OnDisplayCommandDispatched;

        public static void DispatchDisplayCommand(IDisplayCommand display)
        {
            OnDisplayCommandDispatched?.Invoke(display);
        }

        #endregion
    }
}
