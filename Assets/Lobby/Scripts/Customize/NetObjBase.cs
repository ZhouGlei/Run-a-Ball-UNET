using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class NetObjBase : NetworkBehaviour {
	protected bool isAllGamePlayerLoaded = false;

	protected virtual void OnAllGamePlayerLoaded() { } // 所有玩家加载完毕，NetworkBehaviour自己Onstart可能存在不同步问题

	protected virtual void OnUpdate() { } // 保证所有玩家加载完毕后执行

	protected virtual void OnFixedUpdate() { }

	// Use this for initialization
	IEnumerator Start () {
		while (true)
		{
			if (GamePlayer.localPlayer
				&& GamePlayer.localPlayer.isAllGamePlayerLoaded)
			{
				OnAllGamePlayerLoaded();
				isAllGamePlayerLoaded = true;
				yield break;
			}

			yield return null;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (!isAllGamePlayerLoaded)
			return;

		OnUpdate();
	}

	void FixedUpdate()
	{
		if (!isAllGamePlayerLoaded)
			return;

		OnFixedUpdate();
	}
}
