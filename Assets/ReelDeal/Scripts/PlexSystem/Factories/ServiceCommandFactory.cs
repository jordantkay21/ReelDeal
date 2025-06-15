using System;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Bridge;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Interfaces;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Factory
{
    public static class ServiceCommandFactory
    {
        public static IServiceCommand CreateFrom(IActionPayload payload)
        {
            switch (payload)
            {
                case RegisterDevicePayload registerDevice:
                    var request = Utilities.Utilities.HttpRequestBuilder.CreateRegisterRequest();
                    return new RegisterDeviceCommand(request);
                default:
                    throw new NotSupportedException($"Unhandled action payload type: {payload.GetType().Name}");
            }
        }
    }
}
