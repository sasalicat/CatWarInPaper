using UnityEngine;
using System.Collections;

public class keepCloss_AI_player : MonoBehaviour {
    public retourer record;
    public unitManager manager;
    public GameObject traget;
    public GameObject attacktraget;
    public BaseAction_AI_player action;
    public bool uncollision = true;
    public unit unit;
    public int playerMarker;//副主角标记,标记是Q还是E,1为Q,2为E
    // Use this for initialization
    void Start()
    {
        record = GetComponent<retourer>();
        manager = GameObject.Find("ManagerOBJ").GetComponent<unitManager>();
        action = GetComponent<BaseAction_AI_player>();
       
        unit = transform.Find("twoD").GetComponent<unit>();
    }

    // Update is called once per frame
    void Update()
    {
        record.traget = traget;
        //方向分配---------------------------------------------------------------------------------
        if (unit.commendMode == 4)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, traget.transform.position - transform.position, out hit, 100, 2))
            {
                if (uncollision)
                {
                    manager.detourRequest(hit.transform.gameObject, gameObject);
                }
            }


            //Debug.Log("Enter update");
            if ((new Vector2(traget.transform.position.x, traget.transform.position.z) - new Vector2(transform.position.x, transform.position.z)).magnitude > (traget.transform.localScale.x*0.5f+0.5))
            {
                if (record.obstacle != null)
                {
                    action.move_AI_player(record.forward);
                    // Debug.Log("In AI endPoint is" + record.endPoint);
                }
                else
                {
                    //Debug.Log("Enter move");
                    action.move_AI_player(traget.transform.position - transform.position);
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

            if (!Physics.Raycast(transform.position, -transform.forward,out hit, 100, 2))
            {//如果攻擊目標和物體之間沒有障礙物
                action.attack_AI_player();
            }
            else if(hit.transform.gameObject==traget)
            {
                action.attack_AI_player();
            }
        }

    }
    void OnCollisionEnter(Collision other)
    {
        if (unit.commendMode == 4)
        {
            //Debug.Log("enter collision name:"+other.gameObject.name);
            uncollision = false;
            if (other.gameObject != traget.gameObject && other.gameObject.tag == "Player")
            {
                manager.detourRequest(other.gameObject, gameObject);
            }
        }
    }
}
