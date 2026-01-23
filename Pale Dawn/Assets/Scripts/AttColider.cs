using UnityEngine;

public class AttColider : MonoBehaviour
{
    // Split into enemy and projectile files?
    private string layerName;
    [SerializeField] private GameObject Host;
    [SerializeField] private bool hostIsPlayer = false;
    [SerializeField] private bool overrideTrigger = false;
    void Start()
    {
        if (!hostIsPlayer)
        {
            if (Host.GetComponent<Enemy>() != null)
            {
                layerName = Host.GetComponent<Enemy>().getTarget();
            } 
            else if (Host.GetComponent<Projectile>() != null)
            {
                layerName = Host.GetComponent<Projectile>().getTarget();
            } 
            else
            {
                layerName = "Player";
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) //needs to get assigned host to send back info
    {
        // Debug.Log("Entered collision of layer "+collision.gameObject.layer);
        if (collision.gameObject.layer == LayerMask.NameToLayer(layerName))
        {

            if (hostIsPlayer)
            {
                Host.GetComponent<Player>().attack(collision.gameObject);
            }
            else if (!overrideTrigger)
            {
                // Debug.Log("Running trigger");
                Host.GetComponent<Enemy>().trigger(true, collision.gameObject);
            } 
            else
            {
                if (Host.GetComponent<Projectile>() != null)
                {
                    Host.GetComponent<Projectile>().trigger(collision.gameObject);
                }
            }

        }
        else if (hostIsPlayer && collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            Host.GetComponent<Player>().attack(collision.gameObject);
        }
        // else if (hostIsPlayer && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        // {
        //     Host.GetComponent<Player>().spark();
        // }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(layerName))
        {
            if (!hostIsPlayer && Host.GetComponent<Enemy>() != null)
            {
                Host.GetComponent<Enemy>().trigger(false, collision.gameObject);
            }
        }
    }

    public void setLayerName(string nm)
    {
        layerName = nm;
    }
    public void isPlayer()
    {
        hostIsPlayer = true;
    }
}
