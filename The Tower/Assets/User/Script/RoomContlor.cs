using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class RoomContlor : MonoBehaviourPun {

	public string roomName = "";
	private InputField inputField;
	

	// Use this for initialization
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
	}

	public void OnJoinedRoom()
	{
		
	}

	
}
