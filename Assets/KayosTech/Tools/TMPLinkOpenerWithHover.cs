using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace KayosTech.Components
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPLinkOpenerWithHover : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
    {
        [System.Serializable]
        public class TMPLinkAction
        {
            public string linkId;
            public string url;
            public Color hoverColor = Color.yellow;
        }

        [SerializeField] private TMPLinkAction[] linkActions;
        [TextArea] [SerializeField] private string originalText;

        private TextMeshProUGUI textMesh;
        private int lastLinkIndex = -1;

        void Awake()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
            if (originalText == null) originalText = textMesh.text;
            textMesh.text = originalText;
        }

        void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMesh, mousePos, null);

            if (linkIndex != lastLinkIndex)
            {
                RestoreAllLinkColors();
                if (linkIndex != -1)
                {
                    var id = textMesh.textInfo.linkInfo[linkIndex].GetLinkID();
                    var action = linkActions.FirstOrDefault(a => a.linkId == id);

                    if (action != null)
                        HighlightLink(linkIndex, action.hoverColor);
                }

                lastLinkIndex = linkIndex;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMesh, eventData.position, null);
            
            if (linkIndex != -1)
            {
                var linkInfo = textMesh.textInfo.linkInfo[linkIndex];
                string id = linkInfo.GetLinkID();
                var action = linkActions.FirstOrDefault(a => a.linkId == id);
                if (action != null)
                {
                    HighlightLink(linkIndex, action.hoverColor);
                }
            }

            lastLinkIndex = linkIndex;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            RestoreAllLinkColors();
            lastLinkIndex = -1;
        }

        private void HighlightLink(int index, Color highlighColor)
        {
            TMP_LinkInfo linkInfo = textMesh.textInfo.linkInfo[index];
            textMesh.text = textMesh.text.Replace(linkInfo.GetLinkText(), $"<color=#{ColorUtility.ToHtmlStringRGB(highlighColor)}>{linkInfo.GetLinkText()}</color>");
        }

        private void RestoreAllLinkColors()
        {
            textMesh.text = originalText;
        }
    }
}