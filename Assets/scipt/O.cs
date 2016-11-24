using UnityEngine;
using System.Collections;

public class O : MonoBehaviour {
    public float R;
    public Rigidbody rigidbody;
	// Use this for initialization
	void Start () {
        //rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(transform.right,((float)0.5*Mathf.PI));
        transform.position = transform.position +new Vector3(1,0,1)*Time.deltaTime;
        Debug.Log(transform.eulerAngles);
	}
    void FixedUpdate()
    {

        //rigidbody.AddForce(Vector3.right );
    }
}
