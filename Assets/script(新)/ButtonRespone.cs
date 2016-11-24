using UnityEngine;
using System.Collections;

public class ButtonRespone : MonoBehaviour {
    //按鍵列表--------------------------------------------------------------
    public readonly KeyCode firstButton = KeyCode.Z;
    public readonly KeyCode secondButton = KeyCode.X;
    //---------------------------------------------------------------------
    BaseAction_AI_player action;
	// Use this for initialization
	void Start () {
        action = GetComponent<BaseAction_AI_player>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(firstButton))
        {
            action.attack_AI_player();
        }
        
	}
}
