using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only gameplay")] 
    public TileBase tile;
    // public ItemType type;
    // public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    
    [Header("Only  UI")]
    public bool stackable = true;

    [Header("Both")] 
    public NameItem nameItem;
    public ItemType itemType;
    public Sprite image;
    public Sprite equipedImage;
    
    
    public enum NameItem
    {
        Sniper1,
        AR4,
    }
    
    public enum ItemType
    {
        MainGun,
        MiniGun,
        Enegy,
        Food,
    }

}
