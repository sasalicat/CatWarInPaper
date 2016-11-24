using UnityEngine;
using System.Collections;

public class skill : MonoBehaviour {

    public GameObject missiles;
    public GameObject Player;
    public Animator animator;
    public unit unit;
    // Use this for initialization
    void Start()
    {
        Player = gameObject;
        animator = Player.GetComponent<Animator>();
        unit = (unit)gameObject.GetComponent("unit");
    }

    // Update is called once per frame
    void Update()
    {
        if (unit.stiffTime <= 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("mouse 1 down");
                if (unit.action <= 2)
                {
                    unit.actionBefore = unit.action;
                }
                unit.action = 4;
                animator.SetInteger("action", unit.action);
            }
        }
    }
    public void use_skill()
    {
        GameObject obj = Instantiate(missiles, transform.TransformPoint(new Vector3(0, -3.2f, 0)), transform.rotation) as GameObject;
    }
    public void skill_finish()
    {

        unit.action = unit.actionBefore;
        animator.SetInteger("action", unit.action);
    }
}
