using UnityEngine;
using System.Collections;

public class AIaction : MonoBehaviour {
    public GameObject traget;
    float AttackRange=5;
    Transform mytrans;
    public BaseAction_AI_player action;
	// Use this for initialization
	void Start () {
        action=GetComponent<BaseAction_AI_player>();
        mytrans = transform;
	}
	
	// Update is called once per frame
	void Update () {
        //转向--------------------------------------------------
        Debug.Log("arrow");
        action.arrow_AI_player(traget);
        //移动攻击--------------------------------------------------
        if ((traget.transform.position - mytrans.position).magnitude > AttackRange)
        { 
            action.move_AI_player(traget.transform.position-mytrans.position);
            
        }
        else
        {
            action.stay_AI_player();
        }
        action.attack_AI_player();
    }
}
