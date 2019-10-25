using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		
			//if (Input.GetKey(KeyCode.Escape)) Quit();
		
	}

	public void Back()
	{
		PhotonNetwork.LeaveRoom();
		SceneManager.LoadScene("Lobby");
		
	}

	void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
	}
}
