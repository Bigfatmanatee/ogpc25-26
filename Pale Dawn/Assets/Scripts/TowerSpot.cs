// using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TowerSpot : MonoBehaviour
{
    public float maxAngle = 10f;   // degrees
    public float speed = 1f;       // oscillations per second
    public int rayCount = 6;
    public float maxRayAngle = 30f;

    void Update()
    {
        float angle = Mathf.Sin(Time.time * speed) * maxAngle + 180;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);

        float increment = 2*maxRayAngle / rayCount;
        for (int i = 0; i < rayCount+1; i++)
        {
            float rayAngle = -maxRayAngle + increment * i;

            Vector2 direction = Quaternion.Euler(0f, 0f, rayAngle) * transform.TransformDirection(Vector3.up);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);

            if (hit)
            {
                Debug.DrawRay(transform.position, direction.normalized * hit.distance, Color.red);
                if (hit.collider.CompareTag("Player"))
                {
                    transform.GetComponent<Light2D>().color = Color.red;
                }
            }

            
        }
    }
}
