using System;
using UnityEditor;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Managers
{
    public static class PlexSessionManager
    {
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
