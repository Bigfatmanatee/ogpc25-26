using System;
using UnityEngine;

public class WJCollider : MonoBehaviour
{
    public bool collidingWithWall = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            collidingWithWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            collidingWithWall = false;
        }
    }
}
