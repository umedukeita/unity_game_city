using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using System;

public class PlayerCharacter : MonoBehaviourPunCallbacks
{

    public LayerMask mask;

    public GameObject[] itemtype;
    public GameObject gun;

    private GameObject[] items;
    private GameObject[] DscImage;
    private GameObject PowerMax;
    private GameObject Gauge;
    private GameObject DamageImage;
	private GameObject rank;


	private Image GaugeImage;
    private Image[] ItemImage;

    private RectTransform selectImage;

    private Text itemCapText;
	private Text rankText;
	private Text TimeLimit;


    private Slider HP_Slider;
    private Slider Power_Slider;

    private Animator animator;

	private float time;

    private int itemCap;
	private int dead=0;
    private bool setKey;

	public float HP = 100;
	public float timelimit=120;
	public int power = 0;
    public int select = 0;
    public int[] objNumber;

    public Sprite[] ItemSprite;

    public Transform RayPos;

    public int ComNum;
    private string[] R = { "R", "R2", "R3", "R4" }, L = { "L", "L2", "L3", "L4" }, B = { "B", "B2", "B3", "B4" };
    private string[] LT = { "L_Trigger", "L2_Trigger", "L3_Trigger", "L4_Trigger" }, RT = { "R_Trigger", "R2_Trigger", "R3_Trigger", "R4_Trigger" };
	private bool gameEnd = true;
    bool b = true;
    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();

        objNumber = new int[itemtype.Length];

        itemCap = 100;
        items = new GameObject[3];
        DamageImage = GameObject.Find("Damege");
       
        DscImage = new GameObject[3];
        DscImage[2] = GameObject.Find("1pCanvas");
        DscImage[1] = GameObject.Find("1PTEXT2");
        DscImage[0] = GameObject.Find("1PTEXT");
		rank = GameObject.Find("Rank");
	
		rankText = GameObject.Find("RankText").GetComponent<Text>();

		TimeLimit = GameObject.Find("TimeLimit").GetComponent<Text>();
		TimeLimit.text = "0";
		itemCapText = GameObject.Find("ItemPoint_1P").GetComponent<Text>();

        HP_Slider = GameObject.Find("Slider_1P").GetComponent<Slider>();

        Power_Slider = GameObject.Find("PowerSlider1").GetComponent<Slider>();

        PowerMax = GameObject.Find("Max");

        Gauge = GameObject.Find("Gauge");
        GaugeImage = Gauge.GetComponent<Image>();
        GaugeImage.fillAmount = 0;

        var frame = GameObject.Find("Itemframe_1P").gameObject.transform;
        selectImage = frame.GetChild(0).GetComponent<RectTransform>();

        ItemImage = new Image[3];
        for (int i = 1; i < 4; i++)
        {
            ItemImage[i - 1] = frame.GetChild(i).GetComponent<Image>();
        }

		var properties = new ExitGames.Client.Photon.Hashtable();
		properties.Add("Dead", dead);
		PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

