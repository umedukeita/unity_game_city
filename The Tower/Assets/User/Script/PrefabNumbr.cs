using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PrefabNumbr : MonoBehaviour {

    public int Number;
    public int CapaCity;
    public Vector3 size;
    public float time;
    public int powersave;

	private void FixedUpdate()
	{
		if (this.gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 0.001)
		{
			if (this.gameObject.GetComponent<PhotonView>() != null)
			{
				Destroy(this.gameObject.GetComponent<PhotonTransformView>());
				Destroy(this.gameObject.GetComponent<PhotonRigidbodyView>());
				Destroy(this.gameObject.GetComponent<PhotonView>());
			}
		}
	}
}
