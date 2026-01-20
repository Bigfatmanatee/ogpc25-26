using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 5;
    [SerializeField] protected int damageNum = 1;

    protected bool reflected = false;
    protected GameObject target;
    protected GameObject sender;



    [SerializeField] protected Collider2D attHitBox;
    [SerializeField] protected Transform colCheck;
    [SerializeField] protected GameObject sprite;
    [SerializeField] protected Animator anim;
    [SerializeField] protected int direction = 1;
    [SerializeField] private SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected LayerMask LmG;
    protected Vector2 pos;
    protected Vector2 startPos;
    void Start()
    {
        LmG = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        ExtraStart();
    }

    // Update is called once per frame
    void Update()
    {
        ExtraUpdate();
    }

    public virtual void setTarget(GameObject Tar)
    {
        target = Tar;
        pos = target.transform.position;
        startPos = transform.position;
    }
    public virtual void setTarget(GameObject Tar, GameObject Send)
    {
        target = Tar;
        pos = target.transform.position;
        sender = Send;
    }


    protected virtual void FixedUpdate()
    {
        rb.linearVelocity = ((Vector2) transform.position - pos); //normalize vector and add speed into equation
        if (Vector2.Distance(transform.position, pos) <= 0.1f)
        {
            die();
        }
    }

    protected bool checkForWall()
    {
        return Physics2D.OverlapCircle(colCheck.position, 0.2f, LmG);
    }

    public virtual void trigger(bool enter, GameObject gObject) //re-write
    {
        if (enter)
        {
            target = gObject.GetComponent<HitboxPass>().passHost();
        }
    }
    public virtual void damage(GameObject player)
    {
        if (!reflected && sender != null)
        {
            reflected = true;
            pos = sender.transform.position;
        } 
        else if (!reflected)
        {
            reflected = true;
            pos = startPos;
        }
        
    }





    public float getSpeed()
    {
        return speed;
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
    protected virtual void ExtraStart()
    {
        // Debug.Log("no extra start commands");
    }
    protected virtual void ExtraUpdate()
    {
        // Debug.Log("no extra update commands");
    }
}
