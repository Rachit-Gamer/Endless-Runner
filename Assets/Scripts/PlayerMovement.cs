using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f; // Default player speed
    [SerializeField] private float jumpForce = 10f; // Jump force

    private PlayerAction controls;
    private Vector2 moveInput;
    private Rigidbody rb;
    private bool isGrounded;

    private void Awake()
    {
        controls = new PlayerAction();
        controls.Player.Move.performed += performed => moveInput = performed.ReadValue<Vector2>();
        controls.Player.Move.canceled += cancelled => moveInput = Vector2.zero;
        controls.Player.Jump.performed += _ => Jump();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.Translate(moveDirection * playerSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
