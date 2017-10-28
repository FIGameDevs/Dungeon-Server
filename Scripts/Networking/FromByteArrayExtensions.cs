using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;

public static class FromByteArrayExtensions
{

    public static int Int(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(int);
        return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(val, ind - sizeof(int)));
    }
    public static byte Byte(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(byte);
        return val[ind - sizeof(byte)];
    }
    public static float Float(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(float);
        return BitConverter.ToSingle(val, ind - sizeof(float));
    }
    public static double Double(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(double);
        return BitConverter.ToDouble(val, ind - sizeof(double));
    }
    public static short Short(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(short);
        return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(val, ind - sizeof(short)));
    }
    public static ushort Ushort(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(ushort);
        return BitConverter.ToUInt16(val, ind - sizeof(ushort));
    }
    public static uint Uint(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(uint);
        return BitConverter.ToUInt32(val, ind - sizeof(uint));
    }
    public static bool Bool(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(bool);
        return BitConverter.ToBoolean(val, ind - sizeof(bool));
    }
    public static long Long(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(long);
        return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(val, ind - sizeof(long)));
    }
    public static ulong Ulong(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(ulong);
        return BitConverter.ToUInt64(val, ind - sizeof(ulong));
    }
    public static char Char(/*this*/ byte[] val, ref int ind)
    {
        ind += sizeof(char);
        return BitConverter.ToChar(val, ind - sizeof(char));
    }
    public static string String(/*this*/ byte[] val, ref int ind)
    {
        var count = Int(val, ref ind);
        var s =  System.Text.Encoding.UTF8.GetString(val, ind, count);
        ind += System.Text.Encoding.UTF8.GetByteCount(s);
        return s;
    }
}
