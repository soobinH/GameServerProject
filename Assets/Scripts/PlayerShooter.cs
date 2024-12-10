using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShooter : MonoBehaviourPun
{
    public Gun gun;
    public Transform gunPivot;
    public Transform leftHandMount;
    public Transform rightHandMount;
    private PlayerInput playerInput;
    private Animator playerAnimator;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!photonView.IsMine) return; // 자신이 아니면 그대로 리턴

        if (playerInput.fire) gun.Fire();
        else if (playerInput.reload)
        {
            if (gun.Reload()) playerAnimator.SetTrigger("Reload");
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (gun != null && UIManager.Instance != null)
        {
            UIManager.Instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //총의 기준점을 3d모델의 오른쪽 팔꿈치로 이동
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);

    }
}
