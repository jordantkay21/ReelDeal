using KayosTech.ReelDeal.Prototype.LogSystem;
using System;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfig.Response
{
    public static class ResponseDispatcher
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ = typeof(EventManager);
            EventManager.OnServiceCommandDispatched += HandleIncomingServiceCommand;
        }

        private static void HandleIncomingServiceCommand(Command.IServiceCommand command)
        {
            DevLog.Highlight($"5 - Service Action received: {command.GetType().Name}", "Data Flow");
            //TODO - var response = ServiceResponseFactory
            //EventManager.DispatchServiceResponse(response);
        }
    }

    #region Service Response Checkpoint

    #region Service Response TPOs
    public interface IServiceResponse
    {
        ActionType Action { get; }
    }

    public class RegisterDeviceResponse
    {
        public ActionType Action => ActionType.RegisterDevice;

        public string Code;
        public string PinId;
        public string ExpiresAt;

        public RegisterDeviceResponse(string code, string id, string expiresAt)
        {
            Code = code;
            PinId = id;
            ExpiresAt = expiresAt;
        }

        public override string ToString()
        {
            return $"RegisterDeviceResponse: Code=\"{Code ?? "[null]"}\", Id=\"{PinId ?? "null"}\", ExpiresAt=\"{ExpiresAt ?? "[null]"}\"";
        }
    }
    #endregion

    public static class ServiceResponseFactory
    {

    }

    public static class ServiceResponseUtilities
    {

    }

    public static class ServiceResponseHelpers
    {

    }
    #endregion
}