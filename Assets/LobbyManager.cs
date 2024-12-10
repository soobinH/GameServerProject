using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        CreateRoom();
    }
    public void CreateRoom()
    {
        // byte -> 255���� ���� ����
        // chat -> -128 ~ 128���� ���尡��

        RoomOptions roomOptions = new RoomOptions();

        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom("Zombie_1", roomOptions);
    }

    public override void OnCreatedRoom() // �� ������ ȣ���
    {
        base.OnCreatedRoom();
        Debug.Log("OnCreatedRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        Debug.Log("Room Create Failed (" + returnCode + ") : " + message);
        JoinRoom();
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Zombie_1");
    }
    public override void OnJoinedRoom() // �� ����� ȣ���
    {
        base.OnJoinedRoom();
        Debug.Log("OnJoinedRoom");
        PhotonNetwork.LoadLevel("Main");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.Log("Join Room Failed (" + returnCode + ") : " + message);
    }
}
