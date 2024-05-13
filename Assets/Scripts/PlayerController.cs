using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float initialPlayerSpeed = 4f;
    [SerializeField] private float maximumPlayerSpeed = 30f;
    [SerializeField] private float playerSpeedIncreaseRate = .1f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float initialGravityValue = -9.81f;

    [SerializeField] private LayerMask groundLayer;

    private float playerSpeed;
    private float gravity;
    private Vector3 movementDirection = Vector3.forward;
    private Vector3 playerVelocity;

    public float length;

    private PlayerInput playerInput;
    private InputAction turnAction;
    private InputAction jumpAction;
    private InputAction slideAction;

    private CharacterController controller;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        turnAction = playerInput.actions["Turn"];
        jumpAction = playerInput.actions["Jump"];
        slideAction = playerInput.actions["Slide"];
    }

    private void OnEnable()
    {
        //When the script is enabled and after awake
        turnAction.performed += PlayerTurn;
        turnAction.performed += PlayerSlide;
        turnAction.performed += PlayerJump;
    }

    private void OnDisable()
    {
        turnAction.performed -= PlayerTurn;
        turnAction.performed -= PlayerSlide;
        turnAction.performed -= PlayerJump;
    }
    private void Start()
    {
        playerSpeed = initialPlayerSpeed;
        gravity = initialGravityValue;
    }

    private void PlayerTurn(InputAction.CallbackContext context)
    {

    }
    private void PlayerSlide(InputAction.CallbackContext context)
    {

    }
    private void PlayerJump(InputAction.CallbackContext context)
    {

    }
    private void Update()
    {
        controller.Move(transform.forward * playerSpeed * Time.deltaTime);
        if (isGrounded() && playerVelocity.y<0)
        {
            playerVelocity.y = 0f;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private bool isGrounded()
    {
        Vector3 rayCastOriginFirst = transform.position;
        rayCastOriginFirst.y -= controller.height / 2f;
        rayCastOriginFirst.y += .1f;

        Vector3 rayCastOriginSecond = rayCastOriginFirst;
        rayCastOriginFirst -= transform.forward * .2f;
        rayCastOriginSecond += transform.forward * .2f;

        Debug.DrawLine(rayCastOriginFirst, Vector3.down, Color.green, 2f);
        Debug.DrawLine(rayCastOriginSecond, Vector3.down, Color.red, 2f);

        if (Physics.Raycast(rayCastOriginFirst, Vector3.down, out RaycastHit hit1, length, groundLayer) || Physics.Raycast(rayCastOriginSecond, Vector3.down, out RaycastHit hit2,length, groundLayer))
        {
            return true;
        }
        return false;
    }
}
