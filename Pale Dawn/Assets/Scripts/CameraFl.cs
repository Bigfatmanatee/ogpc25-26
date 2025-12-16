using System.Collections;
using UnityEngine;

public class CameraFl : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] Camera cam;
    [SerializeField] GameObject player;
    [SerializeField] Transform followPos;

    [Header("Camera settings")]
    [SerializeField] float zoom = 6.0f;
    [SerializeField] float xOffset = 0;
    [SerializeField] float yOffset = 0;
    [SerializeField] bool xOffWithCam = true;

    [Header("Follow settings")]
    [SerializeField] float speed;


    bool UseOff = true;
    float direction = 1;

    void Start()
    {

    }

    void Update()
    {
        if (xOffWithCam)
        {
            direction = player.GetComponent<Player>().getDirection();
        }
        
    }
    void FixedUpdate()
    {
        if (UseOff)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(followPos.position.x+(xOffset*direction), followPos.position.y+yOffset, transform.position.z), speed);
        } else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(followPos.position.x, followPos.position.y, transform.position.z), speed);
        }
        
    }
    public void changeTarget(Transform follow)
    {
        followPos = follow;
        UseOff = false;
    }
    public void resetTarget()
    {
        followPos = player.transform;
        UseOff = true;
    }
    public void changeZoom(float mult)
    {
        // cam.orthographicSize = zoom*mult;
        StartCoroutine(smoothZoomOut(zoom,zoom*mult));
    }
    public void resetZoom()
    {
        StartCoroutine(smoothZoomIn(cam.orthographicSize,zoom));
    }
    private IEnumerator smoothZoomOut(float Before, float After)
    {
        for (float i = Before; i < After; i += (After-Before)/5)
        {
            cam.orthographicSize = i;
            yield return new WaitForFixedUpdate();
        }
        cam.orthographicSize = After;
    }
    private IEnumerator smoothZoomIn(float Before, float After)
    {
        for (float i = Before; i > After; i -= (Before-After)/5)
        {
            cam.orthographicSize = i;
            yield return new WaitForFixedUpdate();
        }
        cam.orthographicSize = After;
    }
}
