using System.Collections;
using UnityEngine;

public class StopTime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Pause(float duration)
    {
        StartCoroutine(ExecutePause(duration));
    }

    private IEnumerator ExecutePause(float duration)
    {
        //Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration); 

        Time.timeScale = 1f;
    }
}
