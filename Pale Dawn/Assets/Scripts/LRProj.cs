using System;
using System.Collections;
using UnityEngine;

public class LRProj : Enemy
{
    protected override void move()
    {
        if (checkForWall() || !checkForFloor())
        {
            die();
        }
        else
        {
            attack();
        }
    }
    protected override IEnumerator swing()
    {
        //play animation
        // anim.SetBool("attacking", true);
        yield return new WaitForSeconds(0f); //should match animation
        if (shouldSwing && target.GetComponent<Player>() != null)
        {
            // Debug.Log("sending damage to " + target);
            target.GetComponent<Player>().damage(this.gameObject);
        }
        // anim.SetBool("attacking", false);
    }
    protected override void FixedUpdate()
    {
        if (!shouldSwing)
        {
            rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
        }

        if (rb.linearVelocityX > 0) //Facing direction
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Normal
        }
        else if (rb.linearVelocityX < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
        }
    }
}
