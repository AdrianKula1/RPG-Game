using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .6f;
    }    
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }  
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
}
