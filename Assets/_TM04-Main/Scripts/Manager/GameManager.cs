using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
           
    }
    
    public void OpenCanvas(string a)
    {
        if (GameCanvasManager.Instance.CanvasList.ContainsKey(a))
        {
            GameCanvasManager.Instance.CanvasList[a].Show();
        }
        else
        {
            Debug.LogError("UIError: Canvas not found: " + a);
        }
    }
}
