using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;
    public float speedSmoothTime = 0.1f;
    public Vector3 velocity;
    [Range(0.01f, 0.1f)] public float airControlPercent;

    public float currentSpeed => new Vector2(characterController.velocity.x,
    characterController.velocity.z).magnitude;

    private float currentVelocityY;
    private float speedSmoothVelocity;
    private float turnSmoothVelocity;

    private CharacterController characterController;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Move(playerInput.moveInput);
        Rotate();

        playerAnimator.SetFloat("Move", playerInput.move);
    }

    public void Move(Vector2 moveInput)
    {
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        //+ playerInput.rotate * transform.right * moveSpeed * Time.deltaTime;

        /*
        var targetSpeed = moveSpeed * moveInput.magnitude;
        var moveDirection = Vector3.Normalize(transform.forward * moveInput.y +
        transform.right * moveInput.x);

        var smoothTime = characterController.isGrounded ? speedSmoothTime : speedSmoothTime / airControlPercent;

        targetSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        currentVelocityY += Time.deltaTime * Physics.gravity.y;

        var velocity = moveDirection * targetSpeed + Vector3.up * currentVelocityY;

        characterController.Move(velocity * Time.deltaTime);

        if (characterController.isGrounded) currentVelocityY = 0f;

        */
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);

    }

    void Rotate()
    {
        float rotate = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, rotate, 0f);

        /*
        var targetRotation = transform.forward.x;

        targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, speedSmoothTime);

        transform.eulerAngles = Vector3.up * targetRotation;
        */
    }
}
