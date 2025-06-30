using System;
using UnityEngine;
using KayosTech.ReelDeal.Prototype.SessionConfig.Action;
using KayosTech.ReelDeal.Prototype.SessionConfig.Command;
using KayosTech.ReelDeal.Prototype.SessionConfig.Response;
using KayosTech.ReelDeal.Prototype.LogSystem;

namespace KayosTech.ReelDeal.Prototype
{
    public static class EventManager
    {
        static EventManager() { }

        #region Interaction Event
        public delegate void InteractionDispatched(IInteractionDTO intent);

        public static event InteractionDispatched OnInteractionDispatched;

        public static void DispatchInteraction(IInteractionDTO intent)
        {
            OnInteractionDispatched?.Invoke(intent);
        }
        #endregion

        #region Service Action
        public delegate void ServiceActionDispatched(IServiceAction action);

        public static event ServiceActionDispatched OnServiceActionDispatched;

        public static void DispatchServiceAction(IServiceAction action)
        {
            DevLog.Highlight($"2 - ServiceAction dispatched: {action.GetType().Name}", "Data Flow");
            OnServiceActionDispatched?.Invoke(action);
        }
        #endregion

        #region Service Command
        public delegate void ServiceCommandDispatched(IServiceCommand command);

        public static event ServiceCommandDispatched OnServiceCommandDispatched;

        public static void DispatchServiceCommand(IServiceCommand command)
        {
            DevLog.Highlight($"4 - Service Command dispatched: {command.GetType().Name}", "Data Flow");
            OnServiceCommandDispatched?.Invoke(command);
        }
        #endregion

        #region Service Response
        public delegate void ServiceResponseDispatched(IServiceResponse response);

        public static event ServiceResponseDispatched OnServiceResponseDispatched;

        public static void DispatchServiceResponse(IServiceResponse response)
        {
            DevLog.Highlight($"6 -  Service Response dispatched: \n {response}", "Data Flow");
            OnServiceResponseDispatched?.Invoke(response);
        }
        #endregion

        #region Display Command
        public delegate void DisplayCommandDispatched(IDisplayCommand command);

        public static event DisplayCommandDispatched OnDisplayCommandDispatched;

        public static void DispatchDisplayCommand(IDisplayCommand command)
        {
            DevLog.Highlight($"8 - Display Command dispatched: {command.GetType().Name}", "Data Flow");
            OnDisplayCommandDispatched?.Invoke(command);
        }
        #endregion

        #region Display Action
        public delegate void DisplayActionDispatched(IDisplayAction action);

        public static event DisplayActionDispatched OnDisplayActionDispatched;

        public static void DispatchDisplayAction(IDisplayAction action)
        {
            DevLog.Highlight($"10 - DisplayAction dispatched: {action.GetType().Name}", "Data Flow");
            OnDisplayActionDispatched?.Invoke(action);
        }
        #endregion
    }
}