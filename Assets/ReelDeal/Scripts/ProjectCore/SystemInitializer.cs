using System.Collections;
using KayosTech.ReelDeal.Prototype.LogSystem;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Backend;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Frontend;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Manager;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.Core.Bootstrap
{
    [DefaultExecutionOrder(-100)]
    public class SystemInitializer : MonoBehaviour
    {
        [Tooltip("Check to enable logging system initialization on Awake.")]
        public bool autoInitialize = true;

        private IEnumerator Start()
        {
            if (autoInitialize)
            {
                LogRouter.Initialize();
                LogStorage.Initialize();

                yield return new WaitUntil(() => LogSettingsManager.Instance != null); 

                LogSystemBuffer.Flush();

                DevLog.Internal("Log System Initialized and Log Buffer flushed", "BootStrap");
            }
        }
    }
}
