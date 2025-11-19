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
        yield return null;
        if (shouldSwing && target.GetComponent<Player>() != null)
        {
            target.GetComponent<Player>().damage(gameObject);
            die();
        }
    }
    public override void trigger(bool enter, GameObject gObject)
    {
        shouldSwing = true;
        // Debug.Log("ShouldSwing set to true: " + shouldSwing);
        target = gObject.GetComponent<HitboxPass>().passHost();
        // Debug.Log("Target saved as " + target);
    }
    protected override void attack()
    {
        if (shouldSwing)
        {
            // Debug.Log("Running swing function");
            StartCoroutine(swing());
        }

    }
    protected override void FixedUpdate()
    {
        if (!shouldSwing)
        {
            rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
        }
        else
        {
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
