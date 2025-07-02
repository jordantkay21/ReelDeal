using KayosTech.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using static KayosTech.Components.TMPLinkOpenerWithHover;

namespace KayosTech.ReelDeal.Prototype
{
    public enum ActionType
    {
        None,
        RegisterDevice
    }
    public enum DisplayType
    {
        LinkCode
    }

    public enum DisplayTarget
    {
        PlexConnectionWindow,
    }

    #region InteractionDTO
    public interface IIntentDTO
    {
        ActionType Action { get; }
    }

    public sealed class RegisterDeviceIntent:IIntentDTO
    {
        public ActionType Action => ActionType.RegisterDevice;
    }
    #endregion

    #region Service Handler

    public interface IInteractionHandler
    {
        public void HandleInteraction();
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
        public static void SetLinkActions(TMPLinkOpenerWithHover hoverComponent, List<TMPLinkAction> actions)
        {
            var linkActionsField = typeof(TMPLinkOpenerWithHover)
                .GetField("linkActions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            linkActionsField?.SetValue(hoverComponent, actions.ToArray());
        }
    }

}
