using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class ImageBoxCon : MonoBehaviour
    {

        public MeshFilter Image;
        public PlayerCharacter player;
	// Use this for initialization
	void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
           /* if (player.select >= 0)
            {
                Image.sharedMesh = player.itemtype[player.objNumber[player.select]].GetComponent<MeshFilter>().sharedMesh;
            }*/
        }
    }

}