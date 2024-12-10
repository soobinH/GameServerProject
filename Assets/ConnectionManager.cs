using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // 포톤 서버 연결
    }

    public override void OnConnected() // 서버 접속성공시 호출
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
}
