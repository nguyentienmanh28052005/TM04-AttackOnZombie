using System;
using System.Collections.Generic;
using System.IO;
using Pixelplacement;
using UnityEngine;


//note
// key : 1
// coin : 2
// maxHealth : 20
// damage skill thường : 21
//damage skill ulti : 22
// enegy : 23
// LevelAt : 30;
// key coin = 1 + coin thứ + tên round
//key key = 2 + teen round
//Key  Round boss = 30 + atRound (0,  1)
[System.Serializable]
public class DataItem 
{
    public List<item> items;
}

[System.Serializable]
public class item
{
    public int key;
    public float value;
}
public class SaveDataPlayer : Singleton<SaveDataPlayer>
{
    public DataItem dateItem;

    public int key;
    public float value;

    private void Start()
    {
       LoadData(); 
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            //SaveData();
            Debug.Log("Save");
            Save(key, value);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            //SaveData();
            Debug.Log("Load");
            Value(key);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            ResetData();
        }
    }

    public void Save(int key, float value)
    {
        foreach (var nameKey in dateItem.items)
        {
            if (nameKey.key == key)
            {
                    nameKey.value = value;
                    SaveData();
                    return;
            }
        }

        item item = new item();
        item.key = key;
        item.value = value;

        dateItem.items.Add(item);
        SaveData();
    }

    public void ResetData()
    {
        
    }

    public float Value(int key)
    {
        foreach (var nameKey in dateItem.items)
        {
            if (nameKey.key == key)
            {
                //Debug.Log("Load key value: " + key + " " + nameKey.value);
                return nameKey.value;
            }
        }
        Debug.Log("Load key value: " + key + " " + 0);
        return 0;
    }
    public void LoadData()
    {
        string file = "save.json";
        string filePath = Path.Combine(Application.persistentDataPath, file);

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
            ResetData();
        }
        dateItem = JsonUtility.FromJson<DataItem>(File.ReadAllText(filePath));
        Debug.Log("Load Done!");
    }

    public void SaveData()
    {
        string file = "save.json";
        string filePath = Path.Combine(Application.persistentDataPath, file);

        string json = JsonUtility.ToJson(dateItem, true);
        File.WriteAllText(filePath, json);
        
        Debug.Log("File saved, at path: " + filePath);
    }
}
