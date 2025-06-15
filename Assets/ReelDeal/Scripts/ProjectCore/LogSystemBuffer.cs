using System.Collections;
using System.Collections.Generic;
using KayosTech.ReelDeal.Prototype.LogSystem.DataStructure;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem
{
    public static class LogSystemBuffer
    {
        private static readonly Queue<LogActionPayload> PendingLogs = new();
        public static bool IsReady { get; private set; } = false;

        public static void Enqueue(LogActionPayload payload)
        {
            PendingLogs.Enqueue(payload);
        }

        public static void Flush()
        {
            IsReady = true;

            while (PendingLogs.Count > 0)
            {
                var payload = PendingLogs.Dequeue();
                DevLog.ForceRoute(payload);
            }
        }
    }
}