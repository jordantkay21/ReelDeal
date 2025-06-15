using System;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Enums;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Interfaces;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Factory
{
    public interface IActionPayload
    {
        ActionType Action { get; }
    }

    #region Payloads
    public class RegisterDevicePayload : IActionPayload
    {
        public ActionType Action => ActionType.RegisterDevice;
    }
    #endregion

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

        private static RegisterDevicePayload CreateRegisterDevicePayload()
        {
            return new RegisterDevicePayload();
        }
    }
}
