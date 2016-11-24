using UnityEngine;
using System.Collections;

public class persistRecoverMP : MonoBehaviour {
    public float persisTime = 0;//持續時間
    public float tiggerTime;//每次效果間隔時間
    public int recoverNum;//每次触发的治療量
    public GameObject Caster;//此狀態的施放者
    public bool fromtools;//是否来自于道具

    float tiggerleft;
    float persisTimeAlready = 0;//效果已作用時間
    // Use this for initialization
    void Start()
    {
        Debug.Log("Start on call");
    }

    // Update is called once per frame
    void Update()
    {
        if (persisTime != 0)
        {
            if (tiggerleft <= 0)
            {

                transform.Find("twoD").GetComponent<unit>().beenRecoverMP(recoverNum, Caster,fromtools);
                tiggerleft = tiggerTime;

            }
            else
            {
                tiggerleft -= Time.deltaTime;
            }

            persisTimeAlready += Time.deltaTime;
            if (persisTimeAlready >= persisTime)//如果已作用時間超過持續時間
            {
                Destroy(this);//銷毀這個腳本
            }
        }
    }
}
