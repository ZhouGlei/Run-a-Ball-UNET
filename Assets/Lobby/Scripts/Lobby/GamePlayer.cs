using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkIdentity))]
public class GamePlayer : NetworkBehaviour {
	public bool syncName = true;
	public bool syncColor = true;

	[SyncVar(hook = "OnSyncPlayerName")]
	public string playerName;

	[SyncVar(hook = "OnSyncPlayerColor")]
	public Color playerColor;

	public static GamePlayer localPlayer; // 由于是在Start的最后设置，所以可以确保此变量在NetObjBase.OnAllGamePlayerLoaded中可用

	public bool isAllGamePlayerLoaded = false;

	void OnSyncPlayerName(string val)
	{
		print(string.Format("OnSyncPlayerName({0})", val));

		if (syncName)
		{
			this.playerName = val;
			this.gameObject.name = val;
		}
	}

	void OnSyncPlayerColor(Color val)
	{
		print(string.Format("OnSyncPlayerColor({0})", val));

		if (syncColor)
		{
			this.playerColor = val;

			var renderer = this.GetComponent<Renderer>();
			if (renderer != null)
			{
				renderer.material.color = val;
			}
		}
	}

	void Start()
	{
		var root = GameObject.Find("GameRoot");
		if (!root)
		{
			Debug.LogError("场景必须有一个名为GameRoot的根节点");
			return;
		}
		this.transform.SetParent(root.transform, true);

		if (isLocalPlayer)
		{
			localPlayer = this;
			print(string.Format("本地玩家已设置 [PlayerControllerID={0}]", this.playerControllerId));
			CmdLocalPlayerLoaded(this.GetComponent<NetworkIdentity>());
		}
	}

	private void OnDestroy()
	{
		print(string.Format("玩家已销毁 [PlayerControllerID={0}]", this.playerControllerId));
		localPlayer = null;
	}


	[Command]
	void CmdLocalPlayerLoaded(NetworkIdentity netId)
	{
		LobbyManager.s_Singleton._clientNum++; // 每当有一个客户端准备好，就给客户端计数加一
		print("ClientNum:" + LobbyManager.s_Singleton._clientNum);

		if (LobbyManager.s_Singleton._clientNum == LobbyManager.s_Singleton._playerNumber)
		{
			LobbyManager.s_Singleton._lobbyHooks.OnAllGamePlayerLoaded(LobbyManager.s_Singleton._lobbyToGamePlayers);
		}
	}

	[TargetRpc]
	public void TargetAllGamePlayerLoaded(NetworkConnection conn)
	{
		print("TargetAllClientsReady");
		// 不能保证所有客户端对象都在GamePlayer对象之前被加载，因此广播消息可能无法送达在GamePlayer之后被加载的对象，因此增加一个标记，便于延迟加载的对象自行查询（必须继承自NetObjBase）
		// 注意：因为NetObjBase重写了Start/Update/FixedUpdate，所以如非必须，不要在派生类覆盖这几个方法
		isAllGamePlayerLoaded = true;
		//transform.root.BroadcastMessage("OnAllGamePlayerLoaded", SendMessageOptions.DontRequireReceiver);
	}
}
