using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlatformEnemy : Enemy
{
    // changes direction to be 1-4, in order right, down, left, up
    //maybe change to be a follow a track design?
    float pausetime = 0;
    float maxPausetime = 0.3f;
    [SerializeField] Transform rotatePos;
    protected override void ExtraStart()
    {
        rb.gravityScale = 0;
    }
    protected override void ExtraUpdate()
    {
        pausetime += Time.deltaTime;
    }
    protected Vector2 DirToVector(int a) //down from facing direction
    {
        if (a == 1)
        {
            return new Vector2(0, -1);
        }
        else if (a == 2)
        {
            return new Vector2(-1, 0);
        }
        else if (a == 3)
        {
            return new Vector2(0, 1);
        }
        else if (a == 4)
        {
            return new Vector2(1, 0);
        } else
        {
            return new Vector2(0, 0);
        }
    }
    protected override bool raycastForFloor()
    {
        Debug.DrawLine(groundCheck.position, groundCheck.position + new Vector3(DirToVector(getDirection()).x/2, DirToVector(getDirection()).y/2),Color.yellow,0.005f,false);
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, DirToVector(getDirection()), 0.5f, LmG);
        return hit;
    }
    protected override void move()
    {
        if (!raycastForFloor() && pausetime > maxPausetime)
        {
            addDirection();
            transform.RotateAround(rotatePos.transform.position, Vector3.forward, -90);
            if (getDirection() > 4)
            {
                setDirection(1);
            }
            pausetime = 0;
        }
    }
    protected override void FixedUpdate()
    {
        if (!getShouldSwing())
        {
            // Debug.Log("platform enemy direction:"+getDirection());
            if (getDirection() == 1)
            {
                rb.linearVelocity = new Vector2(speed, 0);
            }
            else if (getDirection() == 2)
            {
                rb.linearVelocity = new Vector2(0, speed*-1);
            }
            else if (getDirection() == 3)
            {
                rb.linearVelocity = new Vector2(speed*-1, 0);
            }
            else if (getDirection() == 4)
            {
                rb.linearVelocity = new Vector2(0, speed);
            }
            
        }
        else
        {
            rb.linearVelocity = new Vector2(0, 0);
        }

        
        // if (getDirection() == 1)
        // {
        //     transform.eulerAngles = new Vector3(0, 0, 0);
        // }
        // else if (getDirection() == 2)
        // {
        //     transform.eulerAngles = new Vector3(0, 0, 270);
        // }
        // else if (getDirection() == 3)
        // {
        //     transform.eulerAngles = new Vector3(0, 0, 180);
        // }
        // else if (getDirection() == 4)
        // {
        //     transform.eulerAngles = new Vector3(0, 0, 90);
        // }
    }
}
