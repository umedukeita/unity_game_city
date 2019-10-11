using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class RoomContlor : MonoBehaviourPunCallbacks
{
	public Text title;
	public Text[] NOP;
	public GameObject[] RoomBuuton;
	public GameObject Loby, Room;

	private List<RoomInfo> roomInfoList = new List<RoomInfo>();
	// Use this for initialization
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
		
	}

	private void Update()
	{
		for (int i = 0; i < roomInfoList.Count; i++)
		{
			if (roomInfoList[i] != null)
			{
				NOP[i].text = string.Format("{0}/{1}", roomInfoList[i].PlayerCount, roomInfoList[i].MaxPlayers);
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
			case 4: PhotonNetwork.JoinOrCreateRoom("Room4", new RoomOptions() { MaxPlayers = 4, IsVisible = true, IsOpen = true }, null); break;
		}
	}

	public void RoomButton(string type)
	{
		switch (type) {
			case "Exit":PhotonNetwork.LeaveRoom();title.text = "Loby"; break;
		}
	}

	public override void OnJoinedRoom()
	{
		title.text = PhotonNetwork.CurrentRoom.Name;
		Room.SetActive(true);
		Loby.SetActive(false);
		Debug.Log("OnJoinedRoom");
	}

	public override void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom");
		Room.SetActive(false);
		Loby.SetActive(true);
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

}
