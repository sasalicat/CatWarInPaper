using UnityEngine;
using System.Collections;

public class Net_BaseAction :BaseAction_AI_player{
    public void Passive_CreateStiff(float time)
    {
        if (unit.stiffTime <= time)//硬直結算:如果單位當前硬直小於傷害造成的硬直
        {
            unit.stiffTime = time;//將當前硬直設定為技能造成硬直
            animator.SetFloat("stiffTime", unit.stiffTime);
        }
    }
    public void Passive_CreateConversaly(float time)
    {
        unit.conversely = true;
        animator.SetBool("conversely", true);
        unit.converselyTime += time;
    }

}
