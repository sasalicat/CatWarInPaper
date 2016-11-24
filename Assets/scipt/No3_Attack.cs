using UnityEngine;
using System.Collections;

public class No3_Attack : MonoBehaviour {
    public GameObject missiles;

    public Animator animator;
    public unit unit;
    // Use this for initialization
    void Start() {
        animator = gameObject.GetComponent<Animator>();
        unit = (unit)gameObject.GetComponent("unit");
    }

    // Update is called once per frame
    void Update() {

    }

    void OnCollisionEnter(Collision  collision)
    {
        if (unit.action == 3)
        {

        }
    }
}