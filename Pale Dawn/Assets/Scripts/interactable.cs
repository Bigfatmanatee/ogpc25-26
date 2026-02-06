using System;
using UnityEngine;
using UnityEngine.Events;

public class interactable : MonoBehaviour
{
    //a way to read input, maybe global call when holding up/down or player function to search for nearby interactables
    public static UnityEvent m_Interact;
    private Collider2D AreaTrigger;
    private GameObject target;




    void Start()
    {
        AreaTrigger = GetComponent<Collider2D>();
        if (m_Interact == null)
            m_Interact = new UnityEvent();

        m_Interact.AddListener(OnEventTriggered);
    }

    protected virtual void Update() {}

    protected void OnEventTriggered()
    {
        if (target != null)
        {
            interact();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        target = collision.gameObject;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        target = null;
    }

    protected virtual void interact()
    {
        throw new NotImplementedException();
    }

}
