using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Prototype.NetworkLobby
{
    // Subclass this and redefine the function you want
    // then add it to the lobby prefab
    public abstract class LobbyHook : MonoBehaviour
    {
        public virtual void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) { }

		public virtual void OnAllGamePlayerLoaded(Dictionary<LobbyPlayer, GamePlayer> _dicLobbyToGamePlayers) {}
	}
}
