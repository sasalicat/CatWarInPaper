using UnityEngine;
using System.Collections;

public class toolsManager : MonoBehaviour {
   const int RECAST_TIME = 10;
    const int MAX_UNIT_NUM = 10;
    const int MAX_TOOLS_NUM = 2;
    const float TOOLS_Y = 0.3f;
    const float RIGHT_UP_POINT_X = 9.05f;
    const float RIGHT_UP_POINT_Z = 5.16f;
    const float LEFT_DOWH_POINT_X = -9.39f;
    const float LEFT_DOEN_POINT_Z = -6.03f;

   public GameObject[] tools = new GameObject[MAX_TOOLS_NUM];
    public RecastRecord[] recordTime = new RecastRecord[MAX_TOOLS_NUM];
    public GameObject[] toolSimple = new GameObject[2];//道具樣本,用來創建新物件
    public UniAttributes[] enemyAttrArray = new UniAttributes[MAX_UNIT_NUM];
    public AI_attackRole[] AIarray = new AI_attackRole[MAX_UNIT_NUM];
    public int enemyNum=0;
    public unitManager unitManager;


	// Use this for initialization
	void Start () {
        unitManager = GetComponent<unitManager>();
	//初始化陣列---------------------------------------------------------
        for(int i = 0; i < MAX_TOOLS_NUM; i++)
        {
            recordTime[i] = null;
        }
        for(int i = 0; i < unitManager.MAX_OBJ_NUM; i++)
        {
            if (unitManager.unitArray[i].team == 2)
            {
                enemyAttrArray[enemyNum] = unitManager.objArray[i].GetComponent<UniAttributes>();
                AIarray[enemyNum++]= unitManager.objArray[i].GetComponent<AI_attackRole>();
            }
        }
        
	}

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < MAX_TOOLS_NUM; i++)
        {
            if (recordTime[i] == null)//如果為空
            {
                if (tools[i] == null)
                {
                    recordTime[i] = new RecastRecord(RECAST_TIME);//建立新的計時
                }
            }
            else if (recordTime[i].isover())//如果計時結束
            {
                float randomx;
                float randomz;
                do
                {
                    randomx = Random.Range(LEFT_DOWH_POINT_X, RIGHT_UP_POINT_X);
                    randomz = Random.Range(LEFT_DOEN_POINT_Z, RIGHT_UP_POINT_Z);
                } while (Physics.Raycast(transform.position, new Vector3(randomx, 0, randomz), 10, 2));//如果該位置有單位存在,重新選擇位置
                tools[i]=Instantiate(toolSimple[((int)Random.Range(0, 2))], new Vector3(randomx,TOOLS_Y,randomz), transform.rotation)as GameObject;//建立新的道具在隨機位置
                recordTime[i] = null;//銷毀計時器
                noticeAI(tools[i]);
            }
            else//如果計時未結束
            {
                recordTime[i].reduceTime(Time.deltaTime);
            }
        }
    }
     void noticeAI(GameObject tools)
    {
        //選出最適合拾取道具的角色
        int bestunit = -1;
        for(int i = 0; i < enemyNum; i++)
        {
            if (AIarray[i].tragettool==null) {
                float distance = (tools.transform.position - enemyAttrArray[i].transform.position).magnitude;
                if (bestunit == -1)
                {
                    if (enemyAttrArray[i].pickupRadius >= distance)
                    {
                        bestunit = i;
                    }
                }
                else if (enemyAttrArray[i].pickupRadius >= distance && distance < (tools.transform.position - enemyAttrArray[bestunit].transform.position).magnitude)
                {
                    bestunit = i;
                }
            }
        }
        //設置其道具目標
        if (bestunit!=-1)//如果有最適合的拾取者
        {
            AIarray[bestunit].tragettool = tools;
            AIarray[bestunit].GetComponent<retourer>().obstacle=null;//清空尋路障礙,以防物品消失後走回上次的尋路終點
        }
    }
}
