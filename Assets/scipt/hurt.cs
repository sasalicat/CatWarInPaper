using UnityEngine;
using System.Collections;

public class hurt : MonoBehaviour {
    public Animator animator;
    public unit unit;
    // Use this for initialization
    void Start () {
        unit = (unit)transform.Find("twoD").GetComponent("unit");
        animator = transform.Find("twoD").GetComponent<Animator>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (unit.nowHP<=0&&unit.alive&&!(unit.exiled)&&unit.canBeKill)//死亡判定
        {
            unit.alive = false;
            Debug.Log("in dead conversely" + unit.conversely);
            if (!(unit.conversely))
            {
                Debug.Log("in dead enter if");
                unit.conversely = true;
                Debug.Log("in dead if conversely" + unit.conversely);
                animator.SetBool("conversely", true);
            }
        }
	}
    public virtual void takeDamage(damage damage)
    {
        
        if (unit.alive && !(unit.exiled))//單位活著且沒有被放逐
        {

            if (damage.kind == 1 && !(unit.immune_attack))//受到的是戰鬥傷害且單位不是物免
            {
                if (!(unit.conversely) || damage.hitConversely)//如果單位沒倒地或傷害能攻擊倒地單位
                {
                    unit.nowHP -= (damage.num - (damage.num * (unit.damageReduce / 100)));//傷害結算:單位生命值減少傷害量減物理減傷
                    
                }
                if (unit.canBeStiff)//硬直判定
                { 
                    float realstiff = damage.stiffTime - damage.stiffTime * (unit.stiffReduce / 100);
                    if (unit.stiffTime <= realstiff)//硬直結算:如果單位當前硬直小於傷害造成的硬直
                    {
                        unit.stiffTime = realstiff;//將當前硬直設定為技能造成硬直
                        animator.SetFloat("stiffTime", unit.stiffTime);
                    }
                    
                }
                if (damage.makeConversaly&&(!unit.conversely)&&unit.canBeConvesly)//倒地判定:如果伤害会击倒,且单位不为击倒状态,且可以被击倒
                {
                    unit.conversely = true;
                    animator.SetBool("conversely", true);
                    unit.converselyTime += unit.standConveslyTime;
                }

            }
            else if (damage.kind == 2 && !(unit.immune_skill))//受到的是戰鬥傷害且單位不是物免
            {
                if (!(unit.conversely) || damage.hitConversely)//如果單位沒倒地或傷害能攻擊倒地單位
                {
                    unit.nowHP -= (damage.num - (damage.num * (unit.energyReduce / 100)));//單位生命值減少傷害量減特殊減傷
                }
                if (unit.canBeStiff)
                {
                    float realstiff = damage.stiffTime - damage.stiffTime * (unit.stiffReduce / 100);
                    if (unit.stiffTime <= realstiff)//硬直結算:如果單位當前硬直小於傷害造成的硬直
                    {
                        unit.stiffTime = realstiff;//將當前硬直設定為技能造成硬直
                        animator.SetFloat("stiffTime", unit.stiffTime);
                    }
                }
                if (damage.makeConversaly && (!unit.conversely) && unit.canBeConvesly)//倒地判定
                {
                    unit.conversely = true;
                    animator.SetBool("conversely", true);
                    unit.converselyTime += unit.standConveslyTime;
                }
            }
            
        }
        
    }

}
