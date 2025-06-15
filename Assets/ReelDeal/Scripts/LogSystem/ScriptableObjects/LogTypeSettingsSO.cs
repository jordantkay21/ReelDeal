using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem.Settings
{
    [CreateAssetMenu(menuName = "RealDeal/Logging/Log Type Settings")]
    public class LogTypeSettingsSO : ScriptableObject
    {
        [Header ("Log Settings")]
        public AppLogType type;
        public bool showInUI = true;

        [Header("Log Time Options ")]
        [Range(1f,30f)]
        public float duration = 5f;
        [Range(0f,5f)]
        public float fadeDuration = 1f;

        [Header("Log Display Customization")]
        public Color primaryColor = Color.gray;
        public Color secondaryColor = Color.black;
    }
}