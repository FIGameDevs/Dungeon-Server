using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecvTest
{

    [MsgId(7)]
    public static void ReceiveInt(int num)
    {
        Utils.EditorLog(num);
    }
    [MsgId(9)]
    public static void ComplexRecv(int conId, string text, bool pravda)
    {
        Utils.EditorLog(conId + ", " + text + ", " + pravda);
    }

    [MsgId(25)]
    public static void StringRecv(int conId, string text)
    {
        Utils.EditorLog(conId + ", " + text);
    }
}
