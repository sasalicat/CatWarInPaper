using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using System;
using System.Security;
using System.Reflection;

public class PHotonClient : MonoBehaviour, IPhotonPeerListener
{
    public string ServerAddress = "140.134.27.115:5055";
    protected string ServerApplication = "catServer";

    protected PhotonPeer peer;
    public Manager_Photon manager;
    // Use this for initialization
    void Start()
    {
        this.peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        this.Connect();
    }
    internal virtual void Connect()
    {
        try
        {
            this.peer.Connect(this.ServerAddress, this.ServerApplication);
        }
        catch (SecurityException se)
        {
            this.DebugReturn(0, "Connect Failed" + se.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.peer.Service();
    }

    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.OperationCode) {

            case 1://移動
                {
                    int controlNo = (int)operationResponse.Parameters[1];
                    byte button = (byte)operationResponse.Parameters[2];
                    switch ((char)button)
                    {
                        case 'w': {
                                manager.actionArray[controlNo].move_AI_player(new Vector3(0, 0, 1));
                                break;
                        }
                        case 'a':
                            {
                                manager.actionArray[controlNo].move_AI_player(new Vector3(-1, 0, 0));
                                break;
                            }
                        case 's':
                            {
                                manager.actionArray[controlNo].move_AI_player(new Vector3(0, 0, -1));
                                break;
                            }
                        case 'd':
                            {
                                manager.actionArray[controlNo].move_AI_player(new Vector3(1, 0, 0));
                                break;
                            }

                    }
                    
                    break;
                }
        }

    }

    public void OnStatusChanged(StatusCode statusCode)
    {

    }

    public void OnEvent(EventData eventData)
    {

    }
    public void Go(char button,int no)
    {
        var parameter = new Dictionary<byte, object>()
                    {
                         {(byte)1,no },
                        {(byte)2,(byte)button }
                    };
        this.peer.OpCustom(1, parameter, true);
    }
}
