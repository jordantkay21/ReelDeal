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
                var rawResponse = await CommandExecution.ExecuteAsync(command.Request);
                var serviceResponse = ServiceResponseFactory.Create(command.Action, rawResponse);
                EventManager.DispatchServiceResponse(serviceResponse);
            }
            catch (Exception ex)
            {
                DevLog.Error(ex.Message, "Data Flow");
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

    #region Service Response DTO
    public interface ResponseDTO { }

    public class RegisterDeviceDTO : ResponseDTO
    {
        public string Code;
        public string ID;
        public string Timestamp;

        public override string ToString()
        {
            return $"Extracted Register Device Data: Code = {Code} | ID = {ID} | Expires = {Timestamp}";
            
        }
    }
    #endregion
    public static class CommandExecution
    {
        public static readonly HttpClient client = new HttpClient();

        public static async Task<string> ExecuteAsync(HttpRequestMessage request)
        {
            var httpResponse = await client.SendAsync(request);
            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
    public static class ServiceResponseFactory
    {
        public static IServiceResponse Create(ActionType action, string rawResponse)
        {
            switch (action)
            {
                case ActionType.RegisterDevice:
                    return CreateRegistrationResponse(rawResponse);
                default:
                    throw new NotSupportedException($"Unsupported Service Action: {action}"); ;
            }
        }

        private static RegisterDeviceResponse CreateRegistrationResponse(string rawResponse)
        {
            var dto = ResponseParserUtility.ParseRegisterDevice(rawResponse);

            return new RegisterDeviceResponse(dto.Code, dto.ID, dto.Timestamp);
        }
    }

    #region Utilities
    public static class ResponseParserUtility
    {
        public static RegisterDeviceDTO ParseRegisterDevice(string rawResponse)
        {
            var doc = XDocument.Parse(rawResponse);
            DevLog.Internal($"XDocument created {doc}", $"Action: {ActionType.RegisterDevice}");

            var dto = new RegisterDeviceDTO();

            dto.Code = ServiceResponseHelpers.ExtractData(doc, "code");
            dto.ID = ServiceResponseHelpers.ExtractData(doc, "id");
            dto.Timestamp = ServiceResponseHelpers.FormatUTCTimestamp(ServiceResponseHelpers.ExtractData(doc, "expiresAt"));

            DevLog.Internal(dto.ToString());

            return dto;
        }
    }
    #endregion
    public static class ServiceResponseHelpers
    {
        public static string ExtractData(XDocument doc, string attribute)
        {
            return doc.Root?.Attribute(attribute)?.Value ?? MissingContent(attribute);
        }

        public static string MissingContent(string attribute)
        {
            throw new InvalidOperationException($"Expected attribute {attribute} was not found in the XML document.");
        }

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