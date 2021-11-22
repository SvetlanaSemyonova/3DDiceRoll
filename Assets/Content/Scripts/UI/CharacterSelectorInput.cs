using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Content.Scripts.UI
{
    public class CharacterSelectorInput : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public Action<float> onPointerDrag;
        public Action onEndDrag;

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                onPointerDrag?.Invoke(-eventData.delta.x / 6);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke();
        }
    }
}