using KayosTech.ReelDeal.Prototype.LogSystem;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        private static async void HandleIncomingServiceCommand(Command.IServiceCommand command)
        {
            DevLog.Highlight($"5 - Service Action received: {command.GetType().Name}", "Data Flow");

            try
            {
                var response = await ServiceResponseFactory.ExecuteCommandAsync(command);
                EventManager.DispatchServiceResponse(response);
            }
            catch (Exception ex)
            {
                DevLog.Error($"[ExecuteCommandAsync] {ex.GetType().Name} - {ex.Message}", "Data Flow");
                throw;
            }
        }
    }

    #region Service Response Checkpoint

    #region Service Response TPOs
    public interface IServiceResponse
    {
        ActionType Action { get; }
    }
    public class RegisterDeviceResponse: IServiceResponse
    {
        public ActionType Action => ActionType.RegisterDevice;

        public string Response;

        public RegisterDeviceResponse(string response)
        {
            Response = response;
        }

        public override string ToString()
        {
            return $"RegisterDeviceResponse: {Response}";
        }
    }
    #endregion

    public static class ServiceResponseFactory
    {
        public static readonly HttpClient client = new HttpClient();

        public static async Task<IServiceResponse> ExecuteCommandAsync(Command.IServiceCommand command)
        {
            var httpResponse = await client.SendAsync(command.Request);
            var response = await httpResponse.Content.ReadAsStringAsync();
            return new RegisterDeviceResponse(response);
        } 
    }
    #endregion
}