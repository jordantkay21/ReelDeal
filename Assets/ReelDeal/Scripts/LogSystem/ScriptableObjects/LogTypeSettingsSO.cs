using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem.Settings
{
    [CreateAssetMenu(menuName = "RealDeal/Logging/Log Type Settings")]
    public class LogTypeSettingsSO : ScriptableObject
    {
        public AppLogType type;
        public bool showInUI = true;

        [Range(1f,30f)]
        public float duration = 5f;
        [Range(0f,5f)]
        public float fadeDuration = 1f;

        public Color primaryColor = Color.gray;
        public Color secondaryColor = Color.black;
    }
}