using UnityEngine;

public class FollowProjectile : Enemy
{
    private Vector2 facing;
    protected override void ExtraStart()
    {
        facing = Vector2.zero;
    }
    protected override void move()
    {
        
    }
    protected override void FixedUpdate()
    {
        if (target != null)
        {
            // rb.transform.LookAt(target.transform);
            facing = Vector2.MoveTowards(Vector2.zero,target.transform.position,1);
        }
        rb.linearVelocity = facing*speed;
    }
}
