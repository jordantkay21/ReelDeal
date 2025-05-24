using System;
using TMPro;
using UnityEngine;

namespace KayosTech.Utilities.DebugTools
{
    /// <summary>
    /// Controls the appearance and lifecycle of a single UI log message.
    /// </summary>
    public class UILogMessage : MonoBehaviour
    {
        public TextMeshProUGUI messageText;
        public float fadeDuration = 0.5f;

        private CanvasGroup canvasGroup;
        private float timer;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Initialize(string message, LogLevel type)
        {
            float lifetime = type switch
            {
                LogLevel.Info => 3.0f,
                LogLevel.Success => 4.0f,
                LogLevel.Alert => 5.0f,
                LogLevel.Error => 6.0f,
                _ => 3.0f

            };

            timer = lifetime;
            messageText.text = message;
        }

        void Update()
        {
            timer -= Time.deltaTime;

            if (timer <= fadeDuration)
            {
                canvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            }

            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}