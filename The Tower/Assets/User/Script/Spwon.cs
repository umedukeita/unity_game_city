using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spwon : MonoBehaviour {
    float time = 0;
    public GameObject item; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time > 3)
        {
            Instantiate(item, this.transform.position, this.transform.rotation);
            time = 0;
        }
	}
}
