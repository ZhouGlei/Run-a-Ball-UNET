using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class MyLobbyHook : LobbyHook {
	public override void OnAllGamePlayerLoaded(Dictionary<LobbyPlayer, GamePlayer> _dicLobbyToGamePlayers)
	{
		foreach (var kv in _dicLobbyToGamePlayers)
		{
			var gamePlayer = kv.Value.GetComponent<GamePlayer>();
			gamePlayer.playerName = kv.Key.playerName;
			gamePlayer.playerColor = kv.Key.playerColor;

			gamePlayer.TargetAllGamePlayerLoaded(gamePlayer.connectionToClient);
		}
	}
}
