using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem.Payload
{
    public class LogDisplayPayload : IResponsePayload
    {
        public DataFlow dataFlow => DataFlow.Frontend;

        public string message { get; }
        public Color textColor { get; }
        public Color outlineColor { get; }
        public float duration { get; }
        public float fadeDuration { get; }


        public LogDisplayPayload(string message, Color textColor, Color outlineColor, float duration, float fadeDuration)
        {
            this.message = message;
            this.textColor = textColor;
            this.outlineColor = outlineColor;
            this.duration = duration;
            this.fadeDuration = fadeDuration;
        }
    }
}