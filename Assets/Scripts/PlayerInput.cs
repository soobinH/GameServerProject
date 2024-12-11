using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Cinemachine;

public class PlayerInput : MonoBehaviourPun
{
    public string moveAxis = "Vertical";
    public string moveHorizontalAxis = "Horizontal";
    public string moveVerticalAxis = "Vertical";
    public string rotateAxis = "Horizontal";
    public string fireButton = "Fire1";
    public string reloadButton = "Reload";

    #region 움직임,회전,발사,재장전 입력값을 받는 프로퍼티들
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    public Vector2 moveInput { get; private set; }
    public Vector3 moveInput3 { get; private set; }
    #endregion

    void Update()
    {
        if (!photonView.IsMine) return; // 자신이 아니면 그대로 리턴
        //게임오버 상태에서는 입력을 감지하지않음
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            moveInput = Vector2.zero;
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        moveInput = new Vector2(Input.GetAxis(moveHorizontalAxis), Input.GetAxis(moveVerticalAxis));
        moveInput3 = new Vector3(Input.GetAxis(moveHorizontalAxis),0, Input.GetAxis(moveVerticalAxis));
        if (moveInput.sqrMagnitude > 1) moveInput = moveInput.normalized;
        move = Input.GetAxis(moveAxis);
        rotate = Input.GetAxis(rotateAxis);
        fire = Input.GetButton(fireButton);
        reload = Input.GetButtonDown(reloadButton);
    }
}
