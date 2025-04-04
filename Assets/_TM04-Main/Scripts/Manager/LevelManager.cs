
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int  countEnemy = 0;
    public static int countEnemyDied = 0;
    public static int atLevel = 0;

    public void Update()
    {
        if (countEnemyDied > 100) atLevel = 1;
        if (countEnemyDied > 250) atLevel = 2;
        if (countEnemyDied > 400) atLevel = 3;
        if (countEnemyDied > 500) atLevel = 4;
        if (countEnemyDied > 600) atLevel = 5;
        if (countEnemyDied > 700) atLevel = 6;
        if (countEnemyDied > 1000) atLevel = 7;
        if (countEnemyDied > 2000) atLevel = 8;
    }
}
