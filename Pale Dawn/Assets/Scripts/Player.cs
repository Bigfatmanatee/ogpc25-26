using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 m_PlayerMovement;
    private InputAction m_MoveAction;
    private InputAction m_AttackAction;
    private InputAction m_JumpAction;
    // private bool facingL;
    private float sinceLastSwing = 0;
    private Rigidbody2D rb;
    private LayerMask LmG; //Ground layer mask
    private LayerMask LmE; //Enemy layer mask
    private LayerMask LmA; //Attack layer mask
    [Header("Hitboxes")]

    [SerializeField] private Collider2D hitBox;
    [SerializeField] private Collider2D attHitBox;
    [SerializeField] private Collider2D attHitBoxU;
    [SerializeField] private Collider2D attHitBoxD;
    [SerializeField] private Transform groundCheck;


    [Header("Player Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private float swingTime;
    [SerializeField] private float swingCooldown;
    [SerializeField] private float DsBoost;
    [SerializeField] private float maxInvSec;
    private float InvSec = 0;
    private bool isSwinging = false;

    [SerializeField] private GameObject[] HealthBar;
    [SerializeField] private Animator swingAnimator;


    [SerializeField] private float deadzone = 0.4f; //deadzone % (between 0.0 - 1.0) 



    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        LmG = LayerMask.GetMask("Ground");
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

        // OnDisable();
    }

    private void Awake()
    {
        // OnEnable();
    }

    // private void OnEnable()
    // {
    //     m_MoveAction.Enable();
    //     m_AttackAction.Enable();
    //     m_JumpAction.Enable();
    // }

    // private void OnDisable()
    // {
    //     m_MoveAction.Disable();
    //     m_AttackAction.Disable();
    //     m_JumpAction.Disable();
    // }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, LmG);
    }

    private void Update()
    {
        m_PlayerMovement = m_MoveAction.ReadValue<Vector2>();
        //if (m_PlayerMovement != Vector2.zero)
        //    Debug.Log("Vector = " + m_PlayerMovement);

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

        var attacking = m_AttackAction.ReadValue<float>(); //temp attack code
        if (m_AttackAction.WasPressedThisFrame() && sinceLastSwing >= swingCooldown)
        {
            StartCoroutine(swing());
            sinceLastSwing = -swingTime;
        }
        else
        {
            sinceLastSwing += Time.deltaTime;
        }




        bool jumping = false;
        var jumpRead = m_JumpAction.ReadValue<float>();
        if (Mathf.Approximately(jumpRead, 1f))
            jumping = true;


        if (m_JumpAction.WasPressedThisFrame() && isGrounded()) //normal jumping
        {
            rb.linearVelocityY = jumpPower; //was new Vector2(rb.linearVelocity.x, jumpPower);
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
            rb.gravityScale = 1f;
        }
        InvSec += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        
        rb.linearVelocity = new Vector2(m_PlayerMovement.x * speed, rb.linearVelocity.y);
        
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
            // Debug.DrawLine(transform.position, transform.position + new Vector3(0, 1, 0), Color.aliceBlue, swingTime);
        }
        else if (m_PlayerMovement.y < -deadzone)
        {
            attHitBoxD.enabled = true;
            // Debug.DrawLine(transform.position, transform.position + new Vector3(0, -1, 0), Color.aliceBlue, swingTime);
        }
        else
        {
            attHitBox.enabled = true;
            
        }
        yield return new WaitForSeconds(swingTime);
        attHitBox.enabled = false;
        attHitBoxU.enabled = false;
        attHitBoxD.enabled = false;
        isSwinging = false;
    }
    public void attack(GameObject enemy)
    {
        //this has access to enemy hitbox gameobject, create another script to pass through damage
        var Host = enemy.GetComponent<HitboxPass>().passHost();
        Host.GetComponent<Enemy>().damage(gameObject);
        StartCoroutine(onHit());
    }

    public void damage(GameObject enemy)
    {
        // Debug.Log("Damage recived, sent by " + enemy);
        // Debug.Log("Before damage, Health:" + health);

        if (InvSec >= maxInvSec)
        {
            HealthBar[health - 1].GetComponent<Health>().FireOff();
            health -= enemy.GetComponent<Enemy>().getDamage();
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
    
}


