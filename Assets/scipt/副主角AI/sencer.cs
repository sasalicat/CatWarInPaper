using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class sencer : MonoBehaviour {
    public GameObject[] list = new GameObject[10];
    public int nownum = 0;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            list[nownum] = other.gameObject;
            nownum++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            if (nownum == 1)
            {
                list[0] = null;
                nownum--;
            }
            else
            {
                for (int i = 0; i < nownum; i++)
                {
                    if (other.gameObject == list[i])
                    {
                        for (int j = i; j < nownum - 1; j++)
                        {
                            list[j] = list[j + 1];
                        }
                        list[nownum - 1] = null;
                        nownum--;
                    }
                }
            }
        }
    }
}
