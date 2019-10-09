using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetPhoton : MonoBehaviourPunCallbacks {

	public CameraControl camera;

	private int i = 0;
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

		var obj = PhotonNetwork.Instantiate("Player", new Vector3(i, 1), Quaternion.identity);
		camera.Target = obj.transform;

	}
}
