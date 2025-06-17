using UnityEngine;

using UnityEngine;
using TMPro;

namespace KayosTech.Styles
{
    public class StyleManager : MonoBehaviour
    {
        public static StyleManager Instance { get; private set; }

        [Header("Fonts")]
        public TMP_FontAsset titleFont;
        public TMP_FontAsset buttonFont;
        public TMP_FontAsset headerFont;
        public TMP_FontAsset subtitleFont;
        public TMP_FontAsset bodyFont;
        public TMP_FontAsset logFont;

        [Header("Link Colors")]
        public Color defaultLinkColor = new Color32(0x00, 0xFF, 0xFF, 0xFF); // Active Cyan
        public Color hoverLinkColor = new Color32(0x66, 0xE6, 0xFF, 0xFF);   // Cool Sky Cyan

        [Header("Log Message Colors")]
        public Color successText = new Color32(0xA8, 0xF3, 0xC3, 0xFF);
        public Color successBackground = new Color32(0x32, 0xD4, 0x75, 0xFF);

        public Color errorText = new Color32(0xFF, 0x9A, 0x9A, 0xFF);
        public Color errorBackground = new Color32(0xE9, 0x4F, 0x4F, 0xFF);

        public Color alertText = new Color32(0xFF, 0xE0, 0xA6, 0xFF);
        public Color alertBackground = new Color32(0xFF, 0xB3, 0x47, 0xFF);

        public Color infoText = new Color32(0xAE, 0xE6, 0xFB, 0xFF);
        public Color infoBackground = new Color32(0x4F, 0xC3, 0xF7, 0xFF);

        public Color urgentText = new Color32(0xF2, 0xAE, 0xE0, 0xFF);
        public Color urgentBackground = new Color32(0xD1, 0x47, 0xA3, 0xFF);

        private void Awake()
        {
            // Enforce singleton instance
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

