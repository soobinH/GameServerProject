using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 사용을 위해 추가

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button connectButton; // Inspector에서 버튼 할당

    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        if (connectButton != null)
        {
            connectButton.onClick.AddListener(ConnectToServer);
        }
    }

    // 서버 연결 함수
    public void ConnectToServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            if (connectButton != null)
            {
                connectButton.interactable = false; // 연결 시도 중에는 버튼 비활성화
            }
        }
    }

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PhotonNetwork.LoadLevel("Lobby");
    }

    // 연결 실패시 버튼 다시 활성화
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log($"Disconnected: {cause}");
        if (connectButton != null)
        {
            connectButton.interactable = true;
        }
    }
}