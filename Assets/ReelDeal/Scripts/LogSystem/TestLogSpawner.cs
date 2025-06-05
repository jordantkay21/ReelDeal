using KayosTech.ReelDeal.Prototype.LogSystem;
using NUnit.Framework.Internal;
using UnityEngine;

public class TestLogSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DevLog.Info("This is an informational test log.", "TEST");
        DevLog.Success("This is a success log!", "TEST");
        DevLog.Warning("This is a warning log!", "TEST");
        DevLog.Error("This is an error log!", "TEST");
        DevLog.Urgent("This is an urgent log!", "TEST");
    }

}
