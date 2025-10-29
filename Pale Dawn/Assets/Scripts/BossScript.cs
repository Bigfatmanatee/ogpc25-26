using System;
using System.Collections;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float ylevel;
    [SerializeField] GameObject[] IdleFollow;
    int curNode = 0;
    void Update()
    {
        float height = Mathf.Cos(Time.timeSinceLevelLoad) + ylevel;

        transform.position = Vector2.Lerp(new Vector2(transform.position.x, height), new Vector2(IdleFollow[curNode].transform.position.x, height), speed); //* Mathf.Cos(Time.deltaTime) have a speed up and slow down movement
        
        float dist = Vector2.Distance(new Vector2(transform.position.x, height), new Vector2(IdleFollow[curNode].transform.position.x, height));

        Debug.Log("distance:" + dist);
        if (dist < 4f)
        {
            curNode++;
            if (curNode >= IdleFollow.Length)
            {
                curNode = 0;
            }
        }
    }


    
}
