using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed = 5;
    [SerializeField] protected int maxHealth = 5;
    [SerializeField] protected int health = 5;
    [SerializeField] protected int damageNum = 1;

    [SerializeField] protected float attTime;
    [SerializeField] protected float attCooldown;
    [SerializeField] protected float maxInvSec;
    protected float sinceLastAtt;
    protected GameObject target;
    protected float InvSec = 0;
    protected bool shouldSwing;



    [SerializeField] protected Collider2D attHitBox;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected GameObject sprite;
    [SerializeField] protected Animator anim;
    [SerializeField] protected int direction = 1;
    [SerializeField] private SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected LayerMask LmG;
    void Start()
    {
        health = maxHealth;
        LmG = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        ExtraStart();
    }

    // Update is called once per frame
    void Update()
    {
        move();
        InvSec += Time.deltaTime;
        if (health <= 0)
        {
            die();
        }
        ExtraUpdate();
    }

    protected virtual void attack()
    {
        if (sinceLastAtt >= attCooldown && shouldSwing)
        {
            // Debug.Log("Running swing function");
            StartCoroutine(swing());
            sinceLastAtt = -attTime;
        }
        else
        {
            sinceLastAtt += Time.deltaTime;
        }
    }

    protected IEnumerator pulse()
    {
        spriteRenderer.color = new Color(1, 0, 0, 1);

        for (float i = 0; i <= 80; i++)
        {
            spriteRenderer.color = Vector4.Lerp(spriteRenderer.color, new Vector4(1, 1, 1, 1), i / 80); //smoothly changing the color back in 1 second
            print(spriteRenderer.color.b);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Vector4(1, 1, 1, .1f);
            print("invis");
            yield return new WaitForSeconds(.2f);
            print("visible");
            spriteRenderer.color = new Vector4(1, 1, 1, 1);
            yield return new WaitForSeconds(.1f);
        }
    }

    protected IEnumerator swing()
    {
        //play animation
        anim.SetBool("attacking", true);
        yield return new WaitForSeconds(attTime); //should match animation
        if (shouldSwing && target.GetComponent<Player>() != null)
        {
            // Debug.Log("sending damage to " + target);
            target.GetComponent<Player>().damage(this.gameObject);
        }
        anim.SetBool("attacking", false);
    }


    protected virtual void FixedUpdate()
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
    protected bool checkForWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, LmG);
    }
    protected bool checkForFloor()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, LmG);
    } //make a second function using raycasts to check directly under
    protected virtual bool raycastForFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, LmG);
        return hit;
    }
    public void trigger(bool enter, GameObject gObject)
    {
        if (enter)
        {
            // Debug.Log("Layer " + getTarget() + " detected entering att hitbox");
            shouldSwing = true;
            Debug.Log("ShouldSwing set to true: " + shouldSwing);
            target = gObject.GetComponent<HitboxPass>().passHost();
            // Debug.Log("Target saved as " + target);
        }
        else
        {
            // Debug.Log("Layer " + getTarget() + " detected exiting att hitbox");
            shouldSwing = false;
            Debug.Log("ShouldSwing set to false: " + shouldSwing);
            target = null;
        }

    }
    public void damage(GameObject player)
    {
        // Debug.Log("Damage recived, sent by " + player);
        // Debug.Log("Before damage, Health:" + health);

        if (InvSec >= maxInvSec)
        {
            health -= player.GetComponent<Player>().getDamage();
            Debug.Log("After damage taken, Health:" + health);
            //play Iframe animation
            StartCoroutine(pulse());
            InvSec = 0;
        }
        else
        {
            Debug.Log("didnt take damage, still invincible");
        }


    }
    public float getSpeed()
    {
        return speed;
    }
    public bool getShouldSwing()
    {
        return shouldSwing;
    }
    protected void toggleDirection()
    {
        direction *= -1;
    }
    protected void setDirection(int a) //should be 1 or -1
    {
        direction = a;
    }
    protected void addDirection()
    {
        direction++;
    }
    protected void addDirection(int a)
    {
        direction += a;   
    }
    public int getDirection()
    {
        return direction;
    }
    public string getTarget()
    {
        return "Player";
    }
    public int getDamage()
    {
        return damageNum;
    }
    public void die()
    {
        Destroy(gameObject);
    }
    protected virtual void move()
    {
        throw new NotImplementedException();
    }
    protected virtual void ExtraStart()
    {
        // Debug.Log("no extra start commands");
    }
    protected virtual void ExtraUpdate()
    {
        // Debug.Log("no extra update commands");
    }
}
