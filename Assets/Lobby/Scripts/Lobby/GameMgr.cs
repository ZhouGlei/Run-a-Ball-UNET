using Prototype.NetworkLobby;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour {


	public void Awake()
	{
		if (GameObject.Find("LobbyManager") == null)
		{
			Debug.LogWarning("未发现LobbyManager，重置回第0个场景");
			SceneManager.LoadScene(0);
			return;
		}
	}
}
