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
        if (base.shouldSwing && target.GetComponent<Player>() != null)
        {
            // Debug.Log("sending damage to " + target);
            target.GetComponent<Player>().damage(this.gameObject);
            die();
        }
        // anim.SetBool("attacking", false);
    }
    protected override void FixedUpdate()
    {
        if (!base.shouldSwing)
        {
            rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
        }
        else
        {
            Debug.Log("AHHHHHHHHHHHHHHHHH");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
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
