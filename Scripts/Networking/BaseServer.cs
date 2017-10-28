using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class BaseServer
{

    // https://docs.unity3d.com/ScriptReference/Networking.QosType.html
    int[] qosChannels;
    //server host id in case of more separate hosts
    int hostId;
    int conId; //set when conencted to server
    public int ConId { get { return conId; } }
    Action<int> onConnectCallback;
    Action<int, byte[], int> onDataRecCallback;
    Action<int> onDisconnectCallback;

    public BaseServer(string ip = "localhost", int port = 3333, int maxConnections = 10, byte workerThreads = 1)
    {
        try
        {
            GlobalConfig gConfig = new GlobalConfig();
            gConfig.ThreadPoolSize = workerThreads;
            NetworkTransport.Init(gConfig);
            Utils.EditorLog("Server initialized.");

            var cConfig = InitQosChannels();
            HostTopology topology = new HostTopology(cConfig, maxConnections);
            hostId = NetworkTransport.AddHost(topology, port, ip);
            Utils.EditorLog("Server started.");
        }
        catch (System.Exception ex)
        {
            FileLogger.Log("Error occured in BaseServer: " + ex.Message);
            Utils.EditorLog(ex.Message);
        }

    }
    public void SetCallbacks(Action<int> onConnect, Action<int, byte[], int> onDataRec, Action<int> onDisconnect)
    {
        if (onConnect != null)
            onConnectCallback = onConnect;
        if (onDataRec != null)
            onDataRecCallback = onDataRec;
        if (onDisconnect != null)
            onDisconnectCallback = onDisconnect;
    }

    ConnectionConfig InitQosChannels()
    {
        ConnectionConfig cConfig = new ConnectionConfig();
        qosChannels = new int[9];

        FileLogger.Log("QoS Channels:");
        for (int i = 0; i < 9; i++)
        {
            FileLogger.Log(i + ": " + ((QosType)i).ToString());

            qosChannels[i] = cConfig.AddChannel((QosType)i);
        }
        return cConfig;
    }

    public int GetQosChannelId(QosType qType)
    {
        return qosChannels[(int)qType];
    }

    public bool Send(QosType qos,int connectionId, byte[] buffer, int bufferLength)
    {
        byte error = 0;
        NetworkTransport.Send(hostId, connectionId, qosChannels[(int)qos], buffer, bufferLength, out error);

        if ((NetworkError)error != NetworkError.Ok)
        {
            FileLogger.Log("Error while connecting: " + ((NetworkError)error).ToString());
            return false;
        }
        return true;
    }

    public bool Connect(string ip, int port)
    {
        byte error = 0;
        conId = NetworkTransport.Connect(hostId, ip, port, 0, out error);

        if ((NetworkError)error != NetworkError.Ok)
        {
            FileLogger.Log("Error while connecting: " + ((NetworkError)error).ToString());
            return false;
        }
        return true;

    }

    public bool Disconnect()
    {
        byte error = 0;
        NetworkTransport.Disconnect(hostId, conId, out error);
        if ((NetworkError)error != NetworkError.Ok)
        {
            FileLogger.Log("Error while connecting: " + ((NetworkError)error).ToString());
            return false;
        }
        return true;
    }

    byte[] recBuffer = new byte[2048];
    public void CheckIncoming()
    {
        int connectionId;
        int channelId;
        int bufferSize = 2048;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.ReceiveFromHost(hostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        if ((NetworkError)error != NetworkError.Ok)
        {
            FileLogger.Log("Error while receiving data: " + ((NetworkError)error).ToString());
            return;
        }

        switch (recData)
        {
            case NetworkEventType.Nothing:         //1
                break;
            case NetworkEventType.ConnectEvent:    //2
                Utils.EditorLog("Somebody connected!");
                onConnectCallback(connectionId);
                break;
            case NetworkEventType.DataEvent:       //3
                //Utils.EditorLog("Wow received data! " + System.Convert.ToBase64String(recBuffer, 0, dataSize));
                onDataRecCallback(connectionId, recBuffer, dataSize);
                break;
            case NetworkEventType.DisconnectEvent: //4
                Utils.EditorLog("Somebody disconnected! :(");
                onDisconnectCallback(connectionId);
                break;
        }
    }

    public bool Kick(int connectionId)
    {
        byte error = 0;
        NetworkTransport.Disconnect(hostId, connectionId, out error);
        if ((NetworkError)error != NetworkError.Ok)
        {
            FileLogger.Log("Error while connecting: " + ((NetworkError)error).ToString());
            return false;
        }
        return true;
    }
}
