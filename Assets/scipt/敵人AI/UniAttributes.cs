using UnityEngine;
using System.Collections;

public class UniAttributes : MonoBehaviour {
    public float attackRange;
    public float skillRanage;
    public int skillConsume;
    public bool attackSkill;//技能是否為攻擊性
    public int role;//角色定位
                    //1:戰鬥角色
                    //2.輔助角色
                    //3.拾荒者角色
    public BaseAction_AI_player action;
    public float pickupRadius;


	// Use this for initialization
	void Start () {
        action = GetComponent<BaseAction_AI_player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
