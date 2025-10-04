using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] Transform followPos;
    [SerializeField] float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(followPos.position.x,followPos.position.y,transform.position.z), speed);
    }
}
