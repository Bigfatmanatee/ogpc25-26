using Unity.Mathematics;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformEnemy : Enemy
{
    // changes direction to be 1-4, in order right, down, left, up
    //maybe change to be a follow a track design?
    // float pausetime = 0;
    // float maxPausetime = 0.3f;
    bool paused;
    float resetTimer = 0;
    float maxResetTime = 1;
    Vector2 start;
    [SerializeField] int facing = -1;
    [SerializeField] Transform rotatePos;
    protected override void ExtraStart()
    {
        rb.gravityScale = 0;
        if (facing > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        start = gameObject.transform.position;
    }
    protected override void ExtraUpdate()
    {
        // pausetime += Time.deltaTime;
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
        if (checkForWall())
        {
            //add code for 90 degree inwards angles
        }
        if (!raycastForFloor() && !paused)
        {
            if (facing > 0)
            {
                addDirection(-1);
            }
            else if (facing < 0)
            {
                addDirection(1);
            }

            transform.RotateAround(rotatePos.transform.position, Vector3.forward, 90 * facing);
            
            if (getDirection() > 4)
            {
                setDirection(1);
            }
            else if (getDirection() < 1)
            {
                setDirection(4);
            }
            paused = true;
        }
        else if (raycastForFloor() && paused)
        {
            paused = false;
        }
        else if (!raycastForFloor() && paused)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer >= maxResetTime)
            {
                setDirection(1);
                gameObject.transform.rotation = new quaternion(0,0,0,0);
                gameObject.transform.position = start;
                resetTimer = 0;
                paused = false;
                Debug.Log("Direction: " + getDirection());
            }
        }
    }
    protected override void FixedUpdate()
    {
        if (!getShouldSwing())
        {
            // Debug.Log("platform enemy direction:"+getDirection());
            if (getDirection() == 1)
            {
                rb.linearVelocity = new Vector2(speed*facing*-1, 0);
            }
            else if (getDirection() == 2)
            {
                rb.linearVelocity = new Vector2(0, speed*facing);
            }
            else if (getDirection() == 3)
            {
                rb.linearVelocity = new Vector2(speed*facing, 0);
            }
            else if (getDirection() == 4)
            {
                rb.linearVelocity = new Vector2(0, speed*-1*facing);
            }
            
        }
        else
        {
            rb.linearVelocity = new Vector2(0, 0);
            attack();
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
