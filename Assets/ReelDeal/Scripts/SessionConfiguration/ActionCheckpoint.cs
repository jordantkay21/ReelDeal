using KayosTech.ReelDeal.Prototype.LogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static KayosTech.Components.TMPLinkOpenerWithHover;

namespace KayosTech.ReelDeal.Prototype.SessionConfig.Action
{


    public static class ActionDispatcher
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ = typeof(EventManager);
            EventManager.OnInteractionDispatched += HandleIncomingInteraction;
            EventManager.OnDisplayCommandDispatched += HandleIncomingDisplayCommand;
        }
        private static void HandleIncomingInteraction(IIntentDTO intent)
        {
            DevLog.Highlight($"1 - InteractionDTO received: {intent.GetType().Name}", "Data Flow");
            var action = ServiceActionFactory.CreateFrom(intent);
            EventManager.DispatchServiceAction(action);
        }

        private static void HandleIncomingDisplayCommand(Command.IDisplayCommand command)
        {
            DevLog.Highlight($"9 - Display Command received: {command.ToString()}", "Data Flow");
            var action = DisplayActionFactory.CreateFrom(command);
            EventManager.DispatchDisplayAction(action);
        }

    }

    #region Service Action Checkpoint

    #region Service Action TPOs
    public interface IServiceAction
    {
        ActionType Action { get; }
    }

    public class RegisterDeviceAction : IServiceAction
    {
        public ActionType Action => ActionType.RegisterDevice;
    }
    #endregion

    public static class ServiceActionFactory
    {
        public static IServiceAction CreateFrom(IIntentDTO intent)
        {
            switch (intent)
            {
                case RegisterDeviceIntent registerDevice:
                    return new RegisterDeviceAction();
                default:
                    throw new NotSupportedException($"Unsupported user interaction: {intent.GetType().Name}");
            }
        }
    }

    public static class ServiceActionUtilities
    {

    }

    public static class ServiceActionHelpers
    {

    }
    #endregion

    #region Display Action Checkpoint

    #region Display Action TPOs
    public interface IDisplayAction
    {
        DisplayTarget Target { get; }
        DisplayType Display { get; }
    }

    public class LinkCodeDisplayAction : IDisplayAction
    {
        public DisplayTarget Target => DisplayTarget.PlexConnectionWindow;
        public DisplayType Display => DisplayType.LinkCode;

        public string Message;


        public override string ToString()
        {
            return $"[DisplayType: {Display}, Message: \"{Message}\"";
        }
    }
    #endregion

    public static class DisplayActionFactory
    {
        public static IDisplayAction CreateFrom(Command.IDisplayCommand command)
        {
            switch (command)
            {
                case Command.LinkCodeDisplayCommand linkCode:
                    return CreateLinkIDDisplay(linkCode);
                default:
                    throw new NotSupportedException($"Unsupported Display Command: {command.GetType().Name}");
            }
        }

        private static LinkCodeDisplayAction  CreateLinkIDDisplay(Command.LinkCodeDisplayCommand command)
        {
            var message = DisplayFormattingUtility.FormatLinkCodeMessage(command.Url, command.Code, command.ExpiresAt);

            return new LinkCodeDisplayAction()
            {
                Message = message,
            };
        }
    }

    #region Utilties
    public static class DisplayFormattingUtility
    {
        public static string FormatLinkCodeMessage(string url, string code, string expiresAt)
        {
            return $"To link your device, visit <b>{url}</b> \n" +
                $"Enter the code: <b>{code}</b>\n" +
                $"This code expires at {DisplayActionHelpers.FormatUTCTimestamp(expiresAt)}";
        }
    }
    #endregion

    public static class DisplayActionHelpers
    {
        public static string FormatUTCTimestamp(string utcTimestamp)
        {
            var cleanedUTC = utcTimestamp.Replace(" UTC", "").Trim();

            DateTime utcDateTime = DateTime.ParseExact(
                cleanedUTC,
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal);

            DateTime localTime = utcDateTime.ToLocalTime();

            return localTime.ToString("hh:mm tt");
        }
    }
    #endregion
}
