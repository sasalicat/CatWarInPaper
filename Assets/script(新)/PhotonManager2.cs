using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

public class PhotonManager2 : MonoBehaviour
{
    public const int MAX_OBJ_NUM = 10;//所有陣列的數量上限
    public const float HEIGHT_LINE_LAYEL = 4;//畫線物件的位置的y值
    public GameObject[] objArray = new GameObject[MAX_OBJ_NUM];//場上單位的物件陣列
                                                               // Use this for initialization
    public Net_BaseAction[] actionArray = new Net_BaseAction[MAX_OBJ_NUM];
    public moveguide[] guideArray = new moveguide[MAX_OBJ_NUM];
    public unit []unitArray = new unit[MAX_OBJ_NUM];
    public Vector3[] CreatPosArray = new Vector3[4];
    public newPhotonClient client;

    // Use this for initialization
    void Start()
    {
        CreatPosArray[0] = new Vector3(-17.71f, -4.44f, 3.79f);
        CreatPosArray[1] = new Vector3(-5.4f, -4.44f, 3.79f);
        CreatPosArray[2] = new Vector3(-17.71f, -4.44f, -3.03f);
        CreatPosArray[3] = new Vector3(-5.4f, -4.44f, -3.03f);
        for (int i = 0; i < MAX_OBJ_NUM; i++)
        {
            if (objArray[i] != null)
            {
                actionArray[i] = objArray[i].GetComponent<Net_BaseAction>();
                guideArray[i] = objArray[i].GetComponent<moveguide>();
                unitArray[i] = objArray[i].transform.Find("twoD").GetComponent<unit>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddObj(GameObject newObj)
    {
        for(int i = 0; i < MAX_OBJ_NUM; i++)
        {
            if (objArray[i] == null)
            {
                objArray[i] = newObj;
                actionArray[i] = newObj.GetComponent<Net_BaseAction>();
                guideArray[i] = newObj.GetComponent<moveguide>();
                unitArray[i] = objArray[i].transform.Find("twoD").GetComponent<unit>();
                unitArray[i].No = i;
                return;
            }
        }
    }
}