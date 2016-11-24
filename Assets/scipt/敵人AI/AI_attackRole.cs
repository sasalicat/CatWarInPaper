using UnityEngine;
using System.Collections;

public class AI_attackRole : MonoBehaviour {
    public UniAttributes attribute;
    public BaseAction_AI_player action;
    public unitManager manager;
    public retourer record;
    public unit unit;
    public GameObject traget;
    public GameObject tragettool;
    bool uncollision=false;
    public bool move = false;
    // Use this for initialization
    void Start () {
        attribute = GetComponent<UniAttributes>();
        action = GetComponent<BaseAction_AI_player>();
        manager = GameObject.Find("ManagerOBJ").GetComponent<unitManager>();
        unit = transform.Find("twoD").GetComponent<unit>();
        record = GetComponent<retourer>();
    }
	
	// Update is called once per frame
	void Update () {
        //移動-----------------------------------------------------------
       traget= manager.gettraget(unit.No);
        record.traget = traget;
        move = false;
      
       
        RaycastHit hit;
        if (tragettool != null)
        {
            if (Physics.Raycast(transform.position, traget.transform.position - transform.position, out hit, 20, 2) && hit.transform.gameObject != tragettool)
            {
                manager.detourRequest(hit.transform.gameObject, gameObject,tragettool.transform.position);
            }
            
        }
        else
        {
            if (Physics.Raycast(transform.position, traget.transform.position - transform.position, out hit, 20, 2) && hit.transform.gameObject != traget)//如果有障礙,繞過障礙
            {
                Debug.Log(hit.transform.name);
                if (uncollision && hit.transform.tag == "Player")
                {
                    manager.detourRequest(hit.transform.gameObject, gameObject);
                }
            }
            else
            {
                float distance = (traget.transform.position - transform.position).magnitude;
                Debug.Log("distance is" + distance);
                if (attribute.attackSkill && attribute.skillConsume - unit.nowMP >= -5)//如果是攻擊型技能且將可以施放
                {
                    if (distance > attribute.skillRanage)
                    {
                        move = true;
                    }
                }
                if (distance > attribute.attackRange)
                {
                    move = true;
                }

            }
        }
        //進行移動
        if (tragettool == null)
        {
            if (record.obstacle != null)
            {
                action.move_AI_player(record.endPoint - transform.position);
                // Debug.Log("In AI endPoint is" + record.endPoint);
            }
            else if (move)
            {
                action.move_AI_player(traget.transform.position - transform.position);
            }
            else
            {
                action.stay_AI_player();
            }
        }
        else
        {
            if (record.obstacle != null)
            {
                action.move_AI_player(record.endPoint - transform.position);
                // Debug.Log("In AI endPoint is" + record.endPoint);
            }
            else {
                action.move_AI_player(tragettool.transform.position-transform.position);
            }
        }
        uncollision = true;
        //轉向--------------------------------------------------------------------
        action.arrow_AI_player(traget);
        //攻擊--------------------------------------------------------------------
        if(Physics.Raycast(transform.position, traget.transform.position - transform.position, out hit, 20, 2) && hit.transform.gameObject == traget)
        {
            action.attack_AI_player();
        }
        //

    }


    void OnCollisionEnter(Collision other)
    {
       
            //Debug.Log("enter collision name:"+other.gameObject.name);
            uncollision = false;
            if (other.gameObject != traget.gameObject && other.gameObject.tag == "Player")
            {
                manager.detourRequest(other.gameObject, gameObject);
            }
        }
    
}
