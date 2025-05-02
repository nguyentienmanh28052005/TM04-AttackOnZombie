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
        _crateShow.SetActive(false);
        _playerShow.SetActive(true);
    }
}
