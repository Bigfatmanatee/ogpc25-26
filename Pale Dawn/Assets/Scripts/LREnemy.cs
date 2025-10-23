using System;
using System.Collections;
using UnityEngine;

public class LREnemy : Enemy
{
    protected override void move()
    {
        if (base.checkForWall() || !base.checkForFloor())
        {
            base.toggleDirection();
        }
        else
        {
            base.attack();
        }
    }


    
}
