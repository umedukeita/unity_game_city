using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class RoomController : MonoBehaviourPun
{

    [SerializeField]
    Button buttonCreateRoom;

    private string roomName = "test";

    public void Awake()
    {
        // ここでPhotonに接続している
        // 0.0.1はゲームのバージョンを指定
        // （異なるバージョン同士でマッチングしないように？）
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Start()
    {
     
    }
   

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(
            roomName,
            new Photon.Realtime.RoomOptions() {
                MaxPlayers = 4,
                CustomRoomProperties  = new ExitGames.Client.Photon.Hashtable()
                {

                }
            }
            
            );

    }
}
