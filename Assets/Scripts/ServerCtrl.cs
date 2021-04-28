using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCtrl : NetObjBase {
    public float countdownTotal = 120f;
    public float currentCountdown;
    public bool isGameOver = false;
    // 所有玩家准备完毕
    protected override void OnAllGamePlayerLoaded()
    {
        
        // 如果是服务器端的话执行
        if (isServer)
        {
            Transform[] poses = transform.root.Find("Positions").GetComponentsInChildren<Transform>();
            List<Transform> posedList = new List<Transform>(poses);
            foreach (var kv in LobbyManager.s_Singleton._lobbyToGamePlayers)
            {
                GamePlayer gp = kv.Value;
                int rn = Random.Range(1, posedList.Count);
                while (!posedList[rn])
                {
                    rn = Random.Range(1, posedList.Count);
                }
                Vector3 pos = posedList[rn].position;
                gp.GetComponent<PlayerCtrl>().StartPos = pos;
                posedList.RemoveAt(rn);
            }

            currentCountdown = countdownTotal;
        }
    }

    protected override void OnUpdate()
    {
        //同步计时
        if (isServer)
        {
            if (isGameOver)
                return;
            currentCountdown -= Time.deltaTime;
            if (currentCountdown <= 0)
            {
                currentCountdown = 0;
                isGameOver = true;
            }
            foreach (var kv in LobbyManager.s_Singleton._lobbyToGamePlayers)
            {
                GamePlayer gp = kv.Value;
                gp.GetComponent<PlayerCtrl>().countdown = currentCountdown;
            }
            if (isGameOver)
            {
                int maxScore = -1;
                List<GameObject> winnners = new List<GameObject>();
                foreach (var kv in LobbyManager.s_Singleton._lobbyToGamePlayers)
                {
                    PlayerCtrl pc = kv.Value.GetComponent<PlayerCtrl>();
                    if(pc.score > maxScore)
                    {
                        maxScore = pc.score;
                        winnners.Clear();
                        winnners.Add(pc.gameObject);
                    }else if(pc.score == maxScore)
                    {
                        winnners.Add(pc.gameObject);
                    }
                }
                foreach (var kv in LobbyManager.s_Singleton._lobbyToGamePlayers)
                {
                    PlayerCtrl pc = kv.Value.GetComponent<PlayerCtrl>();
                    pc.TargetGameOver(pc.connectionToClient,winnners.ToArray());
                }
            }

        }
    }
}
