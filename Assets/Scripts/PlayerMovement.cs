using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;
    public float speedSmoothTime = 0.1f;
    public Vector3 velocity;

    private float currentVelocityY;
    private float speedSmoothVelocity;
    private float turnSmoothVelocity;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;



    private Camera _camera;
    private bool isLookAt = true;

    void Start()
    {
        _camera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //UpdateAnimation(playerInput.moveInput);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return; // 자신이 아니면 그대로 리턴

        #region 마우스가 바라보는 방향으로 플레이어 회전
        if (isLookAt == false) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 direction = ray.GetPoint(distance) - transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }

        Move(playerInput.moveInput);
        #endregion

        //Rotate();

        playerAnimator.SetFloat("Move", playerInput.move);

    }

    public void Move(Vector2 moveInput)
    {

        //Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //playerRigidbody.MovePosition(transform.position + m_Input * Time.fixedDeltaTime * moveSpeed);

        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
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

    private void UpdateAnimation(Vector2 moveInput)
    {
        var animationSpeedPercent = moveSpeed;

        //입력으로 moveInput값을 받아서 애니메이터의 VerticalMove와 Horizontal Move 파라미터를 전달한다
        playerAnimator.SetFloat("Vertical Move", moveInput.y * animationSpeedPercent, 0.05f, Time.deltaTime);
        playerAnimator.SetFloat("Horizontal Move", moveInput.x * animationSpeedPercent, 0.05f, Time.deltaTime);
        //수직 수평 방향 움직임이 moveInput값으로 즉시 변화되는게 아니라 이전 값에서 지금 설정한 값으로 연속적으로 부드럽게 변화하게된다
    }
}
