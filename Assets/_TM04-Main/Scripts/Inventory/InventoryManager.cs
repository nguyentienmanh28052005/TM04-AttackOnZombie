using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public int maxStackedItem = 4;
    public InventorySlot[] inventorySlots;
    public InventorySlot[] crateSlots;
    public GameObject inventoryItemPrefab;

    public TypeShow currentTypeShow;

    public InventorySlot _currentSlot;
    public Crate currentCrate;
    public InventorySlot lastCrateSlot;
    public Item[] currentItemCrate;
    
    private int selectedSlot = -1;
    
    public enum TypeShow
    {
        Inventory,
        Crate,
    }

    protected override void Awake()
    {
        base.Awake();
    }
    
    private void Start()
    {
        ChangeSelected(0);
        SetIndex();
    }

    public void ChangeSelected(int newValue)
    {
        if (selectedSlot >= 0)
        {
            if (selectedSlot < 25) inventorySlots[selectedSlot].Deselect();
            else crateSlots[selectedSlot - 25].Deselect();
        }
        
        if(newValue < 25) inventorySlots[newValue].Select();
        else crateSlots[newValue-25].Select();
        selectedSlot = newValue;
    }

    private void SetIndex()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventorySlot slotCrate = crateSlots[i];
            slot.index = i;
            slotCrate.index = i + 25;
        }
    }

    public void SetItemInSlotCrate(int index, Item item)
    {
        
    }

    public void AddItemCrate(Item[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                if (crateSlots[i].transform.childCount == 0)
                {
                    SpawnNewItem(items[i], crateSlots[i]);
                }
            } 
        }
    }

    public void DestroyAllItemInCrate()
    {
        foreach (var slot in crateSlots)
        {
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            if(item != null) Destroy(item.gameObject);
        }
    }
    
    public bool AddItem(Item item)
    {
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < 4 &&
                itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
