using Unity.VisualScripting;
using UnityEngine;

public class AttColider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private string layerName;
    [SerializeField] private GameObject Host;
    [SerializeField] private bool hostIsPlayer = false;
    void Start()
    {
        if (!hostIsPlayer)
        {
            layerName = Host.GetComponent<LREnemy>().getTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision) //needs to get assigned host to send back info
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(layerName))
        {

            if (hostIsPlayer)
            {
                Host.GetComponent<Player>().attack(collision.gameObject);
            }
            else
            {
                Host.GetComponent<LREnemy>().trigger(true, collision.gameObject);
            }

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
            if (!hostIsPlayer)
            {
                Host.GetComponent<LREnemy>().trigger(false, collision.gameObject);
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
