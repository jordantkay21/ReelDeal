using System.Net.Http;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype
{
    public enum ActionType
    {
        RegisterDevice
    }
    public enum DisplayType
    {
        LinkCode
    }

    #region InteractionDTO
    public interface IInteractionDTO
    {
        ActionType Action { get; }
    }

    public sealed class RegisterDeviceIntent:IInteractionDTO
    {
        public ActionType Action => ActionType.RegisterDevice;
    }
    #endregion

    #region Service Handler

    public interface IInteractionHandler
    {
        public void HandleInteraction();
    }
    #endregion


    #region Display Handler
    public interface IDisplayHandler
    {
        DisplayType Display { get; }
    }
    #endregion
}
