using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private bool activated = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !activated) //stop retriggers, disabled hitbox?
        {
            gameObject.SendMessage("bossStart");
            activated = true;
        }
    }
}
