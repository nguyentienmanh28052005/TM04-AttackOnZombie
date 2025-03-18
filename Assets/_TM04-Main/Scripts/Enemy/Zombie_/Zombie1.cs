using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1 : AEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        MoveToGameObject(_player);
    }
    
    
    
}
