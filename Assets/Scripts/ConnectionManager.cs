using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI ����� ���� �߰�

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button connectButton; // Inspector���� ��ư �Ҵ�

    void Start()
    {
        // ��ư�� Ŭ�� �̺�Ʈ �߰�
        if (connectButton != null)
        {
            connectButton.onClick.AddListener(ConnectToServer);
        }
    }

    // ���� ���� �Լ�
    public void ConnectToServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            if (connectButton != null)
            {
                connectButton.interactable = false; // ���� �õ� �߿��� ��ư ��Ȱ��ȭ
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

    // ���� ���н� ��ư �ٽ� Ȱ��ȭ
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