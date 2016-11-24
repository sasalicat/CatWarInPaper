using UnityEngine;
using System.Collections;

public class attack_AI_player : MonoBehaviour {
    public retourer record;
    public unitManager manager;
    public GameObject traget;
    public BaseAction_AI_player action;
    public bool uncollision = true;
    public unit unit;
    // Use this for initialization
    void Start () {
        record = GetComponent<retourer>();
         manager = GameObject.Find("ManagerOBJ").GetComponent<unitManager>();
        action = GetComponent<BaseAction_AI_player>();
        record.traget = traget;
        unit = transform.Find("twoD").GetComponent<unit>();
       // Debug.DrawLine(transform.position, transform.position + (new Vector3(-0.5f,0,-0.9f)) * 100, Color.red, 20);


    }

    // Update is called once per frame
    void Update()
    {
        if (unit.commendMode == 2)
        {
            record.traget = traget;
            action.arrow_AI_player(traget);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.forward, out hit, 100, 2))//使用射线观察前方有没障碍
            {
                if (uncollision)
                {
                    //Debug.Log("enter ray");
                    //Debug.Log("enter hit is" + hit.transform.name);
                    if (hit.transform.gameObject != traget)
                    {
                         //Debug.Log("ENTER !=");
                        manager.detourRequest(hit.transform.gameObject, gameObject);
                    }
                    else//如果前面的是目標則攻擊
                    {
                        //Debug.Log("ENTER else1");
                        action.attack_AI_player();
                    }
                }
                else
                {
                   // Debug.Log("non uncollision");
                }
            }
            else//沒有障礙就攻擊
            {
                //Debug.Log("ENTER else1");
                action.attack_AI_player();

            }
            // Debug.Log("record forward is" + record.forward+"endpoint is"+record.endPoint);
            if (record.obstacle != null)
            {
                action.move_AI_player(record.forward);
                // Debug.Log("In AI endPoint is" + record.endPoint);
            }
            else
            {
                action.move_AI_player(traget.transform.position - transform.position);
            }
            uncollision = true;
            
            //Debug.Log("in AI attack traget is");
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (unit.commendMode == 2)
        {
            //Debug.Log("enter collision name:"+other.gameObject.name);
            
            if (other.gameObject != traget && other.gameObject.tag == "Player")
            {
                uncollision = false;
                manager.detourRequest(other.gameObject, gameObject);
            }
        }
    }


}
