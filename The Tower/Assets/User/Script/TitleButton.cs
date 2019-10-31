using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour {

	public void Button(int i)
	{
		if (i == 0)
		{
			SceneManager.LoadScene("Lobby");
		}
		else if (i == 1)
		{
			Application.Quit();
		}
	}
}
