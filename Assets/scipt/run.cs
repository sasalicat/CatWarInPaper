using UnityEngine;
using System.Collections;
using DG.Tweening;

public class run : MonoBehaviour {
    public int speed=20;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0)) {
            // Debug.Log(transform.localPosition);
            float rotaioin = transform.eulerAngles.y + 90;
            transform.DOMove(new Vector3(transform.position.x+speed  * Mathf.Cos(rotaioin / 180 * Mathf.PI), transform.position.y, transform.position.z-(speed * Mathf.Sin(rotaioin / 180 * Mathf.PI))),1).SetEase(Ease.InBack);
            
        }
	}
}
