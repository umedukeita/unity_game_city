using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour {

	//public PlayerCon player;
	public GameObject item;
    public float ReSpwanTime;
	private float time;
    void Start () {

    }

    // Update is called once per frame
    void Update() {


		if (item.activeSelf == false)
		{
			time += Time.deltaTime;
			if (time > ReSpwanTime)
			{
				item.gameObject.SetActive(true);
				time = 0;
			}

		}
    }

    
    

}
