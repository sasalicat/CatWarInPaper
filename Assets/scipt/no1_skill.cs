using UnityEngine;
using System.Collections;

public class no1_skill : MonoBehaviour {
    public GameObject missiles;
    public Animator animator;
    public unit unit;
    //技能屬性
    //public int kind;
    public int damage;
    public float stiff;
    public float interval;
    public int cost;
    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        unit = (unit)gameObject.GetComponent("unit");
        //屬性加成
        
        damage += (int)(damage * 1.5f * (unit.skill / 100));
        
        stiff += stiff * (unit.stiffable / 100);
        interval -= interval * (unit.attackSpeed / 100);
        //
    }

    // Update is called once per frame
    void Update()
    {
        if (unit.nowMP>= cost&&unit.stiffTime <= 0 && unit.nextAttack <= 0 && !(unit.conversely) && unit.alive && !(unit.exiled))
        {
            //Debug.Log("Enter baseattack");
            if (Input.GetMouseButtonDown(1))
            {
                //動畫控制--------------------------------------------
                //Debug.Log("mouse 0 down");
                if (unit.action <= 2)
                {
                    unit.actionBefore = unit.action;
                }
                unit.action = 4;
                animator.SetInteger("action", unit.action);
                //產生間隔--------------------------------------------
                unit.nowMP -= cost;
                unit.nextSkill += interval;
            }
        }
    }
    public void single_skill_no1()
    {
        GameObject obj = Instantiate(missiles, transform.TransformPoint(new Vector3(0, -3.2f, 0)), transform.rotation) as GameObject;
        ((Missile)obj.GetComponent("Missile")).damage = new damage(1, damage, stiff);
    }
    public void skill_finish_no1()
    {

        unit.action = unit.actionBefore;
        animator.SetInteger("action", unit.action);
    }
}
