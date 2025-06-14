using System;
using KayosTech.ReelDeal.Prototype.LogSystem.DataStructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KayosTech.ReelDeal.Prototype.LogSystem.Frontend
{
    /// <summary>
    /// Controls the appearance and lifecycle of a single UI log Message
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
            logCanvas = GetComponent<CanvasGroup>();
            if(logCanvas == null) Debug.LogWarning("log prefab does not contain a logCanvas which is needed for fading");

            logText.color = logDisplay.TextColor;
            logText.text = logDisplay.Message;
            logOutline.effectColor = logDisplay.OutlineColor;
            timer = logDisplay.Duration;
            fadeDuration = logDisplay.FadeDuration;
        }

        private void Update()
        {
            timer -= Time.deltaTime;

            if (timer <= fadeDuration && logCanvas != null)
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
