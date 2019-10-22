using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetPhoton : MonoBehaviourPunCallbacks {

	public CameraControl camera;
	private Vector3 v;
	// Use this for initialization
	void Start () {

		if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
		{
			v.x = -200;
			v.z = 200;
			PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
		{
			v.x = 200;
			v.z = 200;
		}
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
		{
			v.x = -200;
			v.z = -200;
		}
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
		{
			v.x = 200;
			v.z = -200;
		}
        
        var obj = PhotonNetwork.Instantiate("Player", v, Quaternion.identity);
        camera.Target = obj;
	}
	
	
}
