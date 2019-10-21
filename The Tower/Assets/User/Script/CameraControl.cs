using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Target;
	[SerializeField]
    public float DistanceToPlayerM = 2f;    // カメラとプレイヤーとの距離[m]
    public float HeightM = 1.2f;            // 注視点の高さ[m]
    public float RotationSensitivity = 100f;// 感度
    public int PlayerNumber;

	private float rotX = 0, rotY = 0;
    /*private string[] H = { "Stick_Horizontal_R", "Stick_Horizontal_R2", "Stick_Horizontal_R3", "Stick_Horizontal_R4" };
    private string[] V = { "Stick_Vertical_R", "Stick_Vertical_R2", "Stick_Vertical_R3", "Stick_Vertical_R4" };
	*/
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        /*if (Target == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }*/
    }

    void FixedUpdate()
    {
       
        if (Target != null)
        {
            var HP = Target.GetComponent<PlayerCharacter>().HP;
            if (HP >= 0)
            {
                rotX += Input.GetAxis("Mouse X");
                rotY -= Input.GetAxis("Mouse Y");
                /*
                var rotX = Input.GetAxis(H[PlayerNumber])*2f;
                var rotY = Input.GetAxis(V[PlayerNumber])*2f;
                */

                var lookAt = Target.transform.position + Vector3.up * HeightM;

                rotY = Mathf.Clamp(rotY, -80, 60); //縦回転角度制限する

                transform.eulerAngles = new Vector3(-rotY, rotX, 0.0f); //回転の実行
                                                                        // カメラとプレイヤーとの間の距離を調整
                transform.position = lookAt - transform.forward * DistanceToPlayerM;

                // 注視点の設定
                transform.LookAt(lookAt);

                // カメラを横にずらして中央を開ける
                //transform.position = transform.position + transform.right;
            }
            else
            {
                transform.position = new Vector3(0, 200, 0);
                transform.eulerAngles = new Vector3(90, 0, 0);
            }
        }
    }
}
