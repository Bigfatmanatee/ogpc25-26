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

    [Header("Other stats")]
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] private float maxInvSec;
    private float InvSec = 0;

    [Header("Time between moves")]
    [SerializeField] float moveTimeBase;
    [SerializeField] float moveTimeVarience;

    [Header("Position Nodes")]
    [SerializeField] Transform floorCheckPos;
    [SerializeField] GameObject[] IdleFollow;
    [SerializeField] Transform[] ability1Nodes;
    [SerializeField] Transform ability2Node;

    [Header("Prefabs")]
    [SerializeField] GameObject A1Projectile;
    [SerializeField] GameObject A2Projectile;
    [Header("Player Info")]
    [SerializeField] GameObject player;
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
        health = maxHealth;
    }
    void FixedUpdate()
    {
        height = Mathf.Cos(hTime * 2) * 2 + ylevel;
        dist = Vector2.Distance(new Vector2(transform.position.x, height), new Vector2(IdleFollow[curNode].transform.position.x, height));

        if (isIdle)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, height), new Vector2(IdleFollow[curNode].transform.position.x, height), speed * temp);

            if (dist < 2f)
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
        InvSec += Time.deltaTime;


        if (moveTimer <= 0)
        {
            if (nextMove != null)
            {
                isIdle = false;
                StartCoroutine(nextMove);
            }
            moveTimer = moveTimeBase + UnityEngine.Random.Range(-moveTimeVarience, moveTimeVarience);
            // nextMove = Abilities[UnityEngine.Random.Range(0, Abilities.Count())];
            // nextMove = Abilities[UnityEngine.Random.Range(0, 2)];
            nextMove = "Ability2"; //test specific move
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
        Vector2 startPos = transform.position; //save start position for returning later
        StartCoroutine(MoveToNode(ability1Nodes[0].position,0.2f)); //move to slam position
        yield return new WaitUntil(() => !goingToNode);
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(MoveToNode(ability1Nodes[1].position, 0.5f)); //move slightly up
        yield return new WaitUntil(() => !goingToNode);
        yield return new WaitForSeconds(0.5f); //cooldown before slam

        RaycastHit2D hit = Physics2D.Raycast(floorCheckPos.position, new Vector2(0, -20), 40, LmG); //find floor height
        Debug.Log("Hit position:" + hit.point);
        Debug.DrawLine(transform.position, hit.point, Color.azure, 1.5f);
        float yDist = Vector2.Distance(floorCheckPos.position, hit.point);
        StartCoroutine(MoveToNode(hit.point+new Vector2(0,2), yDist/5)); //move to slam
        yield return new WaitUntil(() => !goingToNode);

        //sliding floor projectiles
        Instantiate(A1Projectile, ability1Nodes[2].position, ability1Nodes[2].rotation).GetComponent<LRProj>().setDirection(-1);
        Instantiate(A1Projectile, ability1Nodes[3].position, ability1Nodes[3].rotation).GetComponent<LRProj>().setDirection(1);
        yield return new WaitForSeconds(1.75f);// time to dodge and attack


        StartCoroutine(MoveToNode(startPos, 0.3f));
        yield return new WaitUntil(() => !goingToNode);
        isIdle = true;
    }
    private IEnumerator Ability2() //shoot projectiles
    {
        Instantiate(A2Projectile, ability2Node.position, ability2Node.rotation).GetComponent<Projectile>().setTarget(player, gameObject);
        yield return new WaitForSeconds(2f);//stall for long enough to have reflect hit
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
        for (int i = 0; distPos > 0.5f; i++)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPos, speed);
            distPos = Vector2.Distance(transform.position, endPos);
            yield return new WaitForFixedUpdate();
        }
        goingToNode = false;
    }

    public void damage(GameObject player)
    {
        // Debug.Log("Damage recived, sent by " + player);
        // Debug.Log("Before damage, Health:" + health);

        if (InvSec >= maxInvSec)
        {
            health -= player.GetComponent<Player>().getDamage();
            Debug.Log("After damage taken, Health:" + health);
            InvSec = 0;
        }
        else
        {
            Debug.Log("didnt take damage, still invincible");
        }
    }

    public bool dead()
    {
        if (health <= 0)
        {
            return true;
        } 
        else
        {
            return false; 
        }
    }
    
    public int getHealth()
    {
        return health;
    }
    public float getHealthPercent()
    {
        return (float) health/maxHealth;
    }
}
