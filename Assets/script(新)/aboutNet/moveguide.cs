using UnityEngine;
using System.Collections;

public class moveguide : MonoBehaviour {
    public const float Residual = 0.2f;
    bool guiding=false;
    public Vector3 endPoint;
    BaseAction_AI_player action;
    Vector3 endToNow;
    // Use this for initialization
    public void Start()
    {
        action = GetComponent<BaseAction_AI_player>();
    }
    public void Update()
    {
        if (guiding)
        {
            endToNow = endPoint - transform.position;
            //Debug.Log("end to now is" + endToNow);
            if (endToNow.magnitude > Residual)
            {
                //Debug.Log("enter >");
                action.move_AI_player(endPoint - transform.position);
            }
            else
            {
                guiding = false;
                action.stay_AI_player();
            }
        }
    }
    public void setEndPoint(Vector3 tragetpoint)
    {
        endPoint = tragetpoint;
        guiding = true;
    }
}
