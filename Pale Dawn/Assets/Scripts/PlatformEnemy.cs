using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlatformEnemy : Enemy
{
    // changes direction to be 1-4, in order right, down, left, up
    //maybe change to be a follow a track design?
    float pausetime = 0;
    float maxPausetime = 0.25f;
    protected override void ExtraStart()
    {
        rb.gravityScale = 0;
    }
    protected override void ExtraUpdate()
    {
        pausetime += Time.deltaTime;
    }
    protected override void move()
    {
        if (!base.checkForFloor() && pausetime > maxPausetime)
        {
            base.addDirection();
            if (base.getDirection() > 4)
            {
                base.setDirection(1);
            }
            pausetime = 0;
        }
    }
    protected override void FixedUpdate()
    {
        if (!base.getShouldSwing())
        {
            if (base.getDirection() == 1)
            {
                rb.linearVelocity = new Vector2(speed, 0);
            }
            else if (base.getDirection() == 2)
            {
                rb.linearVelocity = new Vector2(0, speed*-1);
            }
            else if (base.getDirection() == 3)
            {
                rb.linearVelocity = new Vector2(speed*-1, 0);
            }
            else if (base.getDirection() == 4)
            {
                rb.linearVelocity = new Vector2(0, speed);
            }
            
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        
        if (base.getDirection() == 1)
        {
             transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (base.getDirection() == 2)
        {
                transform.eulerAngles = new Vector3(0, 0, 270);
        }
        else if (base.getDirection() == 3)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (base.getDirection() == 4)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }
}
