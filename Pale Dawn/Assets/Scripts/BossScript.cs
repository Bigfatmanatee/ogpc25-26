using System;
using System.Collections;
using UnityEngine;


public class BossScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float ylevel;
    [SerializeField] GameObject[] IdleFollow;
    [SerializeField] float TAthreshhold;
    int curNode = 0;
    float height;
    float dist;
    float temp;
    bool isIdle;
    float moveTimer = 0;
    string[] Abilities = new string[5];
    string nextMove = null;
    [SerializeField] float moveTimeBase;
    [SerializeField] float moveTimeVarience;
    float hTime;
    void Start()
    { //change strings to function names
        Abilities[0] = "Ability1";
        Abilities[1] = "Ability2";
        Abilities[2] = "Ability3";
        Abilities[3] = "Ability4";
        Abilities[4] = "Ability5";
        isIdle = true;
    }
    void FixedUpdate()
    {
        height = Mathf.Cos(hTime * 2) * 2 + ylevel;
        dist = Vector2.Distance(new Vector2(transform.position.x, height), new Vector2(IdleFollow[curNode].transform.position.x, height));

        if (isIdle)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, height), new Vector2(IdleFollow[curNode].transform.position.x, height), speed * temp);

            if (dist < 4f)
            {
                curNode++;
                if (curNode >= IdleFollow.Length)
                {
                    curNode = 0;
                }
                StartCoroutine(turnAround());
            }
        }
    }

    void Update()
    {
        if (moveTimer <= 0)
        {
            if (nextMove != null)
            {
                isIdle = false;
                StartCoroutine(nextMove);
            }
            moveTimer = moveTimeBase + UnityEngine.Random.Range(-moveTimeVarience, moveTimeVarience);
            nextMove = Abilities[UnityEngine.Random.Range(0, 5)];
            Debug.Log("Timer: " + moveTimer + ", next move: " + nextMove);
        }
        else
        {
            if (isIdle)
            {
                moveTimer -= Time.deltaTime;
                hTime += Time.deltaTime;
            }
            
        }
    }

    IEnumerator turnAround()
    {
        temp = -1;
        for (int i = 0; temp < -TAthreshhold; i++)
        {
            temp /= 2;
            // Debug.Log("TA speed: " + temp + " < "+-TAthreshhold);
            yield return new WaitForFixedUpdate();
        }
        temp *= -1;
        for (int i = 0; temp < 1; i++)
        {
            temp *= 2;
            // Debug.Log("TA speed: " + temp + " > "+speed);
            yield return new WaitForFixedUpdate();
        }
        temp = 1;
    }

    // ability ideas:
    //  -Spawn enemys
    //  -Fireball/projectile (possible reflect?)
    //  -Some kind of zoning/area denial (maybe leave boss open in non blocked area)
    //  -Slam/ground pound (stuns boss on miss)
    //

    // while boss is idle, player can:
    //  -gather projectile/way to damage boss
    //  -fight enemies (dont make it feel like spam/horde)
    //  -position for next attack (make move timer very low)


    private IEnumerator Ability1()
    {
        Debug.Log("Ability 1");
        yield return new WaitForSeconds(1f);
        isIdle = true;
    }
    private IEnumerator Ability2()
    {
        Debug.Log("Ability 2");
        yield return new WaitForSeconds(1f);
        isIdle = true;
    }
    private IEnumerator Ability3()
    {
        Debug.Log("Ability 3");
        yield return new WaitForSeconds(1f);
        isIdle = true;
    }
    private IEnumerator Ability4()
    {
        Debug.Log("Ability 4");
        yield return new WaitForSeconds(1f);
        isIdle = true;
    }
    private IEnumerator Ability5()
    {
        Debug.Log("Ability 5");
        yield return new WaitForSeconds(1f);
        isIdle = true;
    }
    
}
