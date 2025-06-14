using KayosTech.ReelDeal.Prototype.LogSystem;
using NUnit.Framework.Internal;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.Core.Testing
{
    public class TestLogSpawner : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            DevLog.Internal("This is an internal test log.", "Internal Test");
            DevLog.Info("This is an informational test log.", "Info Test");
            DevLog.Success("This is a success log!", "Success Test");
            DevLog.Warning("This is a warning log!", "Warning Test");
            DevLog.Error("This is an error log!", "Error Test");
            DevLog.Urgent("This is an urgent log!", "Urgent Test");
        }

    }
}