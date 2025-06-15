using System.Net.Http;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Enums;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Managers;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Enums
{
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
}

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Interfaces
{


    public interface IResponsePayload
    {
        ActionType Action { get; }
        bool WasSuccessful { get; protected set; }
        string ErrorMessage { get; protected set; }
    }
}

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Utilities
{
    public static class Utilities
    {
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
                request.Headers.Add("X-Plex-Client-Identifier", PlexSessionManager.GetClientID());
                request.Headers.Add("X-Plex-Product", Application.productName);
                request.Headers.Add("X-Plex-Version", Application.version);
                request.Headers.Add("X-Plex-Platform", Application.platform.ToString());
                request.Headers.Add("X-Plex-Device-Name", SystemInfo.deviceName);
            }
        }

    }
}
