using UnityEngine;
using System.Collections;

public class timeControl : MonoBehaviour {
    public Animator animator;
    public unit unit;
    // Use this for initialization
    void Start() {
        animator = gameObject.GetComponent<Animator>();
        unit = (unit)gameObject.GetComponent("unit");
       // unit.stiffTime = 5.0f;
       // animator.SetFloat("stiffTime", unit.stiffTime);
    }
	
	// Update is called once per frame
	void Update () {
        if (unit.stiffTime >= 0)//硬直计时
        {
            unit.stiffTime -= Time.deltaTime;
            animator.SetFloat("stiffTime", unit.stiffTime);
        }
        if (unit.conversely&&unit.alive)//倒地计时
        {
            if (unit.converselyTime >= 0)
            {
                unit.converselyTime -= Time.deltaTime;
            }
            else
            {
                unit.conversely = false;
                animator.SetBool("conversely", false);
            }
        }
        //能量恢復計數---------------------------------
        unit.nextRecover -= Time.deltaTime;
        if (unit.nextRecover <= 0)
        {
            if (unit.nowMP + unit.energyRecover_Second <= unit.maxMP)
            {
                unit.nowMP += unit.energyRecover_Second;
            }
            else
            {
                unit.nowMP = unit.maxMP;
            }
            unit.nextRecover += 1;
        }
        //攻擊間隔計數
        if (unit.nextAttack > 0)
        {
            unit.nextAttack -= Time.deltaTime;
        }
        //技能間隔計數
        if (unit.nextSkill > 0)
        {
            unit.nextSkill -= Time.deltaTime;
        }
    }
}
