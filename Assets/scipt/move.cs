using UnityEngine;

using System.Collections;
using System;

public class move : MonoBehaviour {
   
    public Animator animator;
    public float speed;
    public unit unit;
   
    //public bool walk;
	// Use this for initialization
	void Start () {
        unit = (unit)transform.Find("twoD").GetComponent("unit");
        animator = transform.Find("twoD").GetComponent<Animator>();
        speed = unit.speed;
       
    }
	
	// Update is called once per frame
	void Update () {
        if (unit.stiffTime<=0&&(!unit.conversely)&&unit.alive&&(!unit.exiled))
        {
            if (unit.action == 2)
            {
                unit.actionBefore = unit.action;
                unit.action = 1;
                animator.SetInteger("action", unit.action);
            }
            Vector3 forward = new Vector3(0, 0, 0);
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
            {
                //walk = true;
                if (unit.action == 1)
                {
                    unit.actionBefore = unit.action;
                    unit.action = 2;
                    animator.SetInteger("action", unit.action);
                }
                forward.z += 1;
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
            {
                //walk = true;
                if (unit.action == 1)
                {
                    unit.actionBefore = unit.action;
                    unit.action = 2;
                    animator.SetInteger("action", unit.action);
                }
                forward.z -= 1;
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
            {
                //walk = true;
                if (unit.action == 1)
                {
                    unit.actionBefore = unit.action;
                    unit.action = 2;
                    animator.SetInteger("action", unit.action);
                }
                forward.x -= 1;
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
            {
                //walk = true;
                if (unit.action == 1)
                {
                    unit.actionBefore = unit.action;
                    unit.action = 2;
                    animator.SetInteger("action", unit.action);
                }
                forward.x += 1;
            }
            forward.Normalize();//轉化為單位向量
            forward = forward * (Time.deltaTime) * speed;
            transform.position = transform.position + forward;
            /*if (walk)
            {
                walk = false;
            }
            else
            {
                animator.SetBool("Walk", false);
            }*/
        }
    }
}
