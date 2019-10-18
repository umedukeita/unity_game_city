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

		if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
		{
			i = 0;
            PhotonNetwork.Instantiate("Test", new Vector3(0, 1), Quaternion.identity);
			PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
		{
			i = 10;
		}
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
		{
			i = 20;
		}
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
		{
			i = 30;
		}

		var obj = PhotonNetwork.Instantiate("Player", new Vector3(i, 1), Quaternion.identity);
        camera.Target = obj;
	}
	
	
}
