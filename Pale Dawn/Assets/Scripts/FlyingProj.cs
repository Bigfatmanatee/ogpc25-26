using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingProj : Enemy
{
    protected Transform startPos;
    protected Vector2 aim;
    protected double startTime;
    protected override void move()
    {
        if (checkForWall() || checkForFloor())
        {
            // die();
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
    protected override void ExtraStart()
    {
        rb.gravityScale = 0;
        startTime = Time.time;
    }
    protected override void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position,aim,speed);
        if (Vector2.Distance(transform.position,aim) <= 0.2f)
        {
            die();
        }
    }

    public void updateAim(Vector2 a)
    {
        aim = a;
        // Debug.Log("new position: "+aim.x+" "+aim.y);
    }
}
