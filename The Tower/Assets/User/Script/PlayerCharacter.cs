using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;

namespace Player
{
    [RequireComponent(typeof(PlayerContorol))]
    public class PlayerCharacter : MonoBehaviourPunCallbacks
    {

        public LayerMask mask;

        public GameObject[] itemtype;
        public GameObject[] DscImage;

        public Text itemCapText;
        public Slider HP_Slider;
        public Slider Power_Slider;
        public GameObject PowerMax;
        public RectTransform selectImage;
        public Sprite[] ItemSprite;
        public Image[] ItemImage;
        public GameObject gun;
        public int select = 0;
        public int[] objNumber;
        public Transform RayPos;

        private float HP = 100;
        private int itemCap;
        private GameObject[] items;
        private float time;
        private Animator animator;
        private bool setKey;
        public int power = 0;
        
        public int ComNum;
        private string[] R = { "R", "R2", "R3", "R4" }, L = { "L", "L2", "L3", "L4" }, B = { "B", "B2", "B3", "B4" };
        private string[] LT = { "L_Trigger", "L2_Trigger", "L3_Trigger", "L4_Trigger" }, RT = { "R_Trigger", "R2_Trigger", "R3_Trigger", "R4_Trigger" };
        bool b = true; 
        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            itemtype = Resources.LoadAll("Prefab", typeof(GameObject)).Cast<GameObject>().ToArray();
            items = new GameObject[3];
            objNumber = new int[itemtype.Length];
            itemCap = 100;

        }

        // Update is called once per frame
        void FixedUpdate()
        {
			if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			{
				return;
			}
			itemCapText.text = itemCap + "MB";
				ItemesCollect();
				HP_Slider.value = HP;
				if (items[select] != null)
				{
					Power_Slider.maxValue = items[select].GetComponent<PrefabNumbr>().powersave;
				}
				Power_Slider.value = power;
				if (Power_Slider.maxValue == Power_Slider.value)
				{
					PowerMax.SetActive(true);
				}
				else
				{
					PowerMax.SetActive(false);
				}

				if (setKey)
				{
					var tri = Input.GetAxis(RT[ComNum]);
					if (Input.GetMouseButtonDown(0) || (tri > 0))
					{
						Invoke("ItemSet", 0.2f);
					}
				}
				SelectItem();
				Gun();
			
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Memory")
            {
                Destroy(other.gameObject);
                itemCap += 100;
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Block")
            {
                var damegeLog = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude * collision.gameObject.GetComponent<Rigidbody>().mass;
                if (damegeLog > 5)
                {
                    HP -= damegeLog;
                }
                Debug.Log(damegeLog);
            }
        }

        void ItemesCollect()
        {
            Ray ray = new Ray(RayPos.position, RayPos.forward);
            RaycastHit hit;

            DscImage[0].SetActive(false);
            DscImage[1].SetActive(false);
            DscImage[2].SetActive(false);

            if (Physics.Raycast(ray, out hit, 2.0f, mask))
            {
                if (items[select] == null)
                {
                    var cap = hit.collider.GetComponent<PrefabNumbr>().CapaCity;
                    if (itemCap >= cap)
                    {
                        if (hit.collider.tag == "Block")
                        {

                            DscImage[0].SetActive(true);
                            DscImage[2].SetActive(true);
                        }


                        if (Input.GetMouseButton(0) || Input.GetButton(B[ComNum]))
                        {

                            animator.SetBool("Block", true);
                            time += Time.deltaTime;
                            var objtime = hit.collider.GetComponent<PrefabNumbr>().time;
                            if (time > objtime)
                            {


                                if (hit.collider.tag == "Block")
                                {
                                    objNumber[select] = hit.collider.GetComponent<PrefabNumbr>().Number;
                                    items[select] = itemtype[objNumber[select]];
                                    itemCap -= items[select].GetComponent<PrefabNumbr>().CapaCity;
                                }

                                Destroy(hit.collider.gameObject);

                            }

                        }
                    }
                    else
                    {
                        if (hit.collider.tag == "Block")
                        {

                            DscImage[1].SetActive(true);
                            DscImage[2].SetActive(true);
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0) || Input.GetButtonUp(B[ComNum]))
                {
                    animator.SetBool("Block", false);
                    time = 0;
                }
            }
            else
            {
                animator.SetBool("Block", false);
                time = 0;
            }

        }


        void ItemSet()
        {
            if (items[select] != null)
            {

                if (items[select].tag == "Block")
                {
                    var itemnumber = items[select].GetComponent<PrefabNumbr>();
                    var pos = transform.position + transform.forward*itemnumber.size.x;
                    pos.y = pos.y + itemnumber.size.y;
                    Instantiate(items[select], pos, transform.rotation);
                    items[select] = null;
                }
            }
        }

        private void SelectItem()
        {

            if (Input.GetKeyDown("1"))
            {
                select = 0;
                if (b == false)
                {
                    b = !b;
                }
            }
            if (Input.GetKeyDown("2"))
            {
                select = 1;
                if (b == false)
                {
                    b = !b;
                }
            }
            if (Input.GetKeyDown("3"))
            {
                if (b == false)
                {
                    b = !b;
                }
                select = 2;
            }


            if (Input.GetButtonDown(R[ComNum]))
            {
                if (b == false)
                {
                    b = !b;
                }
                select += 1;
                if (select > 2)
                {
                    select = 0;
                }
            }
            else if (Input.GetButtonDown(L[ComNum]))
            {
                if (b == false)
                {
                    b = !b;
                }
                select -= 1;
                if (select < 0)
                {
                    select = 2;
                }
            }

            if (select == 0)
            {
                selectImage.localPosition = new Vector3(-32, 0, 0);
            }
            else if (select == 1)
            {
                selectImage.localPosition = new Vector3(0, 0, 0);
            }
            else if (select == 2)
            {
                selectImage.localPosition = new Vector3(32, 0, 0);
            }


            for (int i = 0; i < 3; i++)
            {
                if (items[i] == null)
                {
                    ItemImage[i].sprite = ItemSprite[0];
                }
                else if (items[i].tag == "Block")
                {
                    ItemImage[i].sprite = ItemSprite[1];
                }
            }

            
        }

        void Gun()
        {
            var L_Trigger = Input.GetAxis(LT[ComNum]);
            var R_Trigger = Input.GetAxis(RT[ComNum]);
            if (Input.GetMouseButton(1)||L_Trigger>0)
            {
                if ((items[select] != null) &&items[select].GetComponent<PrefabNumbr>().powersave>power)
                {
                    if (itemCap > 0)
                    {
                        power++;
                        itemCap--;
                    }
                }
                setKey = false;
                animator.SetBool("EGUN", true);
                if (Input.GetMouseButtonDown(0)||R_Trigger>0)
                {
                    if (items[select] != null)
                    {
                        var ItemNumber = items[select].GetComponent<PrefabNumbr>();
                        var InstPos = gun.transform.position + gun.transform.forward*ItemNumber.size.x;
                        InstPos.y = InstPos.y + ItemNumber.size.y;

                        var bullet = Instantiate(items[select], InstPos, transform.rotation);
                        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                        //var power = bulletRb.mass * itemCap * 20;
                        bulletRb.velocity = bullet.transform.forward*power;
                        //itemCap += items[select].GetComponent<PrefabNumbr>().CapaCity;
                        power = 0;
                        items[select] = null;

                    }
                }
            }
            else
            {
                itemCap += power;
                power = 0;
                animator.SetBool("EGUN", false);
                setKey = true;
            }

        }
    }
}