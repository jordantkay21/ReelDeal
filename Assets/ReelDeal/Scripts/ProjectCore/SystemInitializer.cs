using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Backend;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Frontend;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.Bootstrap
{
    [DefaultExecutionOrder(-100)]
    public class SystemInitializer : MonoBehaviour
    {
        [Tooltip("Check to enable logging system initialization on Awake.")]
        public bool autoInitialize = true;

        private void Awake()
        {
            if (autoInitialize)
            {
                LogRouter.Initialize();
                Debug.Log("[LogSystem] Router initialized and subscribed to DevLog.");
                LogStorage.Initialize();
                Debug.Log("[LogSystem] StorageCoordinator initialized and subscribed to Application.quitting.");
            }
        }
    }
}
