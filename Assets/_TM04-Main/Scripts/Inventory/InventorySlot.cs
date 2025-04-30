using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public TypeSlot typeSlot;
    public int index;
    public Image image;
    public Color selectColor, notSelectColor;
    private InventoryItem item;

    public enum TypeSlot
    {
        BaseSlot,
        MainGun,
        MiniGun,
    }

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

    public void RefreshIMG(InventoryItem newItem)
    {
        Vector3 mainGunSlotScale = new Vector3(15f, 5f, 3.5f);
        Vector3 baseScale = new Vector3(3.5f, 3.5f, 3.5f);
        
        if (typeSlot == TypeSlot.MainGun && newItem != null)
        {
            newItem.ChangeImage(true);
            newItem.SetScale(mainGunSlotScale);
        }
        else
        {
            newItem.SetScale(baseScale);
            newItem.ChangeImage(false);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryManager.Instance.ChangeSelected(index);
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform; 
            RefreshIMG(inventoryItem);
        }
    }
}
