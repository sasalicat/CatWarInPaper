using UnityEngine;
using System.Collections;

public class firstAidTigger : MonoBehaviour
{
    public float persisTime;//持續時間
    public float tiggerTime;//每次效果間隔時間
    public int treatNum;//每次触发的治療量
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

                    persistTreat effect = Manger.objArray[i].AddComponent<persistTreat>();
                    effect.persisTime = persisTime;
                    effect.tiggerTime = tiggerTime;
                    effect.treatNum = treatNum;
                    effect.Caster = gameObject;
                    effect.fromtools = true;
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }
    
}