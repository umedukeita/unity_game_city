using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerCon : MonoBehaviour
{

    public LayerMask mask;

    public GameObject[] itemtype;
    public GameObject DscImage;

    public Text itemCapText;
    public RectTransform selectImage;
    public Sprite[] ItemSprite;
    public Image[] ItemImage;
    public GameObject gun;
    public int select = 0;
    public int[] objNumber;
    public Transform RayPos;

    private int itemCap;
    private GameObject[] items;
    private float time;
    private Animator animator;
    private bool setKey;

    public int ComNum;
    private string[] H = { "Horizontal", "Horizontal2", "Horizontal3", "Horizontal4" };
    private string[] V = { "Vertical", "Vertical2", "Vertical3", "Vertical4" };
    private string[] RH = { "RightH", "RightH2", "RightH3", "RightH4" };
    private string[] LV = { "LeftV", "LeftV2", "LeftV3", "LeftV4" };

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
        itemCapText.text = itemCap + "MB";
        ItemesCollect();
        if (setKey)
        {
            float tri = Input.GetAxis("L_R_Trigger");
            if (Input.GetMouseButtonDown(0) || (tri < 0))
            {
               Invoke("ItemSet",0.2f);
            }
        } 
        SelectItem();
        Gun();
    }

    void ItemesCollect()
    {
        Ray ray = new Ray(RayPos.position, RayPos.forward);
        RaycastHit hit;

        DscImage.SetActive(false);

        if (Physics.Raycast(ray, out hit, 2.0f, mask))
        {
            if (items[select] == null)
            {
                var cap= hit.collider.GetComponent<PrefabNumbr>().CapaCity;
                if (itemCap >= cap)
                {
                    if (hit.collider.tag == "Block")
                    {

                        DscImage.SetActive(true);
                    }

                    if (Input.GetMouseButton(0) || Input.GetButton("B"))
                    {

                        animator.SetBool("Block", true);
                        time += Time.deltaTime;
                        var objtime= hit.collider.GetComponent<PrefabNumbr>().time;
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
            }
            else if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("B"))
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

                var pos = transform.position + transform.forward;
                pos.y = pos.y + items[select].GetComponent<PrefabNumbr>().size;
                Instantiate(items[select], pos, transform.rotation);
                itemCap += items[select].GetComponent<PrefabNumbr>().CapaCity;
                items[select] = null;
            }
        }
    }
    
    private void SelectItem()
    {
        
        if (Input.GetKeyDown("1"))
        {
            select = 0;
            if (b==false)
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


        if (Input.GetButtonDown("R"))
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
        else if (Input.GetButtonDown("L"))
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

        if (items[select] != null)
        {
        }
    }

    void Gun()
    {
        if (Input.GetMouseButton(1))
        {
            setKey = false;
            animator.SetBool("EGUN", true);
            if (Input.GetMouseButtonDown(0))
            {
                if (items[select]!=null){

                    var InstPos = gun.transform.position + gun.transform.forward;
                    InstPos.y = InstPos.y + items[select].GetComponent<PrefabNumbr>().size;
                    
                    var bullet = Instantiate(items[select],InstPos, transform.rotation);
                    Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                    var power = bulletRb.mass * itemCap*20;
                    bulletRb.AddForce(bullet.transform.forward * power);
                    itemCap += items[select].GetComponent<PrefabNumbr>().CapaCity;
                    items[select] = null;
                    
                }
            }
        }
        else
        {
            animator.SetBool("EGUN", false);
            setKey = true;
        }

    }
}
