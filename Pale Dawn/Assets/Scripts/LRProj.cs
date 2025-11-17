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
        yield return new WaitForSeconds(0f);
        if (shouldSwing && target.GetComponent<Player>() != null)
        {
            target.GetComponent<Player>().damage(this.gameObject);
            die();
        }
    }
    public override void trigger(bool enter, GameObject gObject)
    {
        shouldSwing = true;
        Debug.Log("ShouldSwing set to true: " + shouldSwing);
        target = gObject.GetComponent<HitboxPass>().passHost();
        Debug.Log("Target saved as " + target);
    }
    protected override void FixedUpdate()
    {
        if (!shouldSwing)
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
