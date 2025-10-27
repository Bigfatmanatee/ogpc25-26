using System;
using System.Collections;
using UnityEngine;

public class LREnemy : Enemy
{
    protected override void move()
    {
        if (checkForWall() || !checkForFloor())
        {
            toggleDirection();
        }
        else
        {
            attack();
        }
    }


    
}
