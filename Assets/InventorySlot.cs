using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public int index;
    public Image image;
    public Color selectColor, notSelectColor;
    private InventoryItem item;

    private void Awake()
    {
        Deselect();
    }

    public void Change()
    {
        InventoryManager.Instance.ChangeSelected(index);
        item = GetComponentInChildren<InventoryItem>();
        if (item != null)
        {
            
        }
    }
    
    public void Select()
    {
        image.color = selectColor;
    }

    public void Deselect()
    {
        image.color = notSelectColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform; 
        }
    }
}
