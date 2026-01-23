using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LRProj : Projectile
{
    protected override void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
        if (collisionCheck())
        {
            die();
        }

        // if (rb.linearVelocityX > 0) //Facing direction
        // {
        //     transform.eulerAngles = new Vector3(0, 0, 0); // Normal
        // }
        // else if (rb.linearVelocityX < 0)
        // {
        //     transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
        // }
    }
    protected override void ExtraStart()
    {
        if (direction == -1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
