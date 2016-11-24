using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class unitManager : MonoBehaviour {
    public const int MAX_OBJ_NUM = 10;//所有陣列的數量上限
    public const float HEIGHT_LINE_LAYEL=4;//畫線物件的位置的y值
    public GameObject[] objArray=new GameObject[MAX_OBJ_NUM];//場上單位的物件陣列
    public unit[] unitArray = new unit[MAX_OBJ_NUM];//場上單位的單位腳本陣列
    public GameObject[] hpBarArray = new GameObject[MAX_OBJ_NUM];//血條的物件陣列
    public GameObject[] mpNumArray = new GameObject[MAX_OBJ_NUM];//mp值的物件陣列
    public AspectRatioFitter[] fitterArray = new AspectRatioFitter[MAX_OBJ_NUM];//血條的裁切陣列
    public GameObject[] redLineOBJ = new GameObject[MAX_OBJ_NUM];//紅線繪畫物件的陣列
    public GameObject[] blueLineOBJ = new GameObject[MAX_OBJ_NUM];//藍線繪畫物件的陣列
    public GameObject PathOfExiled;//手動賦予的放逐區物件
     int []ExiledArray=new int[MAX_OBJ_NUM];//流亡者排隊(誤),用來記錄被放逐的單位的No
    Vector3[] OriginArray = new Vector3[MAX_OBJ_NUM];//記錄被放逐時,被放逐單位的位置
     int ExiledNum = 0;//記錄被放逐的單位數量

    public GameObject canvas;//指向畫布
    public GameObject HPbar;//手動賦予的血條預設體
    public GameObject Mpnum;//手動賦予的能量數值預設體
    public GameObject linedrawer;//手動賦予的畫線物體
    public int nowObjNum=0;//記錄現在的物件數
    //public int obsNum = 0;

    public GameObject tragetpointQ;//用于Q的goto模式
    public GameObject tragetpointE;//用于E的goto模式
    // Use this for initialization
    void Start () {
        canvas = transform.Find("Canvas").gameObject;
        for (int i=0; i<MAX_OBJ_NUM; i++) {
            if (objArray[i]!=null) {
                Vector3 temp = objArray[i].transform.position;
                unitArray[i] = objArray[i].transform.Find("twoD").GetComponent("unit")as unit;
                unitArray[i].No = i;
                //初始化血條----------------------------------------------------------------------------------------------------------------
                GameObject newone = Instantiate(HPbar, new Vector3(temp.x, transform.position.y, temp.z), Quaternion.Euler(new Vector3(90, 0, 0))) as GameObject;
                newone.transform.parent = canvas.transform;
                fitterArray[i] = newone.GetComponent<AspectRatioFitter>();
                hpBarArray[i] = newone;
                //初始化能量值--------------------------------------------------------------------------------------------------------------
                GameObject newText = Instantiate(Mpnum, new Vector3(temp.x, transform.position.y, temp.z), Quaternion.Euler(new Vector3(90, 0, 0))) as GameObject;
                mpNumArray[i] = newText;
                newText.transform.parent = canvas.transform;
                //初始化線(紅藍)--------------------------------------------------------------------------------------------------------------
               GameObject newred = Instantiate(linedrawer, new Vector3(temp.x, HEIGHT_LINE_LAYEL, temp.z), Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
                redLineOBJ[i] = newred;

                GameObject newblue = Instantiate(linedrawer, new Vector3(temp.x, HEIGHT_LINE_LAYEL, temp.z), Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
                blueLineOBJ[i] = newblue;
                if (unitArray[i].team == 1 && !unitArray[i].captain)//單位為副主角
                {
                    redLineOBJ[i].GetComponent<LineRenderer>().SetColors(Color.red, Color.red);//設置線段顏色為紅色
                    redLineOBJ[i].GetComponent<LineRenderer>().SetWidth(0.1f, 0.1f);
                    blueLineOBJ[i].GetComponent<LineRenderer>().SetColors(Color.blue, Color.blue);//設置線段顏色為紅色
                    blueLineOBJ[i].GetComponent<LineRenderer>().SetWidth(0.1f, 0.1f);
                }
                else
                {
                    redLineOBJ[i].SetActive(false);//隱藏紅線物件
                    blueLineOBJ[i].SetActive(false);//隱藏藍線物件
                }
                nowObjNum++;
            }
        }
       
        //Exile(0);
        //Replace(0,new Vector3(0,0.5f,0));
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < nowObjNum; i++)
        {
            Vector3 temp = unitArray[i].transform.position;
            hpBarArray[i].transform.position = new Vector3(temp.x, transform.position.y, temp.z);
            fitterArray[i].aspectRatio = ((float)unitArray[i].nowHP / (float)unitArray[i].maxHP);
            mpNumArray[i].transform.position= new Vector3(temp.x+1.2f, transform.position.y, temp.z);
            ((Text)mpNumArray[i].GetComponent<Text>()).text = unitArray[i].nowMP+"";
            //連線控制-----------------------------------------------------------------------------------

            
            redLineOBJ[i].transform.position = new Vector3(temp.x, HEIGHT_LINE_LAYEL,temp.z);
            blueLineOBJ[i].transform.position = new Vector3(temp.x, HEIGHT_LINE_LAYEL, temp.z);
            //紅色指向攻擊目標------------------------------------------------------
            if (unitArray[i].team == 1 && !unitArray[i].captain)//單位為副主角
            {
                LineRenderer redLineDrawer = redLineOBJ[i].GetComponent<LineRenderer>();
                LineRenderer blueLineDrawer = blueLineOBJ[i].GetComponent<LineRenderer>();
                if (unitArray[i].commendMode == 1)
                {
                   
                    follow_AI_player mood1 = objArray[i].GetComponent<follow_AI_player>();
                    Vector3 tragetpos = mood1.traget.transform.position;
                    //redLineDrawer.SetColors(Color.red, Color.red);
                    redLineDrawer.SetPosition(0, redLineOBJ[i].transform.position);
                    redLineDrawer.SetPosition(1, new Vector3(tragetpos.x, HEIGHT_LINE_LAYEL, tragetpos.z));
                    //蓝色指向主角----------------------------------------------------------
                    blueLineDrawer.SetPosition(0, blueLineOBJ[i].transform.position);
                    blueLineDrawer.SetPosition(1, new Vector3(mood1.master.transform.position.x, HEIGHT_LINE_LAYEL, mood1.master.transform.position.z));


                }
                else if(unitArray[i].commendMode == 2)
                {
                    attack_AI_player mood2 = objArray[i].GetComponent<attack_AI_player>();
                    Vector3 tragetpos = mood2.traget.transform.position;
                    redLineDrawer.SetPosition(0, redLineOBJ[i].transform.position);
                    redLineDrawer.SetPosition(1, new Vector3(tragetpos.x, HEIGHT_LINE_LAYEL, tragetpos.z));
                    //蓝线指向目标-------------------------------------------------------------
                    blueLineDrawer.SetPosition(0, blueLineOBJ[i].transform.position);
                    blueLineDrawer.SetPosition(1, new Vector3(tragetpos.x, HEIGHT_LINE_LAYEL, tragetpos.z));
                }
                else if (unitArray[i].commendMode == 3)
                {
                    goTo_AI_player mood3 = objArray[i].GetComponent<goTo_AI_player>();
                    Vector3 tragetpos = mood3.attacktraget.transform.position;
                    Vector3 tragetpoint = mood3.tragetposition.transform.position;
                    redLineDrawer.SetPosition(0, redLineOBJ[i].transform.position);
                    redLineDrawer.SetPosition(1, new Vector3(tragetpos.x, HEIGHT_LINE_LAYEL, tragetpos.z));
                    //蓝线指向目的地-----------------------------------------------------------
                    blueLineDrawer.SetPosition(0, blueLineOBJ[i].transform.position);
                    blueLineDrawer.SetPosition(1, new Vector3(tragetpoint.x, HEIGHT_LINE_LAYEL, tragetpoint.z));
                }
                else if (unitArray[i].commendMode == 4)
                {
                    keepCloss_AI_player mood4 = objArray[i].GetComponent<keepCloss_AI_player>();
                    Vector3 tragetpos = mood4.attacktraget.transform.position;
                    Vector3 tragetobs = mood4.traget.transform.position;
                    redLineDrawer.SetPosition(0, redLineOBJ[i].transform.position);
                    redLineDrawer.SetPosition(1, new Vector3(tragetpos.x,HEIGHT_LINE_LAYEL, tragetpos.z));
                    //蓝线指向障碍-----------------------------------------------------------
                    blueLineDrawer.SetPosition(0, blueLineOBJ[i].transform.position);
                    blueLineDrawer.SetPosition(1, new Vector3(tragetobs.x, HEIGHT_LINE_LAYEL, tragetobs.z));
                }
            }
        }
       

        
    }
    public GameObject gettraget(int no)//由單位呼叫,回傳該單位最近的目標,no為呼叫的單位的編號
    {
        GameObject temptraget = null;
        //Debug.Log("no is " + no);
        //Debug.Log("team is " + unitArray[no].No);
        int team = unitArray[no].team;
        float mindistance = 0;
        for(int i = 0; i < nowObjNum; i++)
        {
            //Debug.Log("in gettraget i =" + i);
            if ( unitArray[i].team != team&& unitArray[i].alive&&!unitArray[i].exiled)//如果該單位和本單位不為同一隊,並且或者,未被放逐
            {
                if (temptraget == null || (objArray[i].transform.position-objArray[no].transform.position).magnitude<mindistance)//若當前沒有找到目標或現在的目標最短路徑長度比之前的目標短
                {
                    temptraget = objArray[i];
                    mindistance = (objArray[i].transform.position - objArray[no].transform.position).magnitude;
                }
            }
        }
        return temptraget;
    }
    public void Exile(int no)//放逐一個單位,它將不再顯示在攝像機裏(將顯示在鬼島...),no為被放逐的單位的編號
    {
        unit temp = unitArray[no];
        if (temp.canBeExiled && !(temp.exiled))
        {
            temp.exiled = true;
            ExiledArray[ExiledNum] = temp.No;//記錄被放逐物件的編號  
            OriginArray[ExiledNum] = objArray[no].transform.position;//記錄被放逐物件的最後位置
            ExiledNum++;
            objArray[no].transform.position = PathOfExiled.transform.position;//將被放逐的物件移動到鬼島..
            if (temp.captain)//如果被放逐的單位是主角
            {
                for (int i=0;i<nowObjNum;i++)//將所有該隊的master設為null,以防隊員在跟隨模式下跑到鬼島...
                {
                    if (unitArray[i].team == temp.team)
                    {
                        unitArray[i].master = null;
                    }
                }
            }
        }
    }
    public void Replace(int no)//釋放一個被放逐的單位在其原來的位置,no為復原單位的編號
    {
        for(int i = 0; i < ExiledNum; i++)
        { 
            if (ExiledArray[i]==no)
            {
                 unitArray[no].exiled = false;
               // Debug.Log("no:"+no);
                 objArray[no].transform.position = OriginArray[i];
                for(int j=i; j<ExiledNum; j++)
                {
                    ExiledArray[j] = ExiledArray[j + 1];
                    OriginArray[j] = OriginArray[j + 1];
                }
                ExiledNum--;
                if (unitArray[no].captain == true)
                {
                    Debug.Log("he is captain");
                    for (int k = 0; k < nowObjNum; k++)//復原從屬關係
                    {
                        Debug.Log("k:" + k);
                        if (unitArray[k].team == unitArray[no].team && no != k)
                        {
                            unitArray[k].master = objArray[no];
                        }
                    }
                }
                return;
            }

        }
    }
    public void Replace(int no,Vector3 place)//釋放一個被放逐的單位在指定的位置,no為復原單位的編號,place為指定的位置座標
    {
        for (int i = 0; i < ExiledNum; i++)
        {
            if (ExiledArray[i] == no)
            {
                unitArray[no].exiled = false;
                objArray[no].transform.position = place;
                for (int j = i; j < ExiledNum - 1; j++)
                {
                    ExiledArray[j] = ExiledArray[j + 1];
                    OriginArray[j] = OriginArray[j + 1];
                }
                ExiledNum--;
                if (unitArray[no].captain == true)
                {
                    Debug.Log("he is captain");
                    for (int k = 0; k < nowObjNum; k++)//復原從屬關係
                    {
                        Debug.Log("k:" + k);
                        if (unitArray[k].team == unitArray[no].team && no != k)
                        {
                            unitArray[k].master = objArray[no];
                        }
                    }
                }
                return;
            }
        }
    }
    public void detourRequest(GameObject obstacle, GameObject requester)//繞過請求,用於AI繞過一個障礙,obstacle為要繞過的障礙物件,request為請求者物件
    {
        //Debug.Log("obstacle is"+obstacle);
        retourer record = requester.GetComponent<retourer>();//获取请求者的绕路记录
        float diameter = requester.transform.localScale.x;//获取请求者直径
        sencer sencer = obstacle.transform.Find("area").GetComponent<sencer>();
        GameObject traget = record.traget;
        Vector3 Tangent1;
        Vector3 Tangent2;
        GameObject lastobstacle = record.obstacle;//记录上一次寻路的障碍
        Vector3 lastforward = record.forward;//记录上一次寻路的方向
        int lastcast = record.caseMark;

        Vector3[] possibleEnd = new Vector3[4];
        int possibleNum = 0;
        record.obstacle = obstacle;
        record.origin = requester.transform.position;
        //record.caseMark = 0;
        

        int obsNum = 0;
        for (int i = 0; i < sencer.nownum; i++)
        {
            if (sencer.list[i] != requester && sencer.list[i] != record.traget)
            {
                obsNum++;
            }
        }
        //Debug.Log("obs num is" + obsNum);
        if (obsNum == 0) {
            if(Vector3.Angle(requester.transform.position - obstacle.transform.position, record.traget.transform.position - obstacle.transform.position) > 90)//感测区内只有一个物件,没有错误的话必定是请求者
            {//如果由碰撞触发请求者障碍物的向量和障碍物目标的向量差角可能小於於90度,那时就不用绕过障碍了
                Vector3 origonToEnd = traget.transform.position - requester.transform.position;
                Vector3 requesterpos = requester.transform.position;
                //Debug.Log("before getCenter obstacle is"+obstacle.name+"position is"+obstacle.transform.position);
                getTangentCenter(obstacle.transform.position, 0.5f * obstacle.transform.localScale.x, requesterpos, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);
                float Angle1 = Vector3.Angle(origonToEnd, Tangent1);
                float Angle2 = Vector3.Angle(origonToEnd, Tangent2);
                //Debug.Log("one:" +Angle1+",two"+Angle2);
                if (Angle1 < Angle2)
                {
                    //Debug.Log("Angle1 is little");
                    Vector3 forward = Tangent1;
                    forward.Normalize();//轉化為單位向量
                    if (overrideForward(obstacle, lastobstacle, forward, lastforward.normalized))
                    {
                        record.forward = forward;
                        record.endPoint = requesterpos + Tangent1; //forward * (origonToEnd.magnitude*Mathf.Cos((Angle1/180)*Mathf.PI));
                                                                   // Debug.Log("mati of origin to end ia" + (record.endPoint - record.origin).magnitude);
                    }
                    record.caseMark = 1;
                }
                else
                {
                    //Debug.Log("Angle2 is little");
                    Vector3 forward = Tangent2;
                    forward.Normalize();//轉化為單位向量
                    if (overrideForward(obstacle, lastobstacle, forward, lastforward.normalized))
                    {
                        record.forward = forward;
                        // Debug.Log("forward ia" + forward);
                        record.endPoint = requesterpos + Tangent2;//forward * (origonToEnd.magnitude * Mathf.Cos((Angle2 / 180) * Mathf.PI));
                                                                  // Debug.Log("mati of origin to end ia" + (record.endPoint - record.origin).magnitude);
                    }
                    record.caseMark = 2;
                }

            }
            else
            {
                record.caseMark = -1;
                record.obstacle = null;
            }
        }
        else if (obsNum == 1)
        {

            GameObject obstacle2 = null;
            for (int i = 0; i < sencer.nownum; i++)
            {
                if (sencer.list[i] != requester)
                {
                    obstacle2 = sencer.list[i];
                }
            }
            Vector3 origonToEnd = traget.transform.position - requester.transform.position;
            Vector3 requesterpos = requester.transform.position;
            if (((obstacle.transform.position - obstacle2.transform.position).magnitude - 0.5 * obstacle.transform.localScale.x - 0.5 * obstacle2.transform.localScale.x < requester.transform.localScale.x) || Vector3.Angle(requesterpos - obstacle.transform.position, obstacle2.transform.position - obstacle.transform.position) > 90) {

                //Debug.Log("before getCenter obstacle is"+obstacle.name+"position is"+obstacle.transform.position);
                getTangentCenter(obstacle.transform.position, 0.5f * obstacle.transform.localScale.x, requesterpos, requester.transform.localScale.x, out Tangent1, out Tangent2);
                // Debug.Log("obs2 is" + obstacle2 + "Tangent1 is" + Tangent1 + "Tangent2 is" + Tangent2);
                //Debug.Log("Angle 1 is" + Vector3.Angle(Tangent1, obstacle2.transform.position - requesterpos) + " Angle 2 is" + Vector3.Angle(Tangent2, obstacle2.transform.position - requesterpos));
                if (Vector3.Angle(Tangent1, obstacle2.transform.position - requesterpos) > Vector3.Angle(Tangent2, obstacle2.transform.position - requesterpos))//找到和障礙相反的方向
                {
                    //   Debug.Log("Angle1 is larger");
                    if (overrideForward(obstacle, lastobstacle, Tangent1.normalized, lastforward.normalized))
                    {
                        record.forward = Tangent1.normalized;
                        record.endPoint = requesterpos + Tangent1;
                    }
                    record.caseMark = 3;
                }
                else
                {
                    if (overrideForward(obstacle, lastobstacle, Tangent2.normalized, lastforward.normalized))
                    {
                        // Debug.Log("Angle2 is larger");
                        record.forward = Tangent2.normalized;
                        record.endPoint = requesterpos + Tangent2;
                    }
                    record.caseMark = 4;
                }
            }
            else
            {
                if (overrideForward(obstacle, lastobstacle, (((obstacle2.transform.position - obstacle.transform.position) * 0.5f + obstacle.transform.position)-requester.transform.position).normalized, lastforward.normalized))
                {
                    record.endPoint = (obstacle2.transform.position - obstacle.transform.position) * 0.5f + obstacle.transform.position;
                    record.forward = record.endPoint - requester.transform.position;
                }
                record.caseMark = 5;
            }
        }
        else
        {
            Vector3 obsToRequest = requester.transform.position - obstacle.transform.position;
            GameObject leftleast = null;
            GameObject rightleast = null;
           // Debug.Log("enter muit");
            for (int i = 0; i < sencer.nownum; i++)
            {
                // Debug.Log("i="+i+"sence is"+sencer+"parent is"+sencer.transform.parent+"list is"+sencer.list[i]);
                if (sencer.list[i] != requester && sencer.list[i] != record.traget)
                {
                    Vector2 A = new Vector2(requester.transform.position.x, requester.transform.position.z);
                    Vector2 B = new Vector2(obstacle.transform.position.x, obstacle.transform.position.z);
                    Vector2 C = new Vector2(sencer.list[i].transform.position.x, sencer.list[i].transform.position.z);
                    // Debug.Log("getS is" + getS(A, B, C));
                    if (getS(A, B, C) < 0)
                    {

                        if (rightleast == null || Vector3.Angle(sencer.list[i].transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position) < Vector3.Angle(rightleast.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position))
                        {
                            rightleast = sencer.list[i];
                        }
                    }
                    else
                    {
                        if (leftleast == null || Vector3.Angle(sencer.list[i].transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position) < Vector3.Angle(leftleast.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position))
                        {
                            leftleast = sencer.list[i];
                        }
                    }
                }
                // Debug.Log("leftleast is"+leftleast+"rightleast is"+rightleast);
            }

            if (rightleast == null)
            {
               // Debug.Log("right least is null");
                if (getS(requester.transform.position, obstacle.transform.position, record.traget.transform.position) < 0 && Vector3.Angle(record.traget.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position) <= 90)
                {//目标在障碍物和请求者连线的右边且,障碍物与请求者与障碍物与目标之间的夹角小于等于90度
                   // Debug.Log("in rightless is null Angle is" + Vector3.Angle(record.traget.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position));
                    record.obstacle = null;
                }
                else
                {
                    getTangentCenter(obstacle.transform.position, 0.5f * obstacle.transform.localScale.x, requester.transform.position, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);
                    //  Debug.Log("in right least Tanglent1 point is" + (Tangent1+requester.transform.position) + "Tanglent2 is" + (Tangent2+requester.transform.position));
                   // Debug.Log("in right least requester is" + requester + "leftless is" + leftleast + "obsNum is" + obsNum);
                    if (Vector3.Angle(leftleast.transform.position - requester.transform.position, Tangent1) > Vector3.Angle(leftleast.transform.position - requester.transform.position, Tangent2))//Tangent1 的切點位於右邊
                    {
                        if (overrideForward(obstacle, lastobstacle, Tangent1.normalized, lastforward))
                        {
                            record.endPoint = requester.transform.position + Tangent1;
                            record.forward = Tangent1;//選擇右邊的方向
                        }
                        record.caseMark = 6;
                    }
                    else
                    {
                        if (overrideForward(obstacle, lastobstacle, Tangent2.normalized, lastforward))
                        {
                            record.endPoint = requester.transform.position + Tangent2;
                            record.forward = Tangent2;
                        }
                        record.caseMark = 7;

                    }
                }
            }
            else if (leftleast == null)
            {
                //Debug.Log("left least is null obs is"+obstacle+"getS is"+ getS(requester.transform.position, obstacle.transform.position, record.traget.transform.position)+"Angle is"+ Vector3.Angle(record.traget.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position));
                if (getS(requester.transform.position, obstacle.transform.position, record.traget.transform.position) > 0 && Vector3.Angle(record.traget.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position) <= 90)
                {
                    //Debug.Log("in less is null Angle is" + Vector3.Angle(record.traget.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position));
                    record.obstacle = null;
                }
                else
                {
                    getTangentCenter(obstacle.transform.position, 0.5f * obstacle.transform.localScale.x, requester.transform.position, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);
                    //  Debug.Log("in left least Tanglent1 is" + Tangent1 + "Tanglent2 is" + Tangent2);
                    if (Vector3.Angle(rightleast.transform.position - requester.transform.position, Tangent1) > Vector3.Angle(rightleast.transform.position - requester.transform.position, Tangent2))//Tangent1 的切點位於右邊
                    {
                        if (overrideForward(obstacle, lastobstacle, Tangent1.normalized, lastforward))
                        {
                            //Debug.Log("choose Tanglent2");
                            record.endPoint = requester.transform.position + Tangent1;
                            record.forward = Tangent1;//選擇左邊的方向
                        }
                        record.caseMark = 8;
                    }
                    else
                    {
                        if (overrideForward(obstacle, lastobstacle, Tangent2.normalized, lastforward))
                        {
                            //Debug.Log("choose Tanglent1");
                            record.endPoint = requester.transform.position + Tangent2;
                            record.forward = Tangent2;
                        }
                        record.caseMark = 9;
                    }
                }
            }
            else
            {
                if (isUseful(requester, record.traget, obstacle, leftleast))
                {
                    float leftans1 = (leftleast.transform.position - obstacle.transform.position).magnitude - leftleast.transform.localScale.x * 0.5f - obstacle.transform.localScale.x * 0.5f;
                    float leftans2 = requester.transform.localScale.x;
                    // Debug.Log("leftleast.transform.position - obstacle.transform.position).magnitude-leftleast.transform.localScale.x*0.5f-obstacle.transform.localScale.x*0.5f is"+leftans1 +" requester.transform.localScale.x is"+leftans2);
                    getTangentCenter(leftleast.transform.position, 0.5f * leftleast.transform.localScale.x, requester.transform.position, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);

                    if (Vector3.Angle(Tangent1, obstacle.transform.position - requester.transform.position) > Vector3.Angle(Tangent2, obstacle.transform.position - requester.transform.position))
                    {
                        //    Debug.Log("In 1 if ");
                        possibleEnd[possibleNum++] = Tangent1;

                    }
                    else
                    {
                        //    Debug.Log("in 1 else");
                        possibleEnd[possibleNum++] = Tangent2;

                    }
                }
                if (isUseful(requester, record.traget, obstacle, rightleast))
                {
                    float rightans1 = (rightleast.transform.position - obstacle.transform.position).magnitude - rightleast.transform.localScale.x * 0.5f - obstacle.transform.localScale.x * 0.5f;
                    float rightans2 = requester.transform.localScale.x;
                    // Debug.Log("rightleast.transform.position - obstacle.transform.position).magnitude-rightleast.transform.localScale.x*0.5f-obstacle.transform.localScale.x*0.5f is" + rightans1 + " requester.transform.localScale.x is" + rightans2);
                    getTangentCenter(rightleast.transform.position, 0.5f * rightleast.transform.localScale.x, requester.transform.position, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);
                    if (Vector3.Angle(Tangent1, obstacle.transform.position - requester.transform.position) > Vector3.Angle(Tangent2, obstacle.transform.position - requester.transform.position))
                    {
                        possibleEnd[possibleNum++] = Tangent1;

                    }
                    else
                    {
                        possibleEnd[possibleNum++] = Tangent2;
                    }
                }
                if (isUseful(requester, record.traget, obstacle, leftleast) && (leftleast.transform.position - obstacle.transform.position).magnitude - leftleast.transform.localScale.x * 0.5f - obstacle.transform.localScale.x * 0.5f >= requester.transform.localScale.x)
                {
                    possibleEnd[possibleNum++] = (obstacle.transform.position + 0.5f * (leftleast.transform.position - obstacle.transform.position)) - requester.transform.position;

                }
                if (isUseful(requester, record.traget, obstacle, rightleast) && (rightleast.transform.position - obstacle.transform.position).magnitude - rightleast.transform.localScale.x * 0.5f - obstacle.transform.localScale.x * 0.5f >= requester.transform.localScale.x)
                {
                    possibleEnd[possibleNum++] = obstacle.transform.position + 0.5f * (rightleast.transform.position - obstacle.transform.position) - requester.transform.position;

                }
                //Debug.Log("possable num is"+possibleNum);
                for (int i = 0; i < possibleNum; i++) {
                    //Debug.Log("possible num ="+i+" magnitude is " + possibleEnd[i].magnitude);
                }
                bubblesort(ref possibleEnd, possibleNum, requester.transform.position);
                // Debug.Log("after sort");
                for (int i = 0; i < possibleNum; i++)
                {
                     //Debug.Log("possible num =" + i + " magnitude is " + possibleEnd[i].magnitude);
                }
                if (possibleNum != 0)
                {
                    if (overrideForward(obstacle, lastobstacle, possibleEnd[0].normalized, lastforward))
                    {
                        //Debug.Log("In marker 10 forward is" + possibleEnd[0]);
                        record.endPoint = possibleEnd[0] + requester.transform.position;
                        record.forward = possibleEnd[0];
                    }
                    record.caseMark = 10;
                }
                else
                {//做obsnum為0的事項
                    if (Vector3.Angle(requester.transform.position - obstacle.transform.position, record.traget.transform.position - obstacle.transform.position) > 90)//感测区内只有一个物件,没有错误的话必定是请求者
                    {//如果由碰撞触发请求者障碍物的向量和障碍物目标的向量差角可能小於於90度,那时就不用绕过障碍了
                        Vector3 origonToEnd = traget.transform.position - requester.transform.position;
                        Vector3 requesterpos = requester.transform.position;
                        //Debug.Log("before getCenter obstacle is"+obstacle.name+"position is"+obstacle.transform.position);
                        getTangentCenter(obstacle.transform.position, 0.5f * obstacle.transform.localScale.x, requesterpos, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);
                        float Angle1 = Vector3.Angle(origonToEnd, Tangent1);
                        float Angle2 = Vector3.Angle(origonToEnd, Tangent2);
                        //Debug.Log("one:" +Angle1+",two"+Angle2);
                        if (Angle1 < Angle2)
                        {
                            //Debug.Log("Angle1 is little");
                            Vector3 forward = Tangent1;
                            forward.Normalize();//轉化為單位向量
                            if (overrideForward(obstacle, lastobstacle, forward.normalized, lastforward))
                            {
                                record.forward = forward;
                                record.endPoint = requesterpos + Tangent1; //forward * (origonToEnd.magnitude*Mathf.Cos((Angle1/180)*Mathf.PI));
                            }
                            // Debug.Log("mati of origin to end ia" + (record.endPoint - record.origin).magnitude);
                            record.caseMark = 11;
                        }
                        else
                        {
                            //Debug.Log("Angle2 is little");
                            Vector3 forward = Tangent2;
                            forward.Normalize();//轉化為單位向量
                            if (overrideForward(obstacle, lastobstacle, forward.normalized, lastforward))
                            {
                                record.forward = forward;
                                // Debug.Log("forward ia" + forward);
                                record.endPoint = requesterpos + Tangent2;//forward * (origonToEnd.magnitude * Mathf.Cos((Angle2 / 180) * Mathf.PI));
                            }                                         // Debug.Log("mati of origin to end ia" + (record.endPoint - record.origin).magnitude);
                            record.caseMark = 12;
                        }

                    }
                    else
                    {
                        record.obstacle= null;
                        record.caseMark = -2;
                    }
                }
            }

        }

    }
    public void detourRequest(GameObject obstacle, GameObject requester, Vector3 tragetpos)//繞過請求遍體,用於AI繞過一個障礙且目標是一個座標而不是物件,obstacle為要繞過的障礙物件,request為請求者物件
    {
        //Debug.Log("obstacle is"+obstacle);
        retourer record = requester.GetComponent<retourer>();//获取请求者的绕路记录
        float diameter = requester.transform.localScale.x;//获取请求者直径
        sencer sencer = obstacle.transform.Find("area").GetComponent<sencer>();
        //GameObject traget = record.traget;
        Vector3 Tangent1;
        Vector3 Tangent2;
        GameObject lastobstacle = record.obstacle;//记录上一次寻路的障碍
        Vector3 lastforward = record.forward;//记录上一次寻路的方向
        int lastcast = record.caseMark;

        Vector3[] possibleEnd = new Vector3[4];
        int possibleNum = 0;
        record.obstacle = obstacle;
        record.origin = requester.transform.position;
        //record.caseMark = 0;


        int obsNum = 0;
        for (int i = 0; i < sencer.nownum; i++)
        {
            if (sencer.list[i] != requester )
            {
                obsNum++;
            }
        }
        //Debug.Log("obs num is" + obsNum);
        if (obsNum == 0)
        {
            if (Vector3.Angle(requester.transform.position - obstacle.transform.position, tragetpos - obstacle.transform.position) > 90)//感测区内只有一个物件,没有错误的话必定是请求者
            {//如果由碰撞触发请求者障碍物的向量和障碍物目标的向量差角可能小於於90度,那时就不用绕过障碍了
                Vector3 origonToEnd =tragetpos- requester.transform.position;
                Vector3 requesterpos = requester.transform.position;
                //Debug.Log("before getCenter obstacle is"+obstacle.name+"position is"+obstacle.transform.position);
                getTangentCenter(obstacle.transform.position, 0.5f * obstacle.transform.localScale.x, requesterpos, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);
                float Angle1 = Vector3.Angle(origonToEnd, Tangent1);
                float Angle2 = Vector3.Angle(origonToEnd, Tangent2);
                //Debug.Log("one:" +Angle1+",two"+Angle2);
                if (Angle1 < Angle2)
                {
                    //Debug.Log("Angle1 is little");
                    Vector3 forward = Tangent1;
                    forward.Normalize();//轉化為單位向量
                    if (overrideForward(obstacle, lastobstacle, forward, lastforward.normalized))
                    {
                        record.forward = forward;
                        record.endPoint = requesterpos + Tangent1; //forward * (origonToEnd.magnitude*Mathf.Cos((Angle1/180)*Mathf.PI));
                                                                   // Debug.Log("mati of origin to end ia" + (record.endPoint - record.origin).magnitude);
                    }
                    record.caseMark = 1;
                }
                else
                {
                    //Debug.Log("Angle2 is little");
                    Vector3 forward = Tangent2;
                    forward.Normalize();//轉化為單位向量
                    if (overrideForward(obstacle, lastobstacle, forward, lastforward.normalized))
                    {
                        record.forward = forward;
                        // Debug.Log("forward ia" + forward);
                        record.endPoint = requesterpos + Tangent2;//forward * (origonToEnd.magnitude * Mathf.Cos((Angle2 / 180) * Mathf.PI));
                                                                  // Debug.Log("mati of origin to end ia" + (record.endPoint - record.origin).magnitude);
                    }
                    record.caseMark = 2;
                }

            }
            else
            {
                record.caseMark = -1;
                record.obstacle = null;
            }
        }
        else if (obsNum == 1)
        {

            GameObject obstacle2 = null;
            for (int i = 0; i < sencer.nownum; i++)
            {
                if (sencer.list[i] != requester)
                {
                    obstacle2 = sencer.list[i];
                }
            }
            Vector3 origonToEnd =tragetpos - requester.transform.position;
            Vector3 requesterpos = requester.transform.position;
            if (((obstacle.transform.position - obstacle2.transform.position).magnitude - 0.5 * obstacle.transform.localScale.x - 0.5 * obstacle2.transform.localScale.x < requester.transform.localScale.x) || Vector3.Angle(requesterpos - obstacle.transform.position, obstacle2.transform.position - obstacle.transform.position) > 90)
            {

                //Debug.Log("before getCenter obstacle is"+obstacle.name+"position is"+obstacle.transform.position);
                getTangentCenter(obstacle.transform.position, 0.5f * obstacle.transform.localScale.x, requesterpos, requester.transform.localScale.x, out Tangent1, out Tangent2);
                // Debug.Log("obs2 is" + obstacle2 + "Tangent1 is" + Tangent1 + "Tangent2 is" + Tangent2);
                //Debug.Log("Angle 1 is" + Vector3.Angle(Tangent1, obstacle2.transform.position - requesterpos) + " Angle 2 is" + Vector3.Angle(Tangent2, obstacle2.transform.position - requesterpos));
                if (Vector3.Angle(Tangent1, obstacle2.transform.position - requesterpos) > Vector3.Angle(Tangent2, obstacle2.transform.position - requesterpos))//找到和障礙相反的方向
                {
                    //   Debug.Log("Angle1 is larger");
                    if (overrideForward(obstacle, lastobstacle, Tangent1.normalized, lastforward.normalized))
                    {
                        record.forward = Tangent1.normalized;
                        record.endPoint = requesterpos + Tangent1;
                    }
                    record.caseMark = 3;
                }
                else
                {
                    if (overrideForward(obstacle, lastobstacle, Tangent2.normalized, lastforward.normalized))
                    {
                        // Debug.Log("Angle2 is larger");
                        record.forward = Tangent2.normalized;
                        record.endPoint = requesterpos + Tangent2;
                    }
                    record.caseMark = 4;
                }
            }
            else
            {
                if (overrideForward(obstacle, lastobstacle, (((obstacle2.transform.position - obstacle.transform.position) * 0.5f + obstacle.transform.position) - requester.transform.position).normalized, lastforward.normalized))
                {
                    record.endPoint = (obstacle2.transform.position - obstacle.transform.position) * 0.5f + obstacle.transform.position;
                    record.forward = record.endPoint - requester.transform.position;
                }
                record.caseMark = 5;
            }
        }
        else
        {
            Vector3 obsToRequest = requester.transform.position - obstacle.transform.position;
            GameObject leftleast = null;
            GameObject rightleast = null;
            // Debug.Log("enter muit");
            for (int i = 0; i < sencer.nownum; i++)
            {
                // Debug.Log("i="+i+"sence is"+sencer+"parent is"+sencer.transform.parent+"list is"+sencer.list[i]);
                if (sencer.list[i] != requester)
                {
                    Vector2 A = new Vector2(requester.transform.position.x, requester.transform.position.z);
                    Vector2 B = new Vector2(obstacle.transform.position.x, obstacle.transform.position.z);
                    Vector2 C = new Vector2(sencer.list[i].transform.position.x, sencer.list[i].transform.position.z);
                    // Debug.Log("getS is" + getS(A, B, C));
                    if (getS(A, B, C) < 0)
                    {

                        if (rightleast == null || Vector3.Angle(sencer.list[i].transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position) < Vector3.Angle(rightleast.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position))
                        {
                            rightleast = sencer.list[i];
                        }
                    }
                    else
                    {
                        if (leftleast == null || Vector3.Angle(sencer.list[i].transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position) < Vector3.Angle(leftleast.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position))
                        {
                            leftleast = sencer.list[i];
                        }
                    }
                }
                // Debug.Log("leftleast is"+leftleast+"rightleast is"+rightleast);
            }

            if (rightleast == null)
            {
                // Debug.Log("right least is null");
                if (getS(requester.transform.position, obstacle.transform.position, tragetpos) < 0 && Vector3.Angle(tragetpos - obstacle.transform.position, requester.transform.position - obstacle.transform.position) <= 90)
                {//目标在障碍物和请求者连线的右边且,障碍物与请求者与障碍物与目标之间的夹角小于等于90度
                 // Debug.Log("in rightless is null Angle is" + Vector3.Angle(record.traget.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position));
                    record.obstacle = null;
                }
                else
                {
                    getTangentCenter(obstacle.transform.position, 0.5f * obstacle.transform.localScale.x, requester.transform.position, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);
                    //  Debug.Log("in right least Tanglent1 point is" + (Tangent1+requester.transform.position) + "Tanglent2 is" + (Tangent2+requester.transform.position));
                    // Debug.Log("in right least requester is" + requester + "leftless is" + leftleast + "obsNum is" + obsNum);
                    if (Vector3.Angle(leftleast.transform.position - requester.transform.position, Tangent1) > Vector3.Angle(leftleast.transform.position - requester.transform.position, Tangent2))//Tangent1 的切點位於右邊
                    {
                        if (overrideForward(obstacle, lastobstacle, Tangent1.normalized, lastforward))
                        {
                            record.endPoint = requester.transform.position + Tangent1;
                            record.forward = Tangent1;//選擇右邊的方向
                        }
                        record.caseMark = 6;
                    }
                    else
                    {
                        if (overrideForward(obstacle, lastobstacle, Tangent2.normalized, lastforward))
                        {
                            record.endPoint = requester.transform.position + Tangent2;
                            record.forward = Tangent2;
                        }
                        record.caseMark = 7;

                    }
                }
            }
            else if (leftleast == null)
            {
                //Debug.Log("left least is null obs is"+obstacle+"getS is"+ getS(requester.transform.position, obstacle.transform.position, record.traget.transform.position)+"Angle is"+ Vector3.Angle(record.traget.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position));
                if (getS(requester.transform.position, obstacle.transform.position, tragetpos) > 0 && Vector3.Angle(tragetpos - obstacle.transform.position, requester.transform.position - obstacle.transform.position) <= 90)
                {
                    //Debug.Log("in less is null Angle is" + Vector3.Angle(record.traget.transform.position - obstacle.transform.position, requester.transform.position - obstacle.transform.position));
                    record.obstacle = null;
                }
                else
                {
                    getTangentCenter(obstacle.transform.position, 0.5f * obstacle.transform.localScale.x, requester.transform.position, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);
                    //  Debug.Log("in left least Tanglent1 is" + Tangent1 + "Tanglent2 is" + Tangent2);
                    if (Vector3.Angle(rightleast.transform.position - requester.transform.position, Tangent1) > Vector3.Angle(rightleast.transform.position - requester.transform.position, Tangent2))//Tangent1 的切點位於右邊
                    {
                        if (overrideForward(obstacle, lastobstacle, Tangent1.normalized, lastforward))
                        {
                            //Debug.Log("choose Tanglent2");
                            record.endPoint = requester.transform.position + Tangent1;
                            record.forward = Tangent1;//選擇左邊的方向
                        }
                        record.caseMark = 8;
                    }
                    else
                    {
                        if (overrideForward(obstacle, lastobstacle, Tangent2.normalized, lastforward))
                        {
                            //Debug.Log("choose Tanglent1");
                            record.endPoint = requester.transform.position + Tangent2;
                            record.forward = Tangent2;
                        }
                        record.caseMark = 9;
                    }
                }
            }
            else
            {
                if (isUseful(requester.transform.position, tragetpos, obstacle.transform.position, leftleast.transform.position))
                {
                    float leftans1 = (leftleast.transform.position - obstacle.transform.position).magnitude - leftleast.transform.localScale.x * 0.5f - obstacle.transform.localScale.x * 0.5f;
                    float leftans2 = requester.transform.localScale.x;
                    // Debug.Log("leftleast.transform.position - obstacle.transform.position).magnitude-leftleast.transform.localScale.x*0.5f-obstacle.transform.localScale.x*0.5f is"+leftans1 +" requester.transform.localScale.x is"+leftans2);
                    getTangentCenter(leftleast.transform.position, 0.5f * leftleast.transform.localScale.x, requester.transform.position, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);

                    if (Vector3.Angle(Tangent1, obstacle.transform.position - requester.transform.position) > Vector3.Angle(Tangent2, obstacle.transform.position - requester.transform.position))
                    {
                        //    Debug.Log("In 1 if ");
                        possibleEnd[possibleNum++] = Tangent1;

                    }
                    else
                    {
                        //    Debug.Log("in 1 else");
                        possibleEnd[possibleNum++] = Tangent2;

                    }
                }
                if (isUseful(requester.transform.position, tragetpos, obstacle.transform.position, rightleast.transform.position))
                {
                    float rightans1 = (rightleast.transform.position - obstacle.transform.position).magnitude - rightleast.transform.localScale.x * 0.5f - obstacle.transform.localScale.x * 0.5f;
                    float rightans2 = requester.transform.localScale.x;
                    // Debug.Log("rightleast.transform.position - obstacle.transform.position).magnitude-rightleast.transform.localScale.x*0.5f-obstacle.transform.localScale.x*0.5f is" + rightans1 + " requester.transform.localScale.x is" + rightans2);
                    getTangentCenter(rightleast.transform.position, 0.5f * rightleast.transform.localScale.x, requester.transform.position, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);
                    if (Vector3.Angle(Tangent1, obstacle.transform.position - requester.transform.position) > Vector3.Angle(Tangent2, obstacle.transform.position - requester.transform.position))
                    {
                        possibleEnd[possibleNum++] = Tangent1;

                    }
                    else
                    {
                        possibleEnd[possibleNum++] = Tangent2;
                    }
                }
                if (isUseful(requester.transform.position, tragetpos, obstacle.transform.position, leftleast.transform.position) && (leftleast.transform.position - obstacle.transform.position).magnitude - leftleast.transform.localScale.x * 0.5f - obstacle.transform.localScale.x * 0.5f >= requester.transform.localScale.x)
                {
                    possibleEnd[possibleNum++] = (obstacle.transform.position + 0.5f * (leftleast.transform.position - obstacle.transform.position)) - requester.transform.position;

                }
                if (isUseful(requester.transform.position,tragetpos, obstacle.transform.position, rightleast.transform.position) && (rightleast.transform.position - obstacle.transform.position).magnitude - rightleast.transform.localScale.x * 0.5f - obstacle.transform.localScale.x * 0.5f >= requester.transform.localScale.x)
                {
                    possibleEnd[possibleNum++] = obstacle.transform.position + 0.5f * (rightleast.transform.position - obstacle.transform.position) - requester.transform.position;

                }
                //Debug.Log("possable num is"+possibleNum);
                for (int i = 0; i < possibleNum; i++)
                {
                    //Debug.Log("possible num ="+i+" magnitude is " + possibleEnd[i].magnitude);
                }
                bubblesort(ref possibleEnd, possibleNum, requester.transform.position);
                // Debug.Log("after sort");
                for (int i = 0; i < possibleNum; i++)
                {
                    //Debug.Log("possible num =" + i + " magnitude is " + possibleEnd[i].magnitude);
                }
                if (possibleNum != 0)
                {
                    if (overrideForward(obstacle, lastobstacle, possibleEnd[0].normalized, lastforward))
                    {
                        //Debug.Log("In marker 10 forward is" + possibleEnd[0]);
                        record.endPoint = possibleEnd[0] + requester.transform.position;
                        record.forward = possibleEnd[0];
                    }
                    record.caseMark = 10;
                }
                else
                {//做obsnum為0的事項
                    if (Vector3.Angle(requester.transform.position - obstacle.transform.position, tragetpos - obstacle.transform.position) > 90)//感测区内只有一个物件,没有错误的话必定是请求者
                    {//如果由碰撞触发请求者障碍物的向量和障碍物目标的向量差角可能小於於90度,那时就不用绕过障碍了
                        Vector3 origonToEnd =tragetpos - requester.transform.position;
                        Vector3 requesterpos = requester.transform.position;
                        //Debug.Log("before getCenter obstacle is"+obstacle.name+"position is"+obstacle.transform.position);
                        getTangentCenter(obstacle.transform.position, 0.5f * obstacle.transform.localScale.x, requesterpos, 0.5f * requester.transform.localScale.x, out Tangent1, out Tangent2);
                        float Angle1 = Vector3.Angle(origonToEnd, Tangent1);
                        float Angle2 = Vector3.Angle(origonToEnd, Tangent2);
                        //Debug.Log("one:" +Angle1+",two"+Angle2);
                        if (Angle1 < Angle2)
                        {
                            //Debug.Log("Angle1 is little");
                            Vector3 forward = Tangent1;
                            forward.Normalize();//轉化為單位向量
                            if (overrideForward(obstacle, lastobstacle, forward.normalized, lastforward))
                            {
                                record.forward = forward;
                                record.endPoint = requesterpos + Tangent1; //forward * (origonToEnd.magnitude*Mathf.Cos((Angle1/180)*Mathf.PI));
                            }
                            // Debug.Log("mati of origin to end ia" + (record.endPoint - record.origin).magnitude);
                            record.caseMark = 11;
                        }
                        else
                        {
                            //Debug.Log("Angle2 is little");
                            Vector3 forward = Tangent2;
                            forward.Normalize();//轉化為單位向量
                            if (overrideForward(obstacle, lastobstacle, forward.normalized, lastforward))
                            {
                                record.forward = forward;
                                // Debug.Log("forward ia" + forward);
                                record.endPoint = requesterpos + Tangent2;//forward * (origonToEnd.magnitude * Mathf.Cos((Angle2 / 180) * Mathf.PI));
                            }                                         // Debug.Log("mati of origin to end ia" + (record.endPoint - record.origin).magnitude);
                            record.caseMark = 12;
                        }

                    }
                    else
                    {
                        record.obstacle = null;
                        record.caseMark = -2;
                    }
                }
            }

        }

    }
    public static void getTangentCenter(Vector3 goalC,float goalR,Vector3 mainC,float mainR,out Vector3 ans1,out Vector3 ans2)//得到一个圆与目标圆相切时的圆心位置,goalC為目標圓圓心位置,goalR為目標圓半徑長度,mainC為圓開始時的圓心位置,mainR為圓的半徑,ans1,ans2為得出可能的圓心位置
    {
        Vector3 centerLine = goalC - mainC;//得到两圆中点连线的向量
        //Vector3 backup = goalC - mainC;//用于OrthoNormalize的向量,使用完后会被function改成单位向量
        Vector3 Tangent=new Vector3(1,0,0);
        //Debug.Log("goalC="+goalC);
       // Debug.Log("backup =" + backup+" x "+backup.x);
        Tangent.z = -(centerLine.x / centerLine.z);
        Tangent.Normalize();
        //Debug.Log("tangent ="+Tangent);
        ans1 = centerLine + Tangent * (goalR + mainR);
        ans2 = centerLine - Tangent * (goalR + mainR);



    }
    void bubblesort(ref Vector3[] node, int num, Vector3 origin)//泡沫排序用於根據長度排序終點,0索引為最短終點
    {
        Vector3 temp;
        for(int i = num - 1; i > 0; i--)
        {
            //Debug.Log("in sort i=" + i);
            for(int j = 0; j < i; j++)
            {
               // Debug.Log("in sort j=" + j);
                if (node[j + 1] .magnitude < node[j].magnitude)
                {
                    
                    temp = node[j];
                    node[j] = node[j + 1];
                    node[j + 1] = temp;
                }
            }
        }
    }
    float getS(Vector2 Apos, Vector2 Bpos, Vector2 Cpos)//判斷Cpos在AposBpos的右邊則回傳負值,左邊回傳正值
    {
        return (Apos.x - Cpos.x) * (Bpos.y - Cpos.y) - (Apos.y - Cpos.y) * (Bpos.x - Cpos.x);
    }
    bool isUseful(GameObject A, GameObject B, GameObject C, GameObject D)//如果AB位置連線與CD位置連線回傳true
    {
        float ans = Vector3.Dot(Vector3.Cross(B.transform.position - A.transform.position, C.transform.position - A.transform.position), Vector3.Cross(B.transform.position - A.transform.position, D.transform.position - A.transform.position));
        float ans2 = Vector3.Dot(Vector3.Cross(D.transform.position - C.transform.position, A.transform.position - C.transform.position), Vector3.Cross(D.transform.position - C.transform.position, B.transform.position - C.transform.position));
        //Debug.Log("ans1 is" + ans + "ans2 is" + ans2);
        if (ans < 0 && ans2 < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool isUseful(Vector3 Apos, Vector3 Bpos, Vector3 Cpos, Vector3 Dpos)//如果Apos Bpos的連線與Cpos Dpos的連線
    {
        float ans = Vector3.Dot(Vector3.Cross(Bpos - Apos, Cpos - Apos), Vector3.Cross(Bpos - Apos, Dpos - Apos));
        float ans2 = Vector3.Dot(Vector3.Cross(Dpos - Cpos, Apos - Cpos), Vector3.Cross(Dpos - Cpos, Bpos - Cpos));
        //Debug.Log("ans1 is" + ans + "ans2 is" + ans2);
        if (ans < 0 && ans2 < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool overrideForward(GameObject newobs,GameObject oldobs,Vector3 newforward,Vector3 oldforward)//用来判断如果是同一个障碍,寻路方向是否相差太大,太大的定義是超過90度
    {
        if (newobs == oldobs)
        {
            if (Vector3.Angle(newforward,oldforward)>90)
            {
                return false;
            }
        }
        return true;
    }
}
