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

        public void Initialize(LogEntry log)
        {
            timer = log.DisplayTime;
            messageText.text = log.Message;
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