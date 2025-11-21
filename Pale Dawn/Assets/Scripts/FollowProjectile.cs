using Unity.VisualScripting;
using UnityEngine;

public class FollowProjectile : Enemy
{
    protected override void move()
    {
        
    }
    protected override void FixedUpdate()
    {
        Vector2 facing = new Vector2(transform.eulerAngles.x,transform.eulerAngles.y);
        rb.linearVelocity = facing*speed;
    }
}
