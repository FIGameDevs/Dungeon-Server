using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;

public static class MessageDelegator
{
    static BaseServer server;
    public static void SetSendingServer(BaseServer s)
    {
        server = s;
    }
    static IMessageSwitcher switcher;
    public static void SetSwitcher(IMessageSwitcher s)
    {
        switcher = s;
    }
    public static void DelegateMSG(int connectionId, byte[] msg, int length)
    {
        if (switcher != null)
            switcher.SwitchMessage(connectionId, msg);//length missing but it shouldn't matter riiight?
    }

    public static bool Send(int connectionId, UnityEngine.Networking.QosType qos, ushort msgId, params byte[][] items)
    {
        var newItems = new byte[items.Length + 1][];
        newItems[0] = msgId.B();
        for (int i = 0; i < items.Length; i++)
        {
            newItems[i + 1] = items[i];
        }

        return SendInternal(connectionId, qos, newItems);
    }

    //Types can be cast to byte[] by using .B() method, example:
    //Send(QosType.Reliable, connectionId, 25.B(), maxMana.B(), isAlive.B(), canMove.B())
    //Note that you must use unique message id(ushort) as third parameter
    /// <summary>
    /// Used to send data to clients, use .B() method on your variables to convert them.
    /// </summary>
    /// <param name="connectionId">Id of connected client.</param>
    /// <param name="qos">Different Sending modes.</param>
    /// <param name="items">Write ushort msg id matching with client, then other data.</param>
    /// <returns></returns>
    public static bool SendInternal(int connectionId, UnityEngine.Networking.QosType qos, params byte[][] items)//TODO message types(ushort?)
    {
        if (server == null)
        {
            Utils.EditorLog("Sending server not set, cannot send a message.");
            return false;
        }
        int l = 0;
        for (int i = 0; i < items.Length; i++)
        {
            l += items[i].Length;
        }
        byte[] msg = new byte[l];
        int offset = 0;
        for (int i = 0; i < items.Length; i++)
        {
            Buffer.BlockCopy(items[i], 0, msg, offset, items[i].Length);
            offset += items[i].Length;
        }

        return server.Send(qos, connectionId, msg, msg.Length);
    }
}
