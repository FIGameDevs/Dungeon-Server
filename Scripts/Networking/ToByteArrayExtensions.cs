using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;

public static class ToByteArrayExtensions
{
    public static byte[] B(this int num)
    {
        return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(num));
    }
    public static byte[] B(this byte num)
    {
        return BitConverter.GetBytes(num);
    }
    public static byte[] B(this float num)
    {
        return BitConverter.GetBytes(num);
    }
    public static byte[] B(this double num)
    {
        return BitConverter.GetBytes(num);
    }
    public static byte[] B(this short num)
    {
        return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(num));
    }
    public static byte[] B(this ushort num)
    {
        return BitConverter.GetBytes(num);
    }
    public static byte[] B(this uint num)
    {
        return BitConverter.GetBytes(num);
    }
    public static byte[] B(this bool num)
    {
        return BitConverter.GetBytes(num);
    }
    public static byte[] B(this long num)
    {
        return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(num));
    }
    public static byte[] B(this ulong num)
    {
        return BitConverter.GetBytes(num);
    }
    public static byte[] B(this char num)
    {
        return BitConverter.GetBytes(num);
    }
    public static byte[] B(this string num)
    {
        byte[] count = System.Text.Encoding.UTF8.GetByteCount(num).B();
        byte[] text = System.Text.Encoding.UTF8.GetBytes(num);
        byte[] b = new byte[count.Length + text.Length];
        for (int i = 0; i < count.Length; i++)
        {
            b[i] = count[i];
        }
        for (int i = 0; i < text.Length; i++)
        {
            b[i + count.Length] = text[i];
        }
        return b;
    }
}