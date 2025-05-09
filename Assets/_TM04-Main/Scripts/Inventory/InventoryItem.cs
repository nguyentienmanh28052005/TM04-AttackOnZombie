using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler,  IEndDragHandler
{

    [Header("UI")] 
    public Image image;

    public Text countText;
    
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public bool inventoryImage = true;

    
    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void ChangeImage(bool equiped)
    {
        if(equiped) image.sprite = item.equipedImage;
        else image.sprite = item.image;
    }
    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textAcitve = count > 1;
        countText.gameObject.SetActive(textAcitve);
    }

    public void SetScale(Vector3 newScale)
    {
        transform.localScale = newScale/3.5f;
    }

    public Item GetItem()
    {
        return item;
    }
    
    public void RefreshIMG(InventoryItem newItem, bool typeInventoryImage)
    {
        InventorySlot slot = GetComponentInParent<InventorySlot>();
        Vector3 baseScale = new Vector3(3.5f, 3.5f, 3.5f);
        Vector3 mainGunSlotScale = new Vector3(15f, 5f, 3.5f);
        if (typeInventoryImage)
        {
            if (slot.typeSlot == InventorySlot.TypeSlot.MainGun)
            {
                    MessageManager.Instance.SendMessage(new Message(ManhMessageType.nullWeapon));
            }
            SetScale(baseScale);
            image.sprite = item.image;
        }
        else
        {
            if (PlayerManager.Instance._currentWeaponType == ManhMessageType.EquipSniper1)
            {
                MessageManager.Instance.SendMessage(new Message(ManhMessageType.EquipSniper1));
            }
            else if (PlayerManager.Instance._currentWeaponType == ManhMessageType.EquipAR4)
            {
                MessageManager.Instance.SendMessage(new Message(ManhMessageType.EquipAR4));
            }
            image.sprite = item.equipedImage;
            SetScale(mainGunSlotScale);
        }
    }

    public void SetTypeImage(bool newTypeImage)
    {
        inventoryImage = newTypeImage;
    }

    public void SetImageItem()
    {
        InventorySlot slot = GetComponentInParent<InventorySlot>();
        Vector3 baseScale = new Vector3(3.5f, 3.5f, 3.5f);
        Vector3 mainGunSlotScale = new Vector3(15f, 5f, 3.5f);
        if (slot.typeSlot == InventorySlot.TypeSlot.BaseSlot)
        {
            SetScale(baseScale);
            image.sprite = item.image;
            inventoryImage = true;
        }
        else if (slot.typeSlot == InventorySlot.TypeSlot.MainGun)
        {
            SetScale(mainGunSlotScale);
            image.sprite = item.equipedImage;
            inventoryImage = false;
        }
    }

    public void MessagePlayer()
    {
        InventorySlot slot = GetComponentInParent<InventorySlot>();
        if (slot.typeSlot == InventorySlot.TypeSlot.MainGun)
        {
            if (item.nameItem == Item.NameItem.Sniper1)
            {
                MessageManager.Instance.SendMessage(new Message(ManhMessageType.EquipSniper1));
            }
            else if (item.nameItem == Item.NameItem.AR4)
            {
                MessageManager.Instance.SendMessage(new Message(ManhMessageType.EquipAR4));
            }
        }
    }
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        RefreshIMG(this, true);
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        InventoryManager.Instance._currentSlot = GetComponentInParent<InventorySlot>();
        if (InventoryManager.Instance._currentSlot.typeSlot == InventorySlot.TypeSlot.Crate)
        {
            InventoryManager.Instance.currentItemCrate[InventoryManager.Instance._currentSlot.index - 25] = null;
        }
        Debug.Log(InventoryManager.Instance._currentSlot.index);
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!inventoryImage)
        {
            RefreshIMG(this, false);
        }
        Debug.Log("OnEndDrag");
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        InventorySlot slot = GetComponentInParent<InventorySlot>();
        InventoryItem[] items = slot.GetComponentsInChildren<InventoryItem>();
        if (items.Length == 2)
        {
            items[0].transform.SetParent(InventoryManager.Instance._currentSlot.transform);
            items[0].MessagePlayer();
            items[0].SetImageItem();
            if(InventoryManager.Instance._currentSlot.typeSlot == InventorySlot.TypeSlot.Crate)
                InventoryManager.Instance.currentItemCrate[InventoryManager.Instance._currentSlot.index - 25] = items[0].item;
        }
        if (slot.typeSlot == InventorySlot.TypeSlot.Crate)
        {
            InventoryManager.Instance.currentItemCrate[slot.index - 25] = item;
        }
    }
}
