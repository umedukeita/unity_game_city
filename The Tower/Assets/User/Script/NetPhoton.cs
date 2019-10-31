﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetPhoton : MonoBehaviourPunCallbacks {

	public CameraControl camera;
	private Color color;
	private Vector3 v;
	// Use this for initialization
	void Start () {
		if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
		{
			v.x = -200;
			v.z = 200;
			color = new Color(180, 0, 180);
			//PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
		{
			v.x = 200;
			v.z = 200;
			color = new Color(0, 180, 0);
		}
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
		{
			v.x = -200;
			v.z = -200;
			color = new Color(200, 200, 0);
		}
		else if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
		{
			v.x = 200;
			v.z = -200;
			color = new Color(0, 100, 255);
		}
        
        var obj = PhotonNetwork.Instantiate("Player", v, Quaternion.identity);
		var objchild = obj.transform.GetChild(0);
		objchild.GetComponent<Renderer>().material.color = color;
        camera.Target = obj;
	}
	
	
}
