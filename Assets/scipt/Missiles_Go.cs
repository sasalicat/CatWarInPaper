using UnityEngine;
using System.Collections;

public class Missiles_Go : MonoBehaviour {
    public int speed=20;
    GameObject obj;
    public    Missile missile;
    
	// Use this for initialization
	void Start () {
        missile = GetComponent<Missile>(); 
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0, -1, 0) * speed*Time.deltaTime);
        

	}
    void OnTriggerEnter(Collider other)
    {
       // Debug.Log("OnTiggerEnter");
        hurt hurt = (hurt)other.GetComponent("hurt");
        if (hurt != null)
        {
        //    Debug.Log("have hurt");
            hurt.takeDamage(missile.damage);
        }
    }
}
