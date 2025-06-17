using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using KayosTech.ReelDeal.Prototype.LogSystem;
using KayosTech.Styles;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic
{


    #region Enums

    public enum ActionType
    {
        RegisterDevice,
        PollAuth,
        RetrieveAccountInfo
    }

    public enum DisplayType
    {
        Error,
        LinkCode,
        ConnectionStatus
    }

    public enum DisplayTarget
    {
        PlexConnectionStatus
    }

    #endregion

    #region Interfaces

    public interface IActionPayload
    {
        ActionType Action { get; }
    }

    public interface IServiceCommand
    {
        ActionType Action { get; }
        HttpRequestMessage Request { get; }
    }

    public interface IResponsePayload
    {
        ActionType Action { get; }
    }

    public interface IDisplayCommand
    {
        DisplayType Display { get; }
    }
    #endregion

    #region ActionPayloads

    public class RegisterDeviceAction : IActionPayload
    {
        public ActionType Action => ActionType.RegisterDevice;
    }

    #endregion

    #region ServiceCommands

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

    #region ResponsePayloads

    public class RegisterDeviceResponse : IResponsePayload
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
            return $"RegisterDeviceResponse: Code=\"{Code ?? "[null]"}\", Id=\"{PinId ?? "[null]"}\", ExpiresAt=\"{ExpiresAt ?? "[null]"}\"";
        }
    }
    #endregion

    #region DisplayPayloads

    public class LinkCodeDisplay : IDisplayCommand
    {
        public DisplayType Display => DisplayType.LinkCode;

        public string Message;
        public List<TMPLinkAction> LinkActions;

        public override string ToString()
        {
            string linkInfo = (LinkActions != null && LinkActions.Count > 0)
                ? string.Join(", ", LinkActions.Select(a => $"[linkId={a.linkId}, url={a.url}]"))
                : "[no link actions]";

            return $"[DisplayType: {Display}, Message: \"{Message}\", Links: {linkInfo}]";
        }

    }

    #endregion

    public class TMPLinkAction
    {
        public string linkId;
        public string url;
        public Color hoverColor;
    }

    public static class HttpRequestBuilder
    {
        public static HttpRequestMessage CreateRegisterRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://plex.tv/api/v2/pins.xml");
            AttachHeaders(request);

            return request;
        }

        private static void AttachHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Accept", "Application/XML");
            request.Headers.Add("X-Plex-Client-Identifier", Managers.PlexSessionManager.GetClientID());
            request.Headers.Add("X-Plex-Product", Application.productName);
            request.Headers.Add("X-Plex-Version", Application.version);
            request.Headers.Add("X-Plex-Platform", Application.platform.ToString());
            request.Headers.Add("X-Plex-Device-Name", SystemInfo.deviceName);
        }
    }

    public static class ServiceUtility
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task<string> ExecuteAsync(HttpRequestMessage request)
        {
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }

    public static class ResponseParser
    { 
        public static Dictionary<string, string> ParseRegisterDevice(string rawXml)
        {
            var doc = XDocument.Parse(rawXml);
            DevLog.Internal($"XDocument created {doc}");

            var code = doc.Root?.Attribute("code")?.Value ?? Helper.MissingContent("code");
            var id = doc.Root?.Attribute("id")?.Value ?? Helper.MissingContent("id");
            var utcTimestamp = doc.Root?.Attribute("expiresAt")?.Value ?? Helper.MissingContent("expiresAt");

            var cleanedUtc = utcTimestamp.Replace(" UTC", "").Trim();

            DateTime utcDateTime = DateTime.ParseExact(
                cleanedUtc,
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal |
                System.Globalization.DateTimeStyles.AdjustToUniversal
            );

            DateTime localTime = utcDateTime.ToLocalTime();

            string formattedLocal = localTime.ToString("hh:mm tt");

            DevLog.Internal($"Extracted Data : Code = {code} | ID = {id} | Expires = {formattedLocal}");

            return new Dictionary<string, string>
            {
                { "code", code},
                { "id", id},
                { "expires", formattedLocal}
            };
        }
    }

    public static class DataExtractor
    {
        public static Dictionary<string, string> ExtractRegistrationDevice(RegisterDeviceResponse response)
        {
            Managers.PlexSessionManager.SavePinID(response.PinId);

            var code = response.Code;
            var id = response.PinId;
            var expiresAt = response.ExpiresAt;
            var color = ColorUtility.ToHtmlStringRGB(StyleManager.Instance.hoverLinkColor);

            DevLog.Internal($"Extracted Data : Code = {code} | ID = {id} | expiresAt = {expiresAt} | color = {color}");

            return new Dictionary<string, string>
            {
                { "code", response.Code },
                { "id", response.PinId },
                { "expiresAt", response.ExpiresAt },
                { "linkId", "plex_link" },
                { "url", "https://plex.tv/link" },
                { "hoverColor", color}
            };
        }
    }
    public static class Helper
    {
        public static string MissingContent(string element)
        {
            DevLog.Error($"Missing expected field ({element}) in RegisterDevice response");
            return $" {element} Not Found";
        }

    }
}
