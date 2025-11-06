using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public class BossScript : MonoBehaviour
{
    [Header("Movement stats")]
    [SerializeField] float speed;
    [SerializeField] float ylevel;
    [SerializeField] float TAthreshhold;
    int curNode = 0;
    float height;
    float dist;
    float temp;
    bool isIdle;
    float moveTimer = 0;
    bool goingToNode = false;
    string[] Abilities = new string[5];
    string nextMove = null;
    [Header("Time between moves")]
    [SerializeField] float moveTimeBase;
    [SerializeField] float moveTimeVarience;

    [Header("Position Nodes")]
    [SerializeField] Transform floorCheckPos;
    [SerializeField] GameObject[] IdleFollow;
    [SerializeField] Transform[] ability1Nodes;
    float hTime;
    LayerMask LmG;
    void Start()
    {
        LmG = LayerMask.GetMask("Ground");
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
            // nextMove = Abilities[UnityEngine.Random.Range(0, Abilities.Count())];
            nextMove = "Ability1"; //test specific move
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


    private IEnumerator Ability1() //slam
    {
        Debug.Log("Ability 1: slam");
        StartCoroutine(MoveToNode(ability1Nodes[0].position,0.2f));
        yield return new WaitUntil(() => !goingToNode);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MoveToNode(ability1Nodes[1].position, 0.5f));
        yield return new WaitUntil(() => !goingToNode);
        yield return new WaitForSeconds(0.2f);
        // yield return new WaitForSeconds(1f);
        RaycastHit2D hit = Physics2D.Raycast(floorCheckPos.position, new Vector2(0, -20), 40, LmG);
        Debug.Log("Hit position:" + hit.point);
        Debug.DrawLine(transform.position, hit.point, Color.azure, 2f);
        float yDist = Vector2.Distance(floorCheckPos.position, hit.point);
        StartCoroutine(MoveToNode(hit.point, yDist / 5));
        yield return new WaitUntil(() => !goingToNode);
        yield return new WaitForSeconds(1f);
        // big hitbox area (maybe sliding floor projectiles?)
        // return to flight path
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


    private IEnumerator MoveToNode(Vector2 endPos, float speed)
    {
        goingToNode = true;
        float distPos = Vector2.Distance(transform.position,endPos);
        for (int i = 0; distPos > 1f; i++)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPos, speed);
            distPos = Vector2.Distance(transform.position, endPos);
            yield return new WaitForFixedUpdate();
        }
        goingToNode = false;
    }
    
}
