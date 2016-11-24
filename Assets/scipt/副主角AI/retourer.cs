using UnityEngine;
using System.Collections;

public class retourer : MonoBehaviour {
    

    public Vector3 origin;//記錄觸發尋路的原點,用來判斷是否超過終點
    public GameObject obstacle;//標記繞過的障礙
    public Vector3 endPoint;//尋路的終點
    public Vector3 forward;//尋路的方向
    public sencer sencer;//自己的感測區
    public GameObject traget;//角色的目標
    public int caseMark=0;//用来标记寻路判断的类型化,名字叫寻路烙印比较绚酷
   

    // Use this for initialization
    void Start () {
        sencer = transform.Find("area").GetComponent<sencer>();

    }
	
	// Update is called once per frame
	void Update () {
       // Debug.Log("(transform.position-origin).magnitude is" + (transform.position - origin).magnitude + "(endPoint-origin).magnitude is" + (endPoint - origin).magnitude);
       // Debug.Log("origin is" + origin + "position is" + transform.position);
        if ((new Vector3(endPoint.x,0,endPoint.z)-new Vector3(transform.position.x,0,transform.position.z)).magnitude<=0.2)
        {
           
            //Debug.Log("enter endpoint!!!!!!!!!!!!!!!!!!!!!!!!!!!!! it is"+endPoint);
           // Debug.Log("this turn obs is" + obstacle);
            origin = new Vector3(0,0,0);
            damage d = new damage(1,2,1);
            obstacle = null;
            endPoint = new Vector3(0, 0, 0);
            forward = new Vector3(0, 0, 0);
        }
        //Debug
        if (!endPoint.Equals(new Vector3(0, 0, 0)))
            (GameObject.Find("mark") as GameObject).transform.position = endPoint;
	}
}
