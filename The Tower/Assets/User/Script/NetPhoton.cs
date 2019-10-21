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
			i = -90;
            PhotonNetwork.Instantiate("Convieni", new Vector3(-90, 2.5f, -40), Quaternion.identity);
			PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
		{
			i = -100;
		}
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
		{
			i = -80;
		}
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
		{
			i = -70;
		}
        
        var obj = PhotonNetwork.Instantiate("Player", new Vector3(i, 1,-50), Quaternion.identity);
        camera.Target = obj;
	}
	
	
}
