using UnityEditor;
using UnityEngine;

public class StealCamera : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    [SerializeField] Transform Position;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeCam()
    {
        Camera.GetComponent<CameraFl>().changeTarget(Position);
        Camera.GetComponent<CameraFl>().changeZoom(1.25f);
    }
    public void returnCam()
    {
        Camera.GetComponent<CameraFl>().resetTarget();
        Camera.GetComponent<CameraFl>().resetZoom();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) //stop retriggers, disabled hitbox?
        {
           takeCam();
           //toggle maps to the closed arena and enable boss

           //BossArena.enabled = true;
           //UnlockedArena.enabled = false;
        }
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
        //    returnCam(); //only return cam when boss is dead
        }
    }
}
