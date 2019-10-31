using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class RoomContlor : MonoBehaviourPunCallbacks
{
	public Text title,roomtext,countdown;
	public Text[] NOP;
	public GameObject[] RoomBuuton;
	public GameObject Loby, Room;
    public InputField inputField;
	private List<RoomInfo> roomInfoList = new List<RoomInfo>();
	private float count = 5, maching = 0;
	public bool countkey;
	PhotonView PhotonView;
	// Use this for initialization
	void Start()
	{
		Screen.SetResolution(1280, 720, false, 60);
		PhotonNetwork.ConnectUsingSettings();
		Room.SetActive(false);
		Loby.SetActive(true);
        inputField = inputField.GetComponent<InputField>();
		PhotonView = GetComponent<PhotonView>();
		countkey = false;
	}

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

	private void Update()
	{
		RoomInfoText();
		Maching();
		

	}
	void RoomInfoText()
	{
		for (int i = 0; i < roomInfoList.Count; i++)
		{
			//Debug.Log(roomInfoList[i].Name);
			if (roomInfoList[i].Name == "Room1")
			{
				NOP[0].text = string.Format("{0}/{1}", roomInfoList[i].PlayerCount, roomInfoList[i].MaxPlayers);
			}
			else if (roomInfoList[i].Name == "Room2")
			{
				NOP[1].text = string.Format("{0}/{1}", roomInfoList[i].PlayerCount, roomInfoList[i].MaxPlayers);
			}
			else if (roomInfoList[i].Name == "Room3")
			{
				NOP[2].text = string.Format("{0}/{1}", roomInfoList[i].PlayerCount, roomInfoList[i].MaxPlayers);
			}
			
		}
	}

	private void Maching()
	{
		RoomPlayerText();
		if (countkey)
		{
			
			count -= Time.deltaTime;
			countdown.text = "ゲーム開始まで残り" + (int)count + "秒";
			if (count <= 0)
			{
				PhotonView.RPC("ChangeScene", RpcTarget.All);
			}
		}
		else
		{
			count = 5;
			countdown.text = "マッチング中";
			maching += Time.deltaTime;
			if (maching > 1)
			{
				countdown.text += "・";
			}
			if (maching > 2)
			{
				countdown.text += "・";
			}
			if (maching > 3)
			{
				countdown.text += "・";
			}
			if (maching > 4)
			{
				maching = 0;
			}
		}
	}

	public void JoinRoomSelect(int number)
	{
		switch (number)
		{
			case 1: PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions() { MaxPlayers = 4, IsVisible = true ,IsOpen=true}, null); break;
			case 2: PhotonNetwork.JoinOrCreateRoom("Room2", new RoomOptions() { MaxPlayers = 4, IsVisible = true, IsOpen = true }, null); break;
			case 3: PhotonNetwork.JoinOrCreateRoom("Room3", new RoomOptions() { MaxPlayers = 4, IsVisible = true, IsOpen = true }, null); break;
            case 4: PhotonNetwork.JoinOrCreateRoom("Room4", new RoomOptions() { MaxPlayers = 4, IsVisible = true, IsOpen = true }, null); SceneManager.LoadScene("Main"); break;
        }
	}

	public void RoomButton(string type)
	{
		switch (type) {
			case "Exit":PhotonNetwork.LeaveRoom();title.text = "Lobby"; break;
		}
	}

	public override void OnJoinedRoom()
	{
		title.text = PhotonNetwork.CurrentRoom.Name;
		Room.SetActive(true);
		Loby.SetActive(false);
		string text;
		if (inputField.text == "")
		{
			text = "Player" + PhotonNetwork.LocalPlayer.ActorNumber;

		}
		else
		{
			text = inputField.text;
		}
        PhotonNetwork.NickName = text;
		Debug.Log("OnJoinedRoom");
		RoomPlayerText();

		if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
		{
			PhotonView.RPC("StartCountDown", RpcTarget.All);
		}
	}

	public override void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom");
		Room.SetActive(false);
		Loby.SetActive(true);
		
	}
	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		RoomPlayerText();
		
	}
	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{        
        RoomPlayerText();
	}

	private void RoomPlayerText()
	{
		roomtext.text = "";
		foreach (var player in PhotonNetwork.PlayerList)
		{
			if(player.NickName==PhotonNetwork.LocalPlayer.NickName)
				roomtext.text += player.NickName + " ←YOU\n";
			else
				roomtext.text += player.NickName + "\n";
		}
		
	}

	
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		Debug.Log("OnRoomListUpdate");

		// 既存の部屋リストをクリア
		if (roomInfoList != null)
		{
			roomInfoList.Clear();
		}

		// 新しいルームリストに更新
		roomInfoList = roomList;
		
	}

	[PunRPC]
	private void ChangeScene()
	{
		SceneManager.LoadScene("Main");
	}
	[PunRPC]
	private void StartCountDown() {
		countkey = true;
	}
}
