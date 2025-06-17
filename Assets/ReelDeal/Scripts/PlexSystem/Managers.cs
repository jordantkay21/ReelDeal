using System;
using KayosTech.ReelDeal.Prototype.LogSystem;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic
{
    public static class Managers
    {
        public static class PlexSessionManager
        {
            private static string PinID;
            public static void SavePinID(string pinId)
            {
                PinID = pinId;
                DevLog.Internal($"PinID set for Device Registration: {PinID}", "Registration Process");
            }
            public static string GetPinID()
            {
                return PinID;
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
}
