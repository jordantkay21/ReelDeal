using KayosTech.ReelDeal.Prototype.LogSystem;
using KayosTech.ReelDeal.Prototype.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using UnityEngine;
using static KayosTech.Components.TMPLinkOpenerWithHover;

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
            DevLog.Highlight($"7 - Service Response received: {response}", "Data Flow");
            var command = DisplayCommandFactory.CreateFrom(response);
            EventManager.DispatchDisplayCommand(command);
        }

    }
    #region Service Command Checkpoint
    #region Service Command TPOs
    public interface IServiceCommand
    {
        ActionType Action { get; }
        HttpMethod Method { get; }
        string Uri { get; }
        public HttpRequestMessage BuildRequest()
        {
            string pin = SessionManager.PinID;
            var request = new HttpRequestMessage(Method, Uri);
            HTTPHelpers.AttachHeaders(request);
            return request;
        }
    }

    public class RegisterDeviceCommand : IServiceCommand
    {
        public ActionType Action => ActionType.RegisterDevice;
        public HttpMethod Method => HttpMethod.Post;
        public string Uri => "https://plex.tv/api/v2/pins.xml";
    }
    public class PollAuthCommand : IServiceCommand
    {
        public ActionType Action => ActionType.PollAuth;
        public HttpMethod Method => HttpMethod.Get;
        public string Uri { get; }

        public PollAuthCommand()
        {
            string pin = SessionManager.PinID;

            if (pin == null)
                throw new NullReferenceException("Failed to retrieve PinID from SessionManager");

            Uri = $"https://plex.tv/pins/{pin}.xml";
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
                    return new RegisterDeviceCommand();
                case Action.PollAuthAction pollAuth:
                    return new PollAuthCommand();
                default:
                    throw new NotSupportedException($"Unsupported Service Action: {action.GetType().Name}");
            }
        }
    }
    #region Utilities
    #endregion

    #region Helpers
    public static class HTTPHelpers
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

    #endregion

    #region Display Command Checkpoint
    #region Display Command TPOs
    public interface IDisplayCommand
    {
        DisplayType Display { get; }
    }

    public class LinkCodeDisplayCommand : IDisplayCommand
    {
        public DisplayType Display => DisplayType.LinkCode;

        public string Code;
        public string ID;
        public string ExpiresAt;

        public readonly string Url = "https://plex.tv/link";

        public LinkCodeDisplayCommand(string code, string id, string expiresAt, Color hoverColor)
        {
            Code = code;
            ID = id;
            ExpiresAt = expiresAt;
        }


        public override string ToString()
        {
            return $"Display Command: DisplayType [{Display}] " +
                $"\n DATA: Code [{Code}] | ID [{ID}] | ExpiresAt [{ExpiresAt}] | URL [{Url}]"; 
        }
    }

    public class AccountAuthSuccess : IDisplayCommand
    {
        public DisplayType Display => DisplayType.AccountAuthSuccess;
    }

    public class AccountAuthProcessing : IDisplayCommand
    {
        public DisplayType Display => DisplayType.AuthPollStatus;
    }
    #endregion

    #region Display Command DTOs
    public interface IDisplayDTO { }

    public class LinkCodeDTO : IDisplayDTO
    {
        public string Code;
        public string ID;
        public string ExpiresAt;
        public Color HoverColor;
    }
    #endregion

    public static class DisplayCommandFactory
    {
        public static IDisplayCommand CreateFrom(Response.IServiceResponse response)
        {
            switch (response.Action)
            {
                case ActionType.RegisterDevice:
                    return CreateLinkIDDisplayCommand(response.Response);
                case ActionType.PollAuth:
                    if (ResponseParserUtility.ParseAuthPollResponse(response.Response))
                    {
                        EventManager.IssueStopPollingRequest();
                        return new AccountAuthSuccess();
                    }
                    return new AccountAuthProcessing();
                default:
                    throw new NotSupportedException($"Unsupported Service Response: {response}"); ;
            }
        }

        private static LinkCodeDisplayCommand CreateLinkIDDisplayCommand(string rawResponse)
        {
            var dto = new LinkCodeDTO();
            ResponseParserUtility.ParseRegisterDevice(rawResponse, dto);

            return new LinkCodeDisplayCommand(dto.Code, dto.ID, dto.ExpiresAt, dto.HoverColor);
        }
    }

    #region Utilities

    public static class ResponseParserUtility
    {
        public static void ParseRegisterDevice(string response, LinkCodeDTO dto)
        {
            var doc = XDocument.Parse(response);

            dto.Code = DataExtractorHelpers.ExtractData(doc, "code");
            dto.ID = DataExtractorHelpers.ExtractData(doc, "id");
            dto.ExpiresAt = DataExtractorHelpers.ExtractData(doc, "expiresAt");
            dto.HoverColor = Managers.StyleManager.Instance.hoverLinkColor;
            
            Managers.SessionManager.PinID = dto.ID;
        }

        public static bool ParseAuthPollResponse(string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                var token = doc.Root?.Element("auth-token")?.Value;

                if (!string.IsNullOrEmpty(token))
                {
                    SessionManager.AuthToken = token;
                    return true;
                }

                DevLog.Internal("No Auth Token Available");
                return false;
            }
            catch
            {
                DevLog.Internal("Error Occured during Token Retrieval");
                return false;
            }
        }
    }

    #endregion

    #region Helpers
    public static class DataExtractorHelpers
    {
        public static string ExtractData(XDocument doc, string attribute)
        {
            return doc.Root?.Attribute(attribute)?.Value ?? MissingContent(attribute);
        }
        public static string MissingContent(string attribute)
        {
            throw new InvalidOperationException($"Expected attribute {attribute} was not found in the XML document.");
        }
    }
    #endregion

    #endregion
}