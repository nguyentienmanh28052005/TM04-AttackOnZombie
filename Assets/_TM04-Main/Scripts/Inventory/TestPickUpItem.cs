using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPickUpItem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    private void Start()
    {
        PickupItem(0);
        PickupItem(1);
    }

    public void PickupItem(int id)
    {
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if (result == true)
        {
            Debug.Log("Item added");
        }
        else
        {
            Debug.Log("Inventory Full");
        }
    }
}
