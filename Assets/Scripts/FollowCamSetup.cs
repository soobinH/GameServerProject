using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class FollowCamSetup : MonoBehaviourPun
{
    public GameObject[] playerList; // 플레이어 배열
    public GameObject pMe; // 자기자신 플레이어

    CinemachineVirtualCamera vCamera; // 시네머신 가상카메라
    void Start()
    {
        vCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player"); // 플레이어 리스트 생성
        foreach (GameObject plr in playerList) // 자신인 플레이어 찾기
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
