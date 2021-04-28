using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerCtrl : NetObjBase
{
    public int MoveForce = 50; // 移动力
    public int JumpForce = 50; // 跳跃力
    
    Rigidbody rd;
    Text scoreText;
    Text winText;
    Text timeText;
    // Start is called before the first frame update

    protected override void OnAllGamePlayerLoaded()
    {
        rd = transform.GetComponent<Rigidbody>();
        scoreText = transform.root.Find("Canvas/ScoreText").GetComponent<Text>();
        winText = transform.root.Find("Canvas/WinText").GetComponent<Text>();
        timeText = transform.root.Find("Canvas/TimeText").GetComponent<Text>();
        scoreText.text = $"得分：{score}";
    }

    // 服务器同步初始位置
    [SyncVar(hook ="OnSyncStartPos")]
    public Vector3 StartPos;
    // 处理同步消息
    public void OnSyncStartPos(Vector3 pos)
    {
        if (isLocalPlayer)
        {
            StartPos = pos;
            transform.position = pos;
            transform.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    // 服务器同步分数
    [SyncVar(hook = "OnScoreUpdated")]
    public  int score ;//得分
    // 处理得分同步消息
    public void OnScoreUpdated(int val)
    {
        if (isLocalPlayer)
        {
            score = val;
            scoreText.text = $"得分：{val}";
        }

    }

    // 服务器同步计时
    [SyncVar(hook ="OnCountdownChanged")]
    public float countdown;
    public void OnCountdownChanged(float val)
    {
        if(isLocalPlayer&& isAllGamePlayerLoaded)
        {
            countdown = val;
            timeText.text = $"Time: {countdown.ToString("f1")}";
        }

    }

    [TargetRpc]
    public void TargetGameOver(NetworkConnection networkConnection, GameObject[] winners)
    {
    
            string s = "获胜者：\n";
            for(int i = 0; i < winners.Length; i++)
            {
                s += winners[i].name + "\n";
            }
            winText.text = s;
            Time.timeScale = 0;
        
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("aaa");
        if(other.gameObject.tag == "PickUps")
        {
            AddScore(1);
            // 染色
            if (other.GetComponent<PickUpCtrl>().owner !=null)
            {
                other.GetComponent<PickUpCtrl>().owner.GetComponent<PlayerCtrl>().score -= 1;
            }
            other.GetComponent<PickUpCtrl>().owner = this.gameObject;
            //other.GetComponent<MeshRenderer>().material.color = transform.GetComponent<GamePlayer>().playerColor;
        }
    }

    private void AddScore(int val)
    {
        score += val;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 不是本机操控对象对返回
        if (!isLocalPlayer)
            return;
        if (Input.GetKey(KeyCode.W))
        {
            rd.AddForce(Vector3.forward* MoveForce*Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rd.AddForce(Vector3.back * MoveForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rd.AddForce(Vector3.left * MoveForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rd.AddForce(Vector3.right * MoveForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rd.AddForce(Vector3.up * JumpForce* Time.fixedDeltaTime,ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            rd.AddForce(Vector3.down * JumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }
}
