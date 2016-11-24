using UnityEngine;
using System.Collections;

public class follow_AI_player : MonoBehaviour {
    public const float MAX_DISTANCE = 3;
    public Animator animator;
    public unit unit;
    public GameObject master;
    public BaseAction_AI_player AIAction;
    public unitManager manager;
    public GameObject traget;


    // Use this for initialization
    void Start()
    {
        unit = (unit)transform.Find("twoD").GetComponent("unit");
        animator = transform.Find("twoD").GetComponent<Animator>();
        master = unit.master;
        AIAction = GetComponent("BaseAction_AI_player")as BaseAction_AI_player;
        manager = GameObject.Find("ManagerOBJ").GetComponent<unitManager>();
        
       
    }	
	// Update is called once per frame
	void Update () {
        if (unit.commendMode == 1)
        {

            //移動------------------------------------------------------------------------------------------
            //Debug.Log("distance:" + (transform.position - master.transform.position).magnitude);
            if ((transform.position - master.transform.position).magnitude > MAX_DISTANCE && unit.master != null)//弱國和主角距離超過最大距離
            {
                AIAction.move_AI_player(master.transform.position - transform.position);//AI移動
            }
            else//否則AI靜止
            {
                AIAction.stay_AI_player();
            }

            traget = manager.gettraget(unit.No);//獲取管理者指派的最近目標
                                                //轉向-------------------------------------------------------------------------------------------
            if (traget != null)
            {
                AIAction.arrow_AI_player(traget);//如果有目標,則朝向目標
            }
            else if (unit.master != null)
            {
                AIAction.arrow_AI_player(master);//如果沒有目標.則朝向主角
            }
            //攻擊--------------------------------------------------------------------------------------------
            if (unit.nextAttack <= 0)
            {
                AIAction.attack_AI_player();
            }
        }
        
    }
}
