using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class FollowCamSetup : MonoBehaviourPun
{
    public GameObject[] playerList; // �÷��̾� �迭
    public GameObject pMe; // �ڱ��ڽ� �÷��̾�

    CinemachineVirtualCamera vCamera; // �ó׸ӽ� ����ī�޶�
    void Start()
    {
        vCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player"); // �÷��̾� ����Ʈ ����
        foreach (GameObject plr in playerList) // �ڽ��� �÷��̾� ã��
        {
            PhotonView pPhotonView = plr.GetComponent<PhotonView>();
            if (pPhotonView.IsMine)
            {
                pMe = plr;
                vCamera.Follow = pMe.transform;
                vCamera.LookAt = pMe.transform;
                break;
            }
        }
    }
}
