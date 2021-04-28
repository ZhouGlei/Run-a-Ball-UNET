using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PickUpMgr : NetObjBase {

	public GameObject cubePrefab;
	public int radius = 20;

    protected override void OnAllGamePlayerLoaded()
    {
        if (isServer)
        {
            for (int i = 0; i < 360; i += 30)
            {
                Transform cubeTs = Instantiate(cubePrefab).transform;
                cubeTs.position = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * radius, 1, Mathf.Sin(i * Mathf.Deg2Rad) * radius);
                cubeTs.SetParent(this.transform, true);
                // 此句话有两个功能
                // 注册到服务器，服务器同步到客户端
                NetworkServer.Spawn(cubeTs.gameObject);
            }
        }
        
    }

}
