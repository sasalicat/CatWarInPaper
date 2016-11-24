using UnityEngine;
using System.Collections.Generic;

public class Arrow_Net : MonoBehaviour {//僅用於發送轉向,需要配合arrow,自身轉向還是靠arrow腳本,其他人的本角色才是靠封包
    public newPhotonClient client;
    int roleNo;
    
    void Start()
    {
        roleNo = transform.Find("twoD").GetComponent<unit>().No;
    }
    // Update is called once per frame
    void Update () {
        var parameter = new Dictionary<byte, object>()
        {
            {(byte)1,roleNo },
            {(byte)2,transform.eulerAngles.x },
           {(byte)3,transform.eulerAngles.y }

        };
        client.peer.OpCustom(5,parameter,true);
    }
}
