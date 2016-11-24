using UnityEngine;
using System.Collections.Generic;

public class Hurt_Net : hurt {
    public newPhotonClient client;
    // Use this for initialization

    public override void  takeDamage(damage damage)
    {
        Debug.Log("in net take damage");
        if (unit.alive && !(unit.exiled))//單位活著且沒有被放逐
        {
            int realnum = 0;
            float realstiff = 0;
            float conversalyTime = 0;
            if (damage.kind == 1 && !(unit.immune_attack))//受到的是戰鬥傷害且單位不是物免
            {

                if (!(unit.conversely) || damage.hitConversely)//如果單位沒倒地或傷害能攻擊倒地單位
                {
                    realnum= (damage.num - (damage.num * (unit.damageReduce / 100)));//傷害結算:單位生命值減少傷害量減物理減傷

                }
                if (unit.canBeStiff)//硬直判定
                {
                   realstiff= damage.stiffTime - damage.stiffTime * (unit.stiffReduce / 100);
                   

                }
                if (damage.makeConversaly && (!unit.conversely) && unit.canBeConvesly)//倒地判定:如果伤害会击倒,且单位不为击倒状态,且可以被击倒
                {
                    conversalyTime = unit.standConveslyTime;
                }

            }
            else if (damage.kind == 2 && !(unit.immune_skill))//受到的是戰鬥傷害且單位不是物免
            {

                if (!(unit.conversely) || damage.hitConversely)//如果單位沒倒地或傷害能攻擊倒地單位
                {
                   realnum-= (damage.num - (damage.num * (unit.energyReduce / 100)));//單位生命值減少傷害量減特殊減傷
                }
                if (unit.canBeStiff)
                {
                    realstiff = damage.stiffTime - damage.stiffTime * (unit.stiffReduce / 100);
                    
                }
                if (damage.makeConversaly && (!unit.conversely) && unit.canBeConvesly)//倒地判定
                {
                    conversalyTime = unit.standConveslyTime;
                }
            }
            var parameter = new Dictionary<byte, object>()
        {
            {(byte)1,realnum },
            {(byte)2,realstiff },
            {(byte)3,conversalyTime },
            {(byte)4,unit.No }
            
        };
            client.peer.OpCustom(4, parameter, true);
            
        }

    }
}
