using System;
using KayosTech.ReelDeal.Prototype.LogSystem.Payload;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KayosTech.ReelDeal.Prototype.LogSystem.Frontend
{
    /// <summary>
    /// Controls the appearance and lifecycle of a single UI log message
    /// </summary>
    public class LogDisplayHandler : MonoBehaviour
    {
        public TextMeshProUGUI logText;
        public Outline logOutline;
        public CanvasGroup logCanvas;

        private float timer;
        private float fadeDuration;

        public void Initialize(LogDisplayPayload logDisplay)
        {
            logText.color = logDisplay.textColor;
            logText.text = logDisplay.message;
            logOutline.effectColor = logDisplay.outlineColor;
            timer = logDisplay.duration;
            fadeDuration = logDisplay.fadeDuration;
        }

        private void Update()
        {
            timer -= Time.deltaTime;

            if (timer <= fadeDuration)
            {
                logCanvas.alpha = Mathf.Clamp01(timer / fadeDuration);
            }

            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
