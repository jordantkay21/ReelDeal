using System;
using System.Collections.Generic;
using System.Linq;
using KayosTech.ReelDeal.Prototype.LogSystem;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic
{
    public static class ActionPayloadFactory
    {
        public static IActionPayload Create(ActionType type, string rawData = null)
        {
            switch (type)
            {
                case ActionType.RegisterDevice:
                    return CreateRegisterDevicePayload();
                default:
                    throw new NotSupportedException($"Unsupported action type: {type}");
            }
        }

        private static RegisterDeviceAction CreateRegisterDevicePayload()
        {
            return new RegisterDeviceAction();
        }
    }

    public static class ServiceCommandFactory
    {
        public static IServiceCommand CreateFrom(IActionPayload action)
        {
            switch (action)
            {
                case RegisterDeviceAction registerDevice:
                    var request = HttpRequestBuilder.CreateRegisterRequest();
                    return new RegisterDeviceCommand(request);
                default:
                    throw new NotSupportedException($"Unsupported action type: {action.GetType().Name}");
            }
        }
    }

    public static class ResponsePayloadFactory
    {
        public static IResponsePayload Create(ActionType type, string rawData)
        {
            switch (type)
            {
                case ActionType.RegisterDevice:
                    return CreateRegistrationResponse(rawData);
                default:
                    throw new NotSupportedException($"Unsupported action type: {type}");
            }
        }

        private static IResponsePayload CreateRegistrationResponse(string rawData)
        {
            var values = ResponseParser.ParseRegisterDevice(rawData);

            string code = values.TryGetValue("code", out var c) ? c : Helper.MissingContent("code");
            string id = values.TryGetValue("id", out var i) ? i : Helper.MissingContent("id");
            string expires = values.TryGetValue("expires", out var e) ? e : Helper.MissingContent("expires");

            var response = new RegisterDeviceResponse(
                code,
                id,
                expires);

            return response;
        }
    }

    public static class DisplayCommandFactory
    {
        public static IDisplayCommand CreateFrom(IResponsePayload response)
        {
            switch (response)
            {
                case RegisterDeviceResponse registerDevice:
                    return CreateLinkIdDisplay(registerDevice);
                default:
                    throw new NotSupportedException($"Unsupported response type {response.GetType()}");
            }
        }

        private static IDisplayCommand CreateLinkIdDisplay(RegisterDeviceResponse response)
        {
            var data = DataExtractor.ExtractRegistrationDevice(response);

            DevLog.Internal($"Data Received: {string.Join(", ", data.Select(kv => $"{kv.Key}={kv.Value}"))}");

            Color hoverColor = ColorUtility.TryParseHtmlString("#" + data["hoverColor"], out var color)
                ? color
                : Color.yellow;

            var linkAction = new TMPLinkAction
            {
                linkId = data["linkId"],
                url = data["url"],
                hoverColor = hoverColor
            };

            string message =
                $"To link your device, visit <link=\"{data["linkId"]}\">{data["url"]}</link>\n" +
                $"Enter the code: <b>{data["code"]}</b>\n" +
                $"This code expires at: {data["expiresAt"]}";


            var displayCommand = new LinkCodeDisplay
            {
                Message = message,
                LinkActions = new List<TMPLinkAction> { linkAction }
            };

            DevLog.Internal($"Link ID Display Command Constructed : {displayCommand}");
            return displayCommand;
        }
    }
}
