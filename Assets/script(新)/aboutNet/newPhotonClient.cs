using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using System;
using System.Security;
using System.Reflection;
public class newPhotonClient : MonoBehaviour, IPhotonPeerListener
 {

    public string ServerAddress = "127.0.0.1:5055";
    protected string ServerApplication = "catServer";
    public responesAction respAction;
    public PhotonPeer peer;
   
    // Use this for initialization
    void Start()
    {
        respAction = GetComponent<responesAction>();
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
        respAction.OnClientRespone(operationResponse);

    }

    public void OnStatusChanged(StatusCode statusCode)
    {

    }

    public void OnEvent(EventData eventData)
    {

    }
   
}
