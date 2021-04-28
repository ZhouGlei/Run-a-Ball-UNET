using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject Player; // 玩家

    public Vector3 offset; // 位置偏移
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        //offset = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Player.transform.position + offset;
    }
}
