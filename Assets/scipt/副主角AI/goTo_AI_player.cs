using UnityEngine;
using System.Collections;

public class goTo_AI_player : MonoBehaviour {
    public retourer record;
    public unitManager manager;
    public GameObject tragetposition;
    public GameObject attacktraget;
    public BaseAction_AI_player action;
    public bool uncollision = true;
    public unit unit;
    public char Marker;//副主角标记,标记是Q还是E,1为Q,2为E
    // Use this for initialization
    void Start () {
        record = GetComponent<retourer>();
        manager = GameObject.Find("ManagerOBJ").GetComponent<unitManager>();
        action = GetComponent<BaseAction_AI_player>();
       // Debug.Log("manager is" + GameObject.Find("ManagerOBJ")+"componment is" + GameObject.Find("ManagerOBJ").GetComponent<unitManager>()+"marker is"+Marker);
        /*if (Marker.Equals('Q'))
        {
            Debug.Log("enter 1");
            tragetposition = manager.tragetpointQ;
        }
        else if (Marker.Equals( 'E'))
        {
            Debug.Log("enter 2");
            tragetposition = manager.tragetpointE;
        }*/
        unit = transform.Find("twoD").GetComponent<unit>();
    }
	
	// Update is called once per frame
	void Update () {
        record.traget = tragetposition;
        //方向分配---------------------------------------------------------------------------------
        if (unit.commendMode == 3)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, tragetposition.transform.position - transform.position, out hit, 20, 2))
            {
                if (uncollision&&hit.transform.tag=="Player")
                {
                    manager.detourRequest(hit.transform.gameObject, gameObject);
                }
            }


            //Debug.Log("Enter update");
            if ((new Vector2(tragetposition.transform.position.x, tragetposition.transform.position.z) - new Vector2(transform.position.x, transform.position.z)).magnitude > 0.2)
            {
                if (record.obstacle != null)
                {
                    action.move_AI_player(record.forward);
                    // Debug.Log("In AI endPoint is" + record.endPoint);
                }
                else
                {
                    //Debug.Log("Enter move");
                    action.move_AI_player(tragetposition.transform.position - transform.position);
                }
            }
            else
            {
                action.stay_AI_player();
            }
            uncollision = true;
            //攻擊----------------------------------------------------------------------------------
            attacktraget = manager.gettraget(unit.No);
            action.arrow_AI_player(attacktraget);

            if (!Physics.Raycast(transform.position, -transform.forward, out hit, 100, 2))
            {//如果攻擊目標和物體之間沒有障礙物
                
                    action.attack_AI_player();
                
            }
            else if(hit.transform.gameObject==attacktraget)
            {//如果障碍物是攻击目标
                action.attack_AI_player();
                //Debug.Log(" have obs in mood3 "+hit.transform.name);
            }
        }

    }
    void OnCollisionEnter(Collision other)
    {
        if (unit.commendMode == 3)
        {
            //Debug.Log("enter collision name:"+other.gameObject.name);
            uncollision = false;
            if (other.gameObject != tragetposition.gameObject && other.gameObject.tag == "Player")
            {
                manager.detourRequest(other.gameObject, gameObject);
            }
        }
    }
}
