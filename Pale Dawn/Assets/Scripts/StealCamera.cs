using UnityEditor;
using UnityEngine;

public class StealCamera : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    [SerializeField] Transform Position;

    void Start(){}


    void Update(){}
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


    public void bossStart()
    {
        takeCam();
    }
    public void bossKilled()
    {
        returnCam();
    }
}
