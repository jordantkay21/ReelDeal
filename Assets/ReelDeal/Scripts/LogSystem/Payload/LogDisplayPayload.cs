using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem.DataStructure
{
    public class LogDisplayPayload : IResponsePayload
    {
        public DataFlow DataFlow => DataFlow.Frontend;

        public string Message { get; }
        public Color TextColor { get; }
        public Color OutlineColor { get; }
        public float Duration { get; }
        public float FadeDuration { get; }


        public LogDisplayPayload(string message, Color textColor, Color outlineColor, float duration, float fadeDuration)
        {
            this.Message = message;
            this.TextColor = textColor;
            this.OutlineColor = outlineColor;
            this.Duration = duration;
            this.FadeDuration = fadeDuration;
        }
    }
}