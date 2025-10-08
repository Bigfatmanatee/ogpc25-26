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

    [SerializeField] private Collider2D hitBox;
    [SerializeField] private Collider2D attHitBox;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform stairCheck;


    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private float swingTime;
    [SerializeField] private float swingCooldown;
    [SerializeField] private float maxInvSec;
    private float InvSec = 0;



    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        LmG = LayerMask.GetMask("Ground");
        LmE = LayerMask.GetMask("EnemyHit");
        LmA = LayerMask.GetMask("Attack");

        attHitBox.enabled = false;
        // attHitBox.GetComponent<AttColider>().isPlayer();
        attHitBox.GetComponent<AttColider>().setLayerName("EnemyHit");


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
    private bool isOnStair()
    {
        return Physics2D.OverlapCircle(stairCheck.position, 0.2f, LmG);
    }

    private void Update()
    {
        m_PlayerMovement = m_MoveAction.ReadValue<Vector2>();
        //if (m_PlayerMovement != Vector2.zero)
        //    Debug.Log("Vector = " + m_PlayerMovement);

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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
        if (m_JumpAction.WasReleasedThisFrame() && rb.linearVelocity.y > 0f) //slow down when stop holding space
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.25f);
        }
        if (!isGrounded() && !jumping) //falling without holding space
        {
            rb.gravityScale = 2f; //was 1.75
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
        
        if (rb.linearVelocityX > 0) //Facing direction
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Normal
        }
        else if (rb.linearVelocityX < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
        }
    }

    private IEnumerator swing()
    {
        attHitBox.enabled = true;
        yield return new WaitForSeconds(swingTime);
        attHitBox.enabled = false;
    }
    public void attack(GameObject enemy)
    {
        //this has access to enemy hitbox gameobject, create another script to pass through damage
        var Host = enemy.GetComponent<HitboxPass>().passHost();
        Host.GetComponent<LREnemy>().damage(this.gameObject);
    }

    public void damage(GameObject enemy)
    {
        Debug.Log("Damage recived, sent by " + enemy);
        Debug.Log("Before damage, Health:" + health);

        if (InvSec >= maxInvSec)
        {
            health -= enemy.GetComponent<LREnemy>().getDamage();
            Debug.Log("After damage taken, Health:" + health);
            // iframe stuff
            InvSec = 0;
        }
        else
        {
            Debug.Log("didnt take damage, still invincible");
        }
    }

    public int getDamage() {
        return 1;
    }
    
}


