using UnityEngine;
using System.Collections;

public class commenfContorl : MonoBehaviour {
    public bool Qmood=false;
    public bool Emood = false;
    public GameObject Qrole;
    public GameObject Erole;
    public unit Qunit;
    public unit Eunit;
	// Use this for initialization
	void Start () {
        Debug.Log("twoD is"+ Qrole.transform.Find("twoD"));
        Debug.Log("unit is" + Qrole.transform.Find("twoD").GetComponent<unit>());
        Qunit = Qrole.transform.Find("twoD").GetComponent<unit>();
        Eunit = Erole.transform.Find("twoD").GetComponent<unit>();
        Debug.Log("goto_AI is"+Qrole.transform.GetComponent<goTo_AI_player>());
        Qrole.transform.GetComponent<goTo_AI_player>().Marker = 'Q';
        Erole.transform.GetComponent<goTo_AI_player>().Marker = 'E';
        Debug.Log("Erole is" + Erole.name);
    }

    // Update is called once per frame
    void Update()
    {
        //QE切換------------------------------------------------------------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q down");
            if (Emood)//如果Emood已經啟動,關閉Emood
            {
                Emood = false;
            }

            if (Qmood)//如果Qmood已經啟動,則關閉Qmood
            {
                Qmood = false;

            }
            else
            {
                Qmood = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Qmood)
            {
                Qmood = false;
            }
            if (Emood)
            {
                Emood = false;
            }
            else
            {
                Emood = true;
            }
        }
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.D))//如果有移動鍵按下,取消所有控制模式
        {
            Qmood = false;
            Emood = false;
        }
        //QE指令------------------------------------------------------------------------------------------------------------------------------
        if (Qmood)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Vector3 mousepoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Physics.Raycast(mousepoint, new Vector3(0, -1, 0), out hit, 100, 2))
                {
                    Debug.Log(hit.transform.name);
                    GameObject obj = hit.transform.gameObject;
                    unit objunit = obj.transform.Find("twoD").GetComponent<unit>();
                    if (objunit.nonlife)//點擊為掩體,進入靠近模式
                    {
                        Debug.Log("Enter mode 4");
                        Qunit.commendMode = 4;
                        (Qrole.GetComponent<keepCloss_AI_player>()).traget = obj;
                    }
                    else if (objunit.team != Qunit.team && !(objunit.tools))//點擊為敵人,進入攻擊模式
                    {
                        Qunit.commendMode = 2;
                        (Qrole.GetComponent<attack_AI_player>()).traget = obj;
                        //(Qrole.GetComponent<BaseAction_AI_player>()).arrow_AI_player(obj);//強制轉向到目標,若不這麼做,Qrole會將當前面向的角色作為障礙觸發繞過請求
                    }
                    else if (objunit.team == Qunit.team)//點擊為隊友
                    {
                        if (!objunit.alive)//如果點擊的是死亡的角色,進入救援模式
                        {

                        }
                        else//進入跟隨模式
                        {
                            Qunit.commendMode = 1;
                        }
                        
                    }
                    else if (objunit.tools)//點擊為道具,進入移動模式
                    {
                        Qunit.commendMode = 3;

                        GameObject tragetposition = (Qrole.GetComponent<goTo_AI_player>().tragetposition);
                        //tragetposition.SetActive(true);
                        tragetposition.transform.position = new Vector3(mousepoint.x, transform.position.y, mousepoint.z);
                    }
                   
                   
                }
                else//點擊地面
                {
                    Qunit.commendMode = 3;
              
                    GameObject tragetposition = (Qrole.GetComponent<goTo_AI_player>().tragetposition);
                    //tragetposition.SetActive(true);
                    tragetposition.transform.position = new Vector3(mousepoint.x,transform.position.y,mousepoint.z);
                }
            }
        }
        else if (Emood)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Vector3 mousepoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, -1, 0), out hit, 100, 2))
                {
                    Debug.Log(hit.transform.name);
                    GameObject obj = hit.transform.gameObject;
                    unit objunit = obj.transform.Find("twoD").GetComponent<unit>();
                    if (objunit.nonlife)//點擊為掩體,進入靠近模式
                    {
                        Debug.Log("Enter mode 4");
                        Eunit.commendMode = 4;
                        (Erole.GetComponent<keepCloss_AI_player>()).traget = obj;
                    }
                    else if (objunit.team != Qunit.team&&!(objunit.tools))//點擊為敵人,進入攻擊模式
                    {
                        Debug.Log(obj.name + " tools is" + objunit.tools);
                        Eunit.commendMode = 2;
                        (Erole.GetComponent<attack_AI_player>()).traget = obj;
                        //(Qrole.GetComponent<BaseAction_AI_player>()).arrow_AI_player(obj);//強制轉向到目標,若不這麼做,Qrole會將當前面向的角色作為障礙觸發繞過請求
                    }
                    else if (objunit.team == Qunit.team)//點擊為隊友
                    {
                        if (!objunit.alive)//如果點擊的是死亡的角色,進入救援模式
                        {

                        }
                        else//進入跟隨模式
                        {
                            Eunit.commendMode = 1;
                        }

                    }
                    else if (objunit.tools)//點擊為道具,進入移動模式
                    {
                        Eunit.commendMode = 3;

                        GameObject tragetposition = (Erole.GetComponent<goTo_AI_player>().tragetposition);
                        //tragetposition.SetActive(true);
                        tragetposition.transform.position = new Vector3(mousepoint.x, transform.position.y, mousepoint.z);
                    }


                }
                else//點擊地面
                {
                    Eunit.commendMode = 3;

                    GameObject tragetposition = (Erole.GetComponent<goTo_AI_player>().tragetposition);
                    //tragetposition.SetActive(true);
                    tragetposition.transform.position = new Vector3(mousepoint.x, transform.position.y, mousepoint.z);
                }
            }
        }
    }
}
