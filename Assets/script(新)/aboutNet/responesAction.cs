using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class responesAction : MonoBehaviour {
    newPhotonClient client;
    public PhotonManager2 manager;
    public GameObject[] roleTemple=new GameObject[6];
    
	// Use this for initialization
	void Start () {
        client = manager.client;
       
	}
	
	// Update is called once per frame
	void Update () {
      
	
	}
    public void OnMove(Vector3 pos,Vector3 endpoint, int no)
    {
      
        var parameter = new Dictionary<byte, object>()
                    {
                         {(byte)1,no },
                        {(byte)2,pos.x},

                        {(byte)3,pos.y },
                         {(byte)4,pos.z },
                          {(byte)5,endpoint.x },
                           {(byte)6,endpoint.y },
                            {(byte)7,endpoint.z }
                    };
       client.peer.OpCustom(1, parameter, true);
    }
    public void OnAttrack(Vector3 pos,Vector3 rotation,int no,bool first)
    {
        Debug.Log("on attack");
        var parameter = new Dictionary<byte, object>()
        {
            {(byte)1,no },
            {(byte)2,pos.x },
            {(byte)3,pos.y },
            {(byte)4,pos.z },
             {(byte)5,rotation.x },
             {(byte)6,rotation.y },
             {(byte)7,rotation.z },
            {(byte)8,first}
        };
        client.peer.OpCustom(3, parameter, true);
    }
    
    public void OnClientRespone(OperationResponse operationResponse)
    {
        Debug.Log("OnRespone");
        switch (operationResponse.OperationCode)
        {

            case 1://移動
                {
                    
                    int controlNo = (int)operationResponse.Parameters[1];
                    //Debug.Log("Enter 1 controlNo is"+controlNo);
                    float posx = (float)operationResponse.Parameters[2];
                    float posy= (float)operationResponse.Parameters[3];
                    float posz= (float)operationResponse.Parameters[4];
                   // Debug.Log("pos is"+posx+" ,"+posy+" ,"+posz);
                    float endpointx= (float)operationResponse.Parameters[5];
                    float endpointy= (float)operationResponse.Parameters[6];
                    float endpointz= (float)operationResponse.Parameters[7];
                    //Debug.Log("endpoint is" + endpointx + " ," + endpointy + " ," + endpointz);
                   
                    manager.objArray[controlNo].transform.position = new Vector3(posx,posy,posz);
                    manager.guideArray[controlNo].setEndPoint(new Vector3(endpointx,endpointy,endpointz));

                    break;
                }
            case 2://創造角色   
                {
                   
                    int roleno = (int)operationResponse.Parameters[1];
                    Debug.Log("Enter 2 roleno is"+roleno);
                    bool mainRole = (bool)operationResponse.Parameters[2];
                    GameObject obj = Instantiate(roleTemple[0],manager.CreatPosArray[roleno], transform.rotation) as GameObject;
                    obj.transform.eulerAngles = Vector3.zero;
                    if (mainRole)
                    {
                        obj.AddComponent<MainRoleBehavior>().respAction = this;
                        obj.AddComponent<Hurt_Net>().client = client;
                        obj.GetComponent<arrow>().enabled = true;
                    }
                    manager.AddObj(obj); 
                    break;
                }
            case 3://攻擊-動作
                {
                    Debug.Log("Enter 3-attack");
                    int controlNo = (int)operationResponse.Parameters[1];
                    float posx = (float)operationResponse.Parameters[2];
                    float posy = (float)operationResponse.Parameters[3];
                    float posz = (float)operationResponse.Parameters[4];
                    float rotatx = (float)operationResponse.Parameters[5];
                    float rotaty = (float)operationResponse.Parameters[6];
                    float rotatz = (float)operationResponse.Parameters[7];
                    bool first = (bool)operationResponse.Parameters[8];
                    manager.objArray[controlNo].transform.position = new Vector3(posx, posy, posz);
                    manager.objArray[controlNo].transform.eulerAngles = new Vector3(rotatx,rotaty,rotatz);
                    if(first)
                        manager.actionArray[controlNo].attack_AI_player();
                    break;
                }
            case 4:
                {
                    Debug.Log("in 4 receive!");
                    int nowHp = (int)operationResponse.Parameters[1];
                    float stiffTime = (float)operationResponse.Parameters[2];
                    float convenslyTime = (float)operationResponse.Parameters[3];
                    int No = (int)operationResponse.Parameters[4];
                    manager.actionArray[No].Passive_CreateStiff(stiffTime);
                    manager.actionArray[No].Passive_CreateConversaly(convenslyTime);
                    break;
                }
            case 5:
                {
                    int No= (int)operationResponse.Parameters[1];
                    float x= (float)operationResponse.Parameters[2];
                    float z= (float)operationResponse.Parameters[3];
                    manager.objArray[No].transform.eulerAngles=new Vector3(x, transform.position.y, z);
                    break;
                }
        }
    }
}
