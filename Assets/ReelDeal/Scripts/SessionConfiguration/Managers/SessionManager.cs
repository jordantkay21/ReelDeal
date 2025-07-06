using KayosTech.ReelDeal.Prototype.LogSystem;
using KayosTech.ReelDeal.Prototype.SessionConfig;
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
                DevLog.Internal($"PinID set for Device registration: {PinID}", $"Session Manager | Data Saved");
            }
        }

        private static string _authToken;
        public static string AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                DevLog.Internal($"AuthToken set for Account Authorization : {AuthToken}", $"Session Manager | Data Saved");
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