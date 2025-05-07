using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInventory : CanvasBase
{

    [SerializeField] private GameObject _playerShow;
    [SerializeField] private GameObject _crateShow;

    public override void Hide()
    {
        base.Hide(); 
    }

    public override void Show(object data = null)
    {
        base.Show(data);
        if (InventoryManager.Instance.currentTypeShow == InventoryManager.TypeShow.Inventory)
        {
            _playerShow.SetActive(true);
            _crateShow.SetActive(false);
        }
        else if (InventoryManager.Instance.currentTypeShow == InventoryManager.TypeShow.Crate)
        {
            _playerShow.SetActive(false);
            _crateShow.SetActive(true);
        }
    }
}
