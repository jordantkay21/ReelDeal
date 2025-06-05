using KayosTech.ReelDeal.Prototype.LogSystem.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem
{
    public interface IActionPayload{ }
    public interface ICommandPayload { }

    public interface IResponsePayload
    {
        DataFlow dataFlow { get; }
    }

    public enum DataFlow
    {
        Frontend,
        Backend,
        Cache
    }

    public enum AppLogType
    {
        Internal,
        Info,
        Success,
        Alert,
        Error,
        Urgent
    }

    public class Utilities
    {
        #region Nested Classes
        /// <summary>
        /// Assumes Namespace Format: {CompanyName}.{ProductName}.{Version}.{System}.{Layer}.{LayerComponent}
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public static string ExtractSystemNamespace(string scriptName)
        {
            if (string.IsNullOrWhiteSpace(scriptName))
                return "Unknown";

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var type = assembly.GetTypes().FirstOrDefault(t => t.Name == scriptName);
                if (type != null && !string.IsNullOrEmpty(type.Namespace))
                {
                    string[] parts = type.Namespace.Split('.');
                    return parts.Length >= 4 ? parts[3] : "Unknown"; // 3 = system
                }
            }

            return "Unknown";
        }


        #endregion

        public static string ColorToHex(Color color)
        {
            string hex = ColorUtility.ToHtmlStringRGBA(color);
            return hex;
        }
    }
}