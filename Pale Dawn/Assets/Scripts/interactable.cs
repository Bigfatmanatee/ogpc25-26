using System;
using UnityEngine;
using UnityEngine.Events;

public class interactable : MonoBehaviour
{

    protected Collider2D AreaTrigger;
    protected GameObject target;
    protected float yMove;


    void Start()
    {
        AreaTrigger = GetComponent<Collider2D>();
    }
    public virtual void Awake()
    {
        
    }

    protected virtual void Update()
    {
        if (target != null)
        {
            //conditional for interacting
            if (Math.Abs(Player.Instance.interact) > Player.Instance.deadzone)
            {
                yMove = Player.Instance.interact;
            } 
            else
            {
                yMove = 0;
            }
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
           target = collision.gameObject;
           Debug.Log(target);
        }
        
    }
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        target = null;
    }
}
