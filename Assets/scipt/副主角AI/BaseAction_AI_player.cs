using UnityEngine;
using System.Collections;

public class BaseAction_AI_player : MonoBehaviour {
    public Animator animator;
    public float speed;
    public unit unit;
    public float attackInterval;
    // Use this for initialization
    void Start () {
        unit = (unit)transform.Find("twoD").GetComponent("unit");
        animator = transform.Find("twoD").GetComponent<Animator>();
        speed = unit.speed;
        attackInterval = unit.attackInterval;
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void stay_AI_player()
    {
        if (unit.action == 2)
        {
            unit.actionBefore = unit.action;
            unit.action = 1;
            animator.SetInteger("action", unit.action);
        }
    }
    public void move_AI_player(Vector3 direction)
    {
        //Debug.Log("Enter move");
        if (unit.stiffTime <= 0 && (!unit.conversely) && unit.alive && (!unit.exiled))
        {
            //Debug.Log("Enter if");
           
            direction = direction.normalized;
            if (unit.action == 1)
            {
                unit.actionBefore = unit.action;
                unit.action = 2;
                //animator.SetInteger("action", unit.action);
            }
            //Debug.Log("direction is " + direction + "speed is" + speed * Time.deltaTime);
            transform.position += direction * speed * Time.deltaTime;
        }
    }
    public void arrow_AI_player(GameObject traget)
    {
        Debug.Log("in arrow"+traget);
        if (unit.stiffTime <= 0 && (!unit.conversely) && unit.alive && (!unit.exiled)/*Input.GetMouseButton(0)*/)
        {
            Vector3 direct = traget.transform.position - transform.position;//向量等於末減初
            transform.forward = -direct;
        }
    }
    public void attack_AI_player()
    {
        if (unit.stiffTime <= 0 && unit.nextAttack <= 0 && !(unit.conversely) && unit.alive && !(unit.exiled))
        {
            //動畫控制--------------------------------------------
            if (unit.action <= 2)
            {
                unit.actionBefore = unit.action;
            }
            unit.action = 3;
            animator.SetInteger("action", unit.action);
            //產生間隔--------------------------------------------
             unit.nextAttack +=attackInterval;
        }
    }

}
