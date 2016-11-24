using UnityEngine;
using System.Collections;

public class MainRoleBehavior : MonoBehaviour {
    public responesAction respAction;
    public int roleNo;
    public Transform twoD;
	// Use this for initialization
	void Start () {
        twoD = transform.Find("twoD");
        roleNo =twoD.GetComponent<unit>().No;
        Debug.Log("No is"+roleNo);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            click.y = transform.position.y;
            respAction.OnMove(this.transform.position,click,roleNo);
        }
        if (Input.GetKeyDown(KeyCode.Z))//攻擊
        {
            Debug.Log("Z down");
            respAction.OnAttrack(this.transform.position, this.transform.eulerAngles, roleNo,true);
        }
    }
}
