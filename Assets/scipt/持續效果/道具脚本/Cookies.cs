using UnityEngine;
using System.Collections;

public class Cookies : MonoBehaviour {
    public int recoverNum;
    public unitManager Manger;
    // Use this for initialization
    void Start()
    {
        Manger = GameObject.Find("ManagerOBJ").GetComponent<unitManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int team = other.transform.Find("twoD").GetComponent<unit>().team;//获得触发者的队伍编号
            for (int i = 0; i < Manger.nowObjNum; i++)
            {
                if (Manger.unitArray[i].team == team)
                {

                    Manger.unitArray[i].beenRecoverMP(10, gameObject, true);
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }
}
