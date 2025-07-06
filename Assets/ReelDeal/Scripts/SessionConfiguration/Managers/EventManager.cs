using System;
using UnityEngine;
using KayosTech.ReelDeal.Prototype.LogSystem;

namespace KayosTech.ReelDeal.Prototype.SessionConfig
{
    public static class EventManager
    {
        static EventManager() { }

        #region Utility Events
        #region Polling Request Events
        #region Polling Result
        public delegate void PollResultIssued(string result);
        public static event PollResultIssued OnPollingResponseIssued;
        public static void IssuePollingResponse(string result)
        {
            DevLog.Highlight("Poll Result Issued.", "Polling Logic");
            OnPollingResponseIssued?.Invoke(result);
        }
        #endregion
        #region Polling Stop

        public static event System.Action OnStopPollingIssued;
        public static void IssueStopPollingRequest()
        {
            DevLog.Highlight("Stop Polling Request Issued.", "Polling Logic");
            OnStopPollingIssued?.Invoke();
        }
        #endregion
        #endregion
        #endregion

        #region Interaction Event
        public delegate void ServiceIntentDispatched(IIntentDTO intent);

        public static event ServiceIntentDispatched OnServiceIntentDispatched;

        public static void DispatchServiceIntent(IIntentDTO intent)
        {
            DevLog.Highlight($"1 - ServiceIntent dispatched: {intent.GetType().Name}", "Data Flow");
            OnServiceIntentDispatched?.Invoke(intent);
        }
        #endregion

        #region Service Action
        public delegate void ServiceActionDispatched(Action.IServiceAction action);

        public static event ServiceActionDispatched OnServiceActionDispatched;

        public static void DispatchServiceAction(Action.IServiceAction action)
        {
            DevLog.Highlight($"2 - ServiceAction dispatched: {action.GetType().Name}", "Data Flow");
            OnServiceActionDispatched?.Invoke(action);
        }
        #endregion

        #region Service Command
        public delegate void ServiceCommandDispatched(Command.IServiceCommand command);

        public static event ServiceCommandDispatched OnServiceCommandDispatched;

        public static void DispatchServiceCommand(Command.IServiceCommand command)
        {
            DevLog.Highlight($"4 - Service Command dispatched: {command.GetType().Name}", "Data Flow");
            OnServiceCommandDispatched?.Invoke(command);
        }
        #endregion

        #region Service Response
        public delegate void ServiceResponseDispatched(Response.IServiceResponse response);

        public static event ServiceResponseDispatched OnServiceResponseDispatched;

        public static void DispatchServiceResponse(Response.IServiceResponse response)
        {
            DevLog.Highlight($"6 -  Service Response dispatched: \n {response.GetType().Name}", "Data Flow");
            OnServiceResponseDispatched?.Invoke(response);
        }
        #endregion

        #region Display Command
        public delegate void DisplayCommandDispatched(Command.IDisplayCommand command);

        public static event DisplayCommandDispatched OnDisplayCommandDispatched;

        public static void DispatchDisplayCommand(Command.IDisplayCommand command)
        {
            DevLog.Highlight($"8 - Display Command dispatched: {command.GetType().Name}", "Data Flow");
            OnDisplayCommandDispatched?.Invoke(command);
        }
        #endregion

        #region Display Action
        public delegate void DisplayActionDispatched(Action.IDisplayAction action);

        public static event DisplayActionDispatched OnDisplayActionDispatched;

        public static void DispatchDisplayAction(Action.IDisplayAction action)
        {
            DevLog.Highlight($"10 - DisplayAction dispatched: {action.GetType().Name}", "Data Flow");
            OnDisplayActionDispatched?.Invoke(action);
        }
        #endregion
    }
}