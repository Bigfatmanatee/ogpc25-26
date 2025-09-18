using JetBrains.Rider.Unity.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 m_PlayerMovement;
    private InputAction m_MoveAction;
    private InputAction m_AttackAction;
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        m_MoveAction = new InputAction("Move");
        m_MoveAction.AddBinding("<Gamepad>/leftStick");
        m_MoveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        m_MoveAction.Enable();

        m_AttackAction = new InputAction("Attack");
        m_AttackAction.AddBinding("<Gamepad>/buttonSouth");
        m_AttackAction.AddBinding("<Keyboard>/k");
        m_AttackAction.Enable();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        m_PlayerMovement = m_MoveAction.ReadValue<Vector2>();
        if (m_PlayerMovement != Vector2.zero)
            Debug.Log("Vector = " + m_PlayerMovement);
            rb.AddForce(m_PlayerMovement * speed);

        var attacking = m_AttackAction.ReadValue<float>();
        if (Mathf.Approximately(attacking, 1f))
            Debug.Log("Attacking");
    }
}
