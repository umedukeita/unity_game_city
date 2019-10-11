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
		var obj = PhotonNetwork.Instantiate("Player", new Vector3(i, 1), Quaternion.identity);
		camera.Target = obj.transform;
		i += 10;
		//PhotonNetwork.ConnectUsingSettings();
	}
	
	// Update is called once per frame
 	/*public override void OnConnectedToMaster() {
		PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);

	}*/

	public override void OnJoinedRoom()
	{

<<<<<<< HEAD
=======
		var obj = PhotonNetwork.Instantiate("Player", new Vector3(i, 1), Quaternion.identity);
		camera.Target = obj.transform;

>>>>>>> ef1f7c4cc2e057efff534fe9f57ae184e3f1ef98
	}
}
