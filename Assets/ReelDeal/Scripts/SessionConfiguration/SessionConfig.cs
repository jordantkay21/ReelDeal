using KayosTech.ReelDeal.Prototype.LogSystem;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using System;

namespace KayosTech.ReelDeal.Prototype.SessionConfig
{
    #region ENUMS
    public enum ActionType     
    {
        None,
        RegisterDevice,
        PollAuth
    }
    public enum DisplayType
    {
        LinkCode,
        AuthPollStatus,
        AccountAuthSuccess
    }

    public enum DisplayTarget
    {
        PlexConnectionWindow,
    }
    #endregion

    #region INTERFACES
    public interface IIntentDTO
    {
        ActionType Action { get; }
    }
    public interface IInteractionHandler
    {
        public void HandleInteraction();
    }
    public interface IDisplayHandler
    {
        DisplayTarget Target { get; }
        private void HandleIncomingDisplayAction(Action.IDisplayAction action) { }
    }
    #endregion

    #region INTENT DTO
    public sealed class RegisterDeviceIntent:IIntentDTO
    {
        public ActionType Action => ActionType.RegisterDevice;
    }
    public sealed class PollAuthIntent : IIntentDTO
    {
        public ActionType Action => ActionType.PollAuth;
    }

    #endregion

    public static class IntentUtilities
    {
        public static IIntentDTO RetrieveIntent(ActionType action)
        {
            switch (action)
            {
                case ActionType.None:
                    return null;
                case ActionType.RegisterDevice:
                    return new RegisterDeviceIntent();
                default:
                    throw new NotSupportedException($"Unsupported User Intent: {action}");
            }
        }
    }

    public static class DisplayUtilities
    {
    }

}
