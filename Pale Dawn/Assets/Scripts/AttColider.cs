using UnityEngine;

public class AttColider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private string layerName;
    private GameObject Host;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision) //needs to get assigned host (aka player) to send back info
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(layerName))
        {
            Host.GetComponent<Player>().attack(collision.gameObject);
        }
    }

    public void setLayerName(string nm) {
        layerName = nm;
    }
}
