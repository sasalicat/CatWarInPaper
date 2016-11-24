using UnityEngine;
using System.Collections;

public class Attack_Fuction : MonoBehaviour
{
    public GameObject missiles;
    public Animator animator;
    public unit unit;
    //攻擊屬性
    public int kind;
    public int damage;
    public float stiff;
    public float interval;
    public commenfContorl commend;
    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        unit = (unit)gameObject.GetComponent("unit");
        //屬性加成
        if (kind == 1)
        {
            damage += (int)(damage * 1.5f * (((float)unit.power) / 100));
        }
        else if (kind == 2)
        {
            damage += (int)(damage * 1.5f * (((float)unit.skill) / 100));
        }
        stiff += stiff * (((float)unit.stiffable) / 100);
        interval -= interval * (((float)unit.attackSpeed) / 100);
        //
        commend = transform.parent.GetComponent<commenfContorl>();
    }

    // Update is called once per frame
    void Update()
    {

       
    }
    public void single_attack()
    {

        GameObject obj = Instantiate(missiles, transform.TransformPoint(new Vector3(0, -3.2f, 0)), transform.rotation) as GameObject;
        ((Missile)obj.GetComponent("Missile")).damage = new damage(1, damage, stiff);
    }
    public void finish()
    {

        unit.action = unit.actionBefore;
        animator.SetInteger("action", unit.action);
    }
}
