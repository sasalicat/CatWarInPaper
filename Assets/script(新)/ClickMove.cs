using UnityEngine;
using System.Collections;

public class ClickMove : MonoBehaviour {

    private Transform transf;
    public bool going;
    public Vector3 endPoint;
    public BaseAction_AI_player action;
    public const float FIX_DISTANCE = 0.2f;
    // Use this for initialization
    void Start () {
        transf = this.transform;
        action = GetComponent<BaseAction_AI_player>();
	}
	
	// Update is called once per frame
	void Update () {
        //點擊--------------------------------------------------------------------
        if (Input.GetMouseButtonDown(0))
        {
            endPoint=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPoint.y = transf.position.y;
            going = true;
        }
        //動畫控制以及移動---------------------------------------------------------
        if(going)//表示還沒有到達目的地點
            action.move_AI_player(endPoint-transf.position);
        //確認是否到達指定地點
        if (going&&(endPoint - transf.position).magnitude < FIX_DISTANCE)
        {
            action.stay_AI_player();
            going = false;
        }
	
	}

}
