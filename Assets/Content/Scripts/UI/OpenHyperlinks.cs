using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Content.Scripts.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class OpenHyperlinks : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            var tmPro = GetComponent<TextMeshProUGUI>();
            var linkIndex = TMP_TextUtilities.FindIntersectingLink(tmPro, Input.mousePosition, null);
            
            if (linkIndex != -1)
            {
                var linkInfo = tmPro.textInfo.linkInfo[linkIndex];
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }
    }
}