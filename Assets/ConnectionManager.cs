using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // ���� ���� ����
    }

    public override void OnConnected()
    {
        base.OnConnected();
    }
    void Update()
    {
        
    }
}
