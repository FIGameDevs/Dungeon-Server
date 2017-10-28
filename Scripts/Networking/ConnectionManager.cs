using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConnectionManager
{
    BaseServer server;
    public HashSet<int> clients = new HashSet<int>();

    Action<int> onConnectedCallback;
    Action<int> onDisconnectedCallback;

    public ConnectionManager(BaseServer s)
    {
        if (s == null)
            throw new System.Exception("BaseServer argument cannot be null.");
        server = s;
        server.SetCallbacks(ClientConnected, MessageDelegator.DelegateMSG, ClientDisconnected);
    }
    public void ClientConnected(int connectionId)
    {
        if (clients.Contains(connectionId))
        {
            Utils.EditorLog("Client with this id already connected!");
            return;
        }
        clients.Add(connectionId);
        if (onConnectedCallback != null)
            onConnectedCallback(connectionId);
    }
    //Use the array instantly, it will be reused


    public void ClientDisconnected(int connectionId)
    {
        if (!clients.Contains(connectionId))
        {
            Utils.EditorLog("Trying to disconnect unconnected client id?");
            return;
        }
        clients.Remove(connectionId);
        if (onDisconnectedCallback != null)
            onDisconnectedCallback(connectionId);
    }

    public void CheckIncomingMessages()
    {
        server.CheckIncoming();
    }
}
