using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection.Emit;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }
    public float interact;
    private Vector2 m_PlayerMovement;
    private InputAction m_MoveAction;
    private InputAction m_AttackAction;
    private InputAction m_JumpAction;
    private InputAction m_DashAction;
    private float sinceLastSwing = 0;
    private Rigidbody2D rb;
    // private bool isOnLadder = false;
    private LayerMask LmG; //Ground layer mask
    private LayerMask LmC; //enemy collision layer mask
    private LayerMask LmE; //Enemy hitbox layer mask
    private LayerMask LmA; //Attack layer mask
    [Header("Hitboxes")]

    [SerializeField] private Collider2D hitBox;
    [SerializeField] private Collider2D attHitBox;
    [SerializeField] private Collider2D attHitBoxU;
    [SerializeField] private Collider2D attHitBoxD;
    [SerializeField] private Transform wjHitBox; // walljump hitbox
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem wjParticles;
    [SerializeField] private StopTime timeManager;


    [Header("Player Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private float swingTime;
    [SerializeField] private float swingCooldown;
    [SerializeField] private float DsBoost;
    [SerializeField] private float maxInvSec;
    [SerializeField] private float dashCooldown;
    private float dashTime = 0;
    private bool groundTouch = true;
    private float InvSec = 0;
    private bool isSwinging = false;


    [Header("Other Refrences")]
    [SerializeField] private GameObject HealthManager;

    [SerializeField] private Animator swingAnimator;
    [SerializeField] private GameObject swingManager;
    private float swingOffset = 0.75f; //swing animation distance from the center of the player

    [Header("Control settings")]
    [SerializeField] public float deadzone = 0.4f; //deadzone % (between 0.0 - 1.0) 



    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        LmG = LayerMask.GetMask("Ground");
        LmC = LayerMask.GetMask("EnemyCol");
        LmE = LayerMask.GetMask("EnemyHit");
        LmA = LayerMask.GetMask("Attack");

        attHitBox.enabled = false;
        attHitBoxU.enabled = false;
        attHitBoxD.enabled = false;
        // attHitBox.GetComponent<AttColider>().isPlayer();
        attHitBox.GetComponent<AttColider>().setLayerName("EnemyHit");
        attHitBoxU.GetComponent<AttColider>().setLayerName("EnemyHit");
        attHitBoxD.GetComponent<AttColider>().setLayerName("EnemyHit");


        m_MoveAction = new InputAction("Move");
        m_MoveAction.AddBinding("<Gamepad>/leftStick");
        m_MoveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        m_MoveAction.Enable();

        m_AttackAction = new InputAction("Attack");
        m_AttackAction.AddBinding("<Gamepad>/buttonWest");
        m_AttackAction.AddBinding("<Keyboard>/j");
        m_AttackAction.Enable();

        m_JumpAction = new InputAction("Jump");
        m_JumpAction.AddBinding("<Gamepad>/buttonSouth");
        m_JumpAction.AddBinding("<Keyboard>/k");
        m_JumpAction.Enable();

        m_DashAction = new InputAction("Dash");
        m_DashAction.AddBinding("<Gamepad>/buttonEast");
        m_DashAction.AddBinding("<Keyboard>/leftShift");
        m_DashAction.Enable();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // private void Enable()
    // {
    //     m_MoveAction.Enable();
    //     m_AttackAction.Enable();
    //     m_JumpAction.Enable();
    // }

    // private void Disable()
    // {
    //     m_MoveAction.Disable();
    //     m_AttackAction.Disable();
    //     m_JumpAction.Disable();
    // }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, LmG) || Physics2D.OverlapCircle(groundCheck.position, 0.2f, LmC);
    }

    private void Update()
    {
        m_PlayerMovement = m_MoveAction.ReadValue<Vector2>();
        interact = m_PlayerMovement.y;

        if(m_PlayerMovement.x != 0) //was m_PlayerMovement != Vector2.zero
        {
            animator.SetFloat("Speed", 1f); //Walking
        }
        else
        {
            animator.SetFloat("Speed", 0f); //Idle
        }

        // draw directional swing guide
        float dist = 1.8f;
        Vector3 dir = new Vector3(dist,0,0);
        if (transform.eulerAngles.y != 0)
        {
            dir = new Vector3(-dist, 0, 0);
        }
        if (m_PlayerMovement.y > deadzone)
        {
            dir = new Vector3(0, dist, 0);
        }
        else if (m_PlayerMovement.y < -deadzone)
        {
            dir = new Vector3(0, -dist, 0);
        }
        Color c = Color.aliceBlue;
        if (isSwinging)
        {
            c = Color.darkRed;
        }
        Debug.DrawLine(transform.position, transform.position + dir, c, 0.005f);

        // swing code
        if (m_AttackAction.WasPressedThisFrame() && sinceLastSwing >= swingCooldown)
        {
            StartCoroutine(swing());
            sinceLastSwing = -swingTime;
        }
        else
        {
            sinceLastSwing += Time.deltaTime;
        }



        // jumping code
        bool jumping = false;
        var jumpRead = m_JumpAction.ReadValue<float>();
        if (Mathf.Approximately(jumpRead, 1f))
            jumping = true;

        if (m_JumpAction.WasPressedThisFrame()) 
        {
            if (isGrounded()) //normal jumping
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            else if (wjHitBox.GetComponent<WJCollider>().collidingWithWall) //walljump
            {
                // print("walljump");
                rb.linearVelocity = new Vector2(-transform.right.x * 10f, jumpPower);
                wjParticles.Emit(15);
            }
        }
        if (m_JumpAction.WasReleasedThisFrame() && rb.linearVelocity.y > 0f) //slow down when stop holding space
        {
            rb.linearVelocityY = rb.linearVelocity.y * 0.25f; //was new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.25f);
        }
        if (!isGrounded() && !jumping) //falling without holding space
        {
            rb.gravityScale = 2f; //was 1.75
        }
        else if (isGrounded() && !jumping)
        {
            rb.linearVelocityY *= 0.95f;
        }
        else
        {
            rb.gravityScale = 1.25f; //was 1f
        }
        

    //dash code

        if (m_DashAction.WasPressedThisFrame() && dashTime >= dashCooldown)
        {
            float xTest = 0.5f;
            if (m_PlayerMovement.x >= 0.3)
            {
                xTest = m_PlayerMovement.x;
            }
            Debug.Log("X from "+getVelX()+" to "+((Math.Abs(getVelX())+20)*m_PlayerMovement.x));
            Debug.Log("Y from "+getVelY()+" to "+(17*m_PlayerMovement.y*xTest));
            setVel(
                new Vector2(
                    (Math.Abs(getVelX())+20)*m_PlayerMovement.x,
                    17*m_PlayerMovement.y*xTest //straight and diagonal dash feel fine, fix upward dash. was 11
                )
            );
            dashTime = 0;
            groundTouch = false;
        }


        InvSec += Time.deltaTime;
        if (isGrounded())
        {
            groundTouch = true;
        }
        if (groundTouch)
        {
            dashTime += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        
        rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocityX, m_PlayerMovement.x * speed, 8 * Time.deltaTime), rb.linearVelocity.y);
        
        if (m_PlayerMovement.x > 0) //Facing direction, was rb.linearVelocityX
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Normal
        }
        else if (m_PlayerMovement.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
        }
    }

    private IEnumerator swing()
    {
        print("swing animation playing");
        swingAnimator.ResetTrigger("Swing");
        swingAnimator.SetTrigger("Swing");

        //playing the particles
        swingAnimator.transform.GetChild(0).GetComponent<ParticleSystem>().Emit(55);

        isSwinging = true;
        if (m_PlayerMovement.y > deadzone)
        {
            attHitBoxU.enabled = true;
            swingManager.GetComponent<SwingAnim>().setPos(attHitBoxU.transform,90,0,swingOffset);
        }
        else if (m_PlayerMovement.y < -deadzone)
        {
            attHitBoxD.enabled = true;
            swingManager.GetComponent<SwingAnim>().setPos(attHitBoxD.transform,-90,0,-swingOffset);
        }
        else
        {
            attHitBox.enabled = true;
            if (transform.eulerAngles.y != 0)
            {
                swingManager.GetComponent<SwingAnim>().setPos(attHitBox.transform,180,-swingOffset,0); //flip instead of rotate?
            } 
            else
            {
                swingManager.GetComponent<SwingAnim>().setPos(attHitBox.transform,0,swingOffset,0);
            }
            
            
        }
        yield return new WaitForSeconds(swingTime);
        attHitBox.enabled = false;
        attHitBoxU.enabled = false;
        attHitBoxD.enabled = false;
        isSwinging = false;
    }
    public void attack(GameObject enemy)
    {
        if (enemy.layer == 15)
        {
            StartCoroutine(onHit());  
            return;
        }
            
        if (enemy.GetComponent<BossScript>() != null)
        {
            enemy.GetComponent<BossScript>().damage(gameObject);
        } 
        else if (enemy.GetComponent<Projectile>() != null)
        {
            enemy.GetComponent<Projectile>().damage(gameObject);
        }
        else
        {
            var Host = enemy.GetComponent<HitboxPass>().passHost();
            if (Host.GetComponent<Enemy>() != null)
            {
                Host.GetComponent<Enemy>().damage(gameObject);
            } 
            else if (Host.GetComponent<Projectile>() != null)
            {
                Host.GetComponent<Projectile>().damage(gameObject);
            }
            
            StartCoroutine(onHit());  
        }
        
    }

    // // Walljump stuff
    // public void walljump()
    // {
    //     if (wjHitBox.GetComponent<WJCollider>().collidingWithWall && !isGrounded())
    //     {
    //         rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower*0.5f);
    //     }
    // }

    public void damage(GameObject enemy)
    {
        // Debug.Log("Damage recived, sent by " + enemy);
        // Debug.Log("Before damage, Health:" + health);

        if (InvSec >= maxInvSec)
        {
            if (health-1 > 0)
            {
                HealthManager.GetComponent<HealthManager>().damage();
            } 
            else
            {
                die();
            }
            health -= 1;
            Debug.Log("After damage taken, Health:" + health);
            InvSec = 0;
        }
        else
        {
            Debug.Log("didnt take damage, still invincible");
        }
    }
    public IEnumerator onHit()
    {
        //returned if hit an enemy, for downward slash boost 
        // Other ideas:
        // soul/mana meter
        // lifesteal
        if (attHitBoxD.enabled == true)
        {
            //boost logic
            if (rb.linearVelocityY <= 0)
            {
                rb.linearVelocityY = 0;
            }
            rb.linearVelocityY += DsBoost;
            yield return new WaitForSeconds(0.15f);
        }
    }
    private void die()
    {
        throw new NotImplementedException();
    }

    public int getDamage() {
        return 1;
    }
    public int getDirection()
    {
        if (transform.eulerAngles.y != 0)
        {
            return -1;
        }
        return 1;
    }
    public int getMaxHealth()
    {
        return maxHealth;
    }
    public int getCurHealth()
    {
        return health;
    }
    public Vector2 getPos()
    {
        return rb.transform.position;
    }
    public float getPosX()
    {
        return rb.transform.position.x;
    }
    public float getPosY()
    {
        return rb.transform.position.y;
    }
    public float getVelX()
    {
        return rb.linearVelocityX;
    }
    public float getVelY()
    {
        return rb.linearVelocityY;
    }
    public void setVel(Vector2 vel)
    {
        rb.linearVelocity = vel;
    }
    public void setYVel(int vel)
    {
        rb.linearVelocityY = vel;
    }
    public void setXVel(int vel)
    {
        rb.linearVelocityX = vel;
    }
    public void setPos(Vector2 pos)
    {
        rb.transform.position = pos;
    }
    public void setPosX(float x)
    {
        rb.transform.position = new Vector2(x,rb.transform.position.y);
    }
    public void setPosY(float y)
    {
        rb.transform.position = new Vector2(rb.transform.position.x, y);
    }
    public void offsetPos(Vector2 pos)
    {
        rb.transform.position = new Vector2(rb.transform.position.x + pos.x, rb.transform.position.y + pos.y);
    }
    
}


