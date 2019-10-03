using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetPhoton : MonoBehaviourPunCallbacks {

	public CameraControl camera;
	// Use this for initialization
	void Start () {
		
		PhotonNetwork.ConnectUsingSettings();
	}
	
	// Update is called once per frame
 	public override void OnConnectedToMaster() {
		PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);

	}

	public override void OnJoinedRoom()
	{

		if (PhotonNetwork.PlayerList.Length == 1)
		{
			var obj = PhotonNetwork.Instantiate("Player", new Vector3(0, 1), Quaternion.identity);
			camera.Target = obj.transform;
		}
		else
		{
			var obj = PhotonNetwork.Instantiate("Player", new Vector3(1, 1), Quaternion.identity);
			camera.Target = obj.transform;

		}
	}
}
