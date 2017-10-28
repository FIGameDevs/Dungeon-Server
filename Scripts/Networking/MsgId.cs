using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;

[AttributeUsage(AttributeTargets.Method,
                   AllowMultiple = true,
                   Inherited = true)]
public class MsgId : Attribute
{
    public ushort id;
    public MsgId(ushort recvId)
    {
        id = recvId;
    }
}

public interface IMessageSwitcher
{
    void SwitchMessage(int connectionId, byte[] msg);
}