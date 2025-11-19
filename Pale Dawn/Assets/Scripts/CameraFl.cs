using UnityEngine;

public class CameraFl : MonoBehaviour
{
    [SerializeField] Transform followPos;
    [SerializeField] float speed;
    [SerializeField] float xOffset = 0;
    [SerializeField] float yOffset = 0;
    [SerializeField] bool xOffWithCam = true;
    [SerializeField] GameObject player;
    float direction = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (xOffWithCam)
        {
            direction = player.GetComponent<Player>().getDirection();
        }
        
    }
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(followPos.position.x+(xOffset*direction), followPos.position.y+yOffset, transform.position.z), speed);
    }
}
