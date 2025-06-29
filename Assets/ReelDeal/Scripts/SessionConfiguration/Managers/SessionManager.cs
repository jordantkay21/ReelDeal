using KayosTech.ReelDeal.Prototype.LogSystem;
using System;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.Managers
{
    public static class SessionManager
    {

        private static string _pinID;
        public static string PinID
        {
            get => _pinID;
            set
            {
                _pinID = value;
                DevLog.Internal($"PinID set for Device registration: {PinID}", $"Action:{ActionType.RegisterDevice}");
            }
        }      

        public static string GetClientID()
        {
            string key = "PlexClientID";

            if (!PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetString(key, Guid.NewGuid().ToString());
                PlayerPrefs.Save();
            }

            return PlayerPrefs.GetString(key);
        }
    }

}