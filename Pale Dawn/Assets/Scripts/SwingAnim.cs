using UnityEngine;

public class SwingAnim : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setPos(Transform pos, float degrees, float Xoffset, float Yoffset)
    {
        transform.position = (Vector2) pos.position+new Vector2(Xoffset,Yoffset);
        transform.rotation = Quaternion.Euler(Vector3.forward * degrees);
        if (degrees > 90 || degrees < -90)
        {
            transform.Rotate(new Vector3(0,180,180));
        } else
        {
            transform.Rotate(new Vector3(0,0,0));
        }
    }
}
