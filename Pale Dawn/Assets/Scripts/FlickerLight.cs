using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLight : MonoBehaviour
{
    Light2D light2D;
    public float leastTime;
    public float maxTime;
    public float currentTime;
    private float currentIntensity;
    public float flickerIntensity = 0;
    private bool onOff = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = Random.Range(leastTime, maxTime);
        light2D = transform.GetComponent<Light2D>();
        currentIntensity = light2D.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0 && onOff == true)
        {
            onOff = false;
            currentTime = Random.Range(leastTime, maxTime);
            light2D.intensity = currentIntensity;
        }
        else if (currentTime <= 0 && onOff == false)
        {
            onOff = true;
            currentTime = Random.Range(leastTime, maxTime);
            light2D.intensity = Random.Range(currentIntensity * flickerIntensity, currentIntensity);
        }

        
    }
}
