using KayosTech.ReelDeal.Prototype.LogSystem;
using System;
using UnityEngine;


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
        private static void HandleIncomingInteraction(IInteractionDTO intent)
        {
            DevLog.Highlight($"1 - InteractionDTO received: {intent.GetType().Name}", "Data Flow");
            var action = ServiceActionFactory.CreateFrom(intent);
            EventManager.DispatchServiceAction(action);
        }

        private static void HandleIncomingDisplayCommand(Command.IDisplayCommand command)
        {
            DevLog.Highlight($"9 - Display Command received: {command.GetType().Name}", "Data Flow");
            //TODO - var action = DisplayActionFactory;
            //EventManager.DispatchDisplayAction(action);
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
        public static IServiceAction CreateFrom(IInteractionDTO intent)
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
        DisplayType Display { get; }
    }
    #endregion

    public static class DisplayActionFactory
    {

    }

    public static class DisplayActionUtilities
    {

    }

    public static class DisplayActionHelpers
    {

    }
    #endregion
}
