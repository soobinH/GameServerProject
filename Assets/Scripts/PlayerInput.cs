using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public readonly string moveAxis = "Vertical";
    public readonly string rotateAxis = "Horizontal";
    public readonly string fireButton = "Fire1";
    public readonly string reloadButton = "Reload";

    #region 움직임,회전,발사,재장전 입력값을 받는 프로퍼티들
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    #endregion

    void Update()
    {
        //게임오버 상태에서는 입력을 감지하지않음
        if (GameManager.instance != null && GameManager.instance.isGameOver)
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        move = Input.GetAxis(moveAxis);
        rotate = Input.GetAxis(rotateAxis);
        fire = Input.GetButton(fireButton);
        reload = Input.GetButtonDown(reloadButton);
    }
}