		DamageImage.SetActive(false);
		rank.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            itemCapText.text = itemCap + "MB";
			timelimit -= Time.deltaTime;
			TimeLimit.text = timelimit.ToString("F2");
			if (timelimit <= 0)
			{
				var properties = new ExitGames.Client.Photon.Hashtable();
				properties["Dead"]=dead;
				PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
				photonView.RPC("GameEnd", RpcTarget.All);
                TimeLimit.text = "終了";
            }
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

			
            /*if (setKey)
            {
                var tri = Input.GetAxis(RT[ComNum]);
                if (Input.GetMouseButtonDown(0) || (tri > 0))
                {
                    Invoke("ItemSet", 0.2f);
                }
            }*/
            SelectItem();
            Gun();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Memory")
        {
			other.gameObject.SetActive(false);
			itemCap += 100*(dead+1);
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Block"&&gameEnd)
        {
			if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 5)
			{
				var damegeLog = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude * collision.gameObject.GetComponent<Rigidbody>().mass / 50;
				if (damegeLog >= 10)
				{
					HP -= (int)damegeLog;
					DamageEffect();
				}
				if (HP <= 0)
				{
					dead++;

					this.transform.position = new Vector3(0, -19, 0);
					Invoke("ReSpawn", 5f);
				}
				Debug.Log((int)damegeLog);
			}

        }
    }


	void ReSpawn()
	{
		HP = 100;

		itemCap += 100 * (dead + 1);
        Vector3 vector=new Vector3(-200, 0, 200); ;
        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0: vector = new Vector3(-200, 0, 200); break;
            case 1: vector = new Vector3(200, 0, 200); break;
            case 2: vector = new Vector3(-200, 0, -200); break;
            case 3: vector = new Vector3(200, 0, -200); break;
        }
		this.transform.position = vector;
	}

    void DamageEffect()
    {
        if (DamageImage.activeSelf)
        {
            DamageImage.SetActive(false);
        }
        else
        {
            DamageImage.SetActive(true);
            Invoke("DamageEffect", 0.5f);
        }
    }

    void ItemesCollect()
    {
        Ray ray = new Ray(RayPos.position, RayPos.forward);
        RaycastHit hit;

        DscImage[0].SetActive(false);
        DscImage[1].SetActive(false);
        DscImage[2].SetActive(false);
        Gauge.SetActive(false);

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

                        Gauge.SetActive(true);
                        GaugeImage.fillAmount = time / objtime;
                        if (time > objtime)
                        {


                            if (hit.collider.tag == "Block")
                            {
                                objNumber[select] = hit.collider.GetComponent<PrefabNumbr>().Number;
                                items[select] = itemtype[objNumber[select]];
                                itemCap -= items[select].GetComponent<PrefabNumbr>().CapaCity;
                            }

                            //Destroy(hit.collider.gameObject);
                            //PhotonNetwork.Destroy(hit.collider.gameObject);
                            photonView.RPC("DestroyObject", RpcTarget.All,hit.collider.gameObject.name);
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

                var InstPos = gun.transform.position + gun.transform.forward * itemnumber.size.x;
                InstPos.y = InstPos.y + itemnumber.size.y;
                PhotonNetwork.Instantiate(items[select].name, InstPos, transform.rotation); ;
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

        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            select--;
           
			if (select < 0)
			{
				select = 2;
			}
		}
        else if (scroll < 0)
        {
            select++;
			if (select > 2)
			{
				select = 0;
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
        if (Input.GetMouseButton(1) || L_Trigger > 0)
        {
            if ((items[select] != null) && items[select].GetComponent<PrefabNumbr>().powersave > power)
            {
                if (itemCap > 0)
                {
                    power++;
                    itemCap--;
                }
            }
            setKey = false;
            animator.SetBool("EGUN", true);
            if (Input.GetMouseButtonDown(0) || R_Trigger > 0)
            {
                if (items[select] != null)
                {
                    var ItemNumber = items[select].GetComponent<PrefabNumbr>();
                    var InstPos = gun.transform.position + gun.transform.forward * ItemNumber.size.x;
                    InstPos.y = InstPos.y + ItemNumber.size.y;

                    var bullet = PhotonNetwork.Instantiate(items[select].name, InstPos, transform.rotation);
                    Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                    //var power = bulletRb.mass * itemCap * 20;
                    bulletRb.velocity = bullet.transform.forward * power;
                    //itemCap += items[select].GetComponent<PrefabNumbr>().CapaCity;
                    power = 0;
                    items[select] = null;
					Debug.Break();
                    //Debug.Break();
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



	[PunRPC]
	void DestroyObject(string name)
	{
		var gameObject = GameObject.Find(name);
		if (gameObject.GetComponent<PhotonView>() != null)
		{
			if (gameObject.GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
			{
				PhotonNetwork.Destroy(gameObject);
				Debug.Log("M");
			}
		}
		else
		{
			Debug.Log("E");
			Destroy(gameObject);
		}
	}
	[PunRPC]
	void GameEnd()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		gameEnd = false;
		rank.SetActive(true);
		var juni_a= new int[4];
		var juni_b = new int[4];
		object p_object;
		for (int i = 0; i < 4; i++)
		{	if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("Dead", out p_object))
			{
				juni_a[i] = (int)p_object;
				juni_b[i] = (int)p_object;
			}
		}
		Array.Sort(juni_a);

		var text = new string[4];
		
		rankText.text = "";
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (juni_b[i]==juni_a[j])
				{
					text[i] = "第" + (j+1) + "位" + "　　" +PhotonNetwork.PlayerList[i].NickName;
					if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.LocalPlayer.NickName)
					{
						text[i] += "　←YOU";
					}
					break;
				}
			}
		}
		for (int i = 0; i < 4; i++)
		{
			rankText.text += text[i]+"\n";
		}
		
	}
}