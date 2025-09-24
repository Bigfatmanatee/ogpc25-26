using System;
using System.Collections;
using UnityEngine;

public class LREnemy : MonoBehaviour
{
    // [SerializeField] private int movementPattern = 1; //1=left & right
    [SerializeField] private float speed = 5;
    [SerializeField] private int maxHealth = 5;

    [SerializeField] private float attTime;
    [SerializeField] private float attCooldown;
    [SerializeField] private float maxIframes;
    private float sinceLastAtt;
    private GameObject target;
    private float Iframes;
    private bool shouldSwing;



    [SerializeField] private Collider2D attHitBox;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;
    private int health;
    private int direction = 1;
    private Rigidbody2D rb;
    private LayerMask LmG;
    void Start()
    {
        LmG = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkForWall() || !checkForFloor())
        {
            direction *= -1;
        }
        else
        {
            attack();
        }

    }

    private void attack()
    {
        if (sinceLastAtt >= attCooldown && shouldSwing)
        {
            StartCoroutine(swing());
            sinceLastAtt = -attTime;
        }
        else
        {
            sinceLastAtt += Time.deltaTime;
        }
    }

    private IEnumerator swing()
    {
        //play animation
        yield return new WaitForSeconds(attTime); //should match animation
        if (shouldSwing && target.GetComponent<Player>() != null)
        {
            target.GetComponent<Player>().damage(this); //idk how to do this again
        }
    }


    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
        if (rb.linearVelocityX > 0) //Facing direction
        {
            // facingL = true;
            transform.eulerAngles = new Vector3(0, 0, 0); // Normal
        }
        else if (rb.linearVelocityX < 0)
        {
            // facingL = false;
            transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
        }
    }
    private bool checkForWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, LmG);
    }
    private bool checkForFloor()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, LmG);
    }
    public void trigger(bool enter, GameObject gObject)
    {
        if (enter)
        {
            shouldSwing = true;
            target = gObject;
        }
        else
        {
            shouldSwing = false;
            target = null;
        }

    }
}
