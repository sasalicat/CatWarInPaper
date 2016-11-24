using UnityEngine;
using System.Collections;

public class Manager_Photon : MonoBehaviour {
    public const int MAX_OBJ_NUM = 10;//所有陣列的數量上限
    public const float HEIGHT_LINE_LAYEL = 4;//畫線物件的位置的y值
    public GameObject[] objArray = new GameObject[MAX_OBJ_NUM];//場上單位的物件陣列
                                                               // Use this for initialization
    public BaseAction_AI_player[] actionArray = new BaseAction_AI_player[MAX_OBJ_NUM];

    void Start()
    {
        for (int i = 0; i < MAX_OBJ_NUM; i++)
        {
            if (objArray[i] != null)
            {
                actionArray[i] = objArray[i].GetComponent<BaseAction_AI_player>();
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
