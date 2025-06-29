using KayosTech.ReelDeal.Prototype.LogSystem;
using KayosTech.ReelDeal.Prototype.Managers;
using System;
using System.Net.Http;
using UnityEngine;


namespace KayosTech.ReelDeal.Prototype.SessionConfig.Command
{
    public static class CommandDispatcher
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ = typeof(EventManager);
            EventManager.OnServiceActionDispatched += HandleIncomingServiceAction;
            EventManager.OnServiceResponseDispatched += HandleIncomingServiceResponse;
        }
        private static void HandleIncomingServiceAction(Action.IServiceAction action)
        {
            DevLog.Highlight($"3 - Service Action received: {action.GetType().Name}", "Data Flow");
            var command = ServiceCommandFactory.CreateFrom(action);
            EventManager.DispatchServiceCommand(command);
        }

        private static void HandleIncomingServiceResponse(Response.IServiceResponse response)
        {
            DevLog.Highlight($"7 - Service Response received: {response.GetType().Name}", "Data Flow");
            //TODO - var command = DisplayCommandFactory
            //EventManager.DispatchDisplayCommand(command);
        }

    }
    #region Service Command Checkpoint
    #region Service Command TPOs
    public interface IServiceCommand
    {
        ActionType Action { get; }
    }

    public class RegisterDeviceCommand : IServiceCommand
    {
        public ActionType Action => ActionType.RegisterDevice;
        public HttpRequestMessage Request { get; }

        public RegisterDeviceCommand(HttpRequestMessage request)
        {
            Request = request;
        }
    }

    #endregion
    public static class ServiceCommandFactory
    {
        public static IServiceCommand CreateFrom(Action.IServiceAction action)
        {
            switch (action)
            {
                case Action.RegisterDeviceAction registerDevice:
                    var request = ServiceCommandUtilities.CreateRegisterRequest();
                    return new RegisterDeviceCommand(request);                    
                default:
                    throw new NotSupportedException($"Unsupported Service Action: {action.GetType().Name}");
            }
        }
    }
    public static class ServiceCommandUtilities
    {
        public static HttpRequestMessage CreateRegisterRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://plex.tv/api/v2/pins.xml");
            ServiceCommandHelpers.AttachHeaders(request);

            return request;
        }

    }
    public static class ServiceCommandHelpers
    {
        public static void AttachHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Accept", "Application/XML");
            request.Headers.Add("X-Plex-Client-Identifier", SessionManager.GetClientID());
            request.Headers.Add("X-Plex-Product", Application.productName);
            request.Headers.Add("X-Plex-Version", Application.version);
            request.Headers.Add("X-Plex-Platform", Application.platform.ToString());
            request.Headers.Add("X-Plex-Device-Name", SystemInfo.deviceName);
        }
    }
    #endregion

    #region Display Command Checkpoint
    public interface IDisplayCommand
    {
        DisplayType Display { get; }
    }
    #region Display Command TPOs

    #endregion

    public static class DisplayCommandFactory
    {

    }

    public static class DisplayCommandUtilities
    {

    }

    public static class DisplayCommandHelpers
    {

    }
    #endregion
}
