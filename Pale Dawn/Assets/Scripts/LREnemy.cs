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

    public override void damage(GameObject player)
    {
        if (InvSec >= maxInvSec)
        {
            health -= player.GetComponent<Player>().getDamage();
            Debug.Log("After damage taken, Health:" + health);
            print("PUSHHBACKKK");
            //play Iframe animation
            StartCoroutine(pulse());
            transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0) * 10, ForceMode2D.Impulse);
            pushback = true;
            InvSec = 0;
        }
        else
        {
            Debug.Log("didnt take damage, still invincible");
        }
    }
    
}
