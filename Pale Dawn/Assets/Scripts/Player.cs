using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 m_PlayerMovement;
    private InputAction m_MoveAction;
    private InputAction m_AttackAction;
    private InputAction m_JumpAction;
    private Rigidbody2D rb;
    private LayerMask LmG; 
    
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private Transform groundCheck;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LmG = LayerMask.GetMask("Ground");
        
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

    private void Awake() {
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

        var attacking = m_AttackAction.ReadValue<float>(); //temp attack code
        if (Mathf.Approximately(attacking, 1f))
            Debug.Log("Attacking");



        bool jumping = false;
        var jumpRead = m_JumpAction.ReadValue<float>();
        if (Mathf.Approximately(jumpRead, 1f))
        {
            //Debug.Log("Jumping");
            //Debug.Log("IsGrounded() =" + isGrounded());
            jumping = true;
        }

        //Debug.Log("Jumping: "+jumping);
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
            rb.gravityScale = 1.75f;
        } 
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void FixedUpdate()
    {
         rb.linearVelocity = new Vector2(m_PlayerMovement.x * speed, rb.linearVelocity.y);
    }
}
