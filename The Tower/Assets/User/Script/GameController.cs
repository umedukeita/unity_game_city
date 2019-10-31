using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class GameController : MonoBehaviourPunCallbacks
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{



		//if (Input.GetKey(KeyCode.Escape)) Quit();

	}

	public void Back()
	{
		PhotonNetwork.LeaveRoom();
		SceneManager.LoadScene("Title");

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
