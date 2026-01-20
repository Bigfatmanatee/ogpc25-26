using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingProjOld : Enemy
{
    protected Transform startPos;
    protected Vector2 aim;
    protected double startTime;
    protected GameObject ReflTarget;
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
        InvSec = maxInvSec;
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
    public void updateAim(Vector2 a, GameObject boss)
    {
        aim = a;
        ReflTarget = boss;
    }

    public override void damage(GameObject player)
    {
        //reflect code
        if (InvSec >= maxInvSec)
        {
            updateAim(ReflTarget.transform.position);
        }
        
    }
}
