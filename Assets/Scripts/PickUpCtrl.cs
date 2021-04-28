using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpCtrl : NetObjBase
{
    [SyncVar(hook ="OnOwnerChanged")]
    public GameObject owner;
    // 处理拥有者改变同步
    public void OnOwnerChanged(GameObject val)
    {
        owner = val;
        this.GetComponent<MeshRenderer>().material.color = val.GetComponent<MeshRenderer>().material.color;
    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ChangeSpeed());
        
    }

    private IEnumerator ChangeSpeed()
    {
        int rot = Random.Range(-10, 45);
        transform.Rotate(Vector3.one * rot * Time.deltaTime);
        yield return new WaitForSeconds(Random.Range(1,3));
    }
}
