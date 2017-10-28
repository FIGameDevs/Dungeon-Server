using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseServerFront : MonoBehaviour
{
    ConnectionManager conManager;
    BaseServer server;
    [SerializeField]
    string ip = "127.0.0.1";
    [SerializeField]
    int port = 3333;
    [SerializeField]
    int maxConnections = 10;
    [SerializeField]
    byte workerThreads = 1;


    void Awake()
    {
        server = new BaseServer(ip, port, maxConnections, workerThreads);

        conManager = new ConnectionManager(server);
        var assembly = System.Reflection.Assembly.LoadFile(Application.dataPath + "/StreamingAssets/NetCodeGen.dll");
        var t = assembly.GetType("IdSwitcher");//musím sehnat generovanou assembly, asi uložit cestu k ní or something
        MessageDelegator.SetSwitcher((IMessageSwitcher)System.Activator.CreateInstance(t));
        MessageDelegator.SetSendingServer(server);
        //test
        /* TIS JUST A TEST AND IT WURKS
        var items = new byte[][] {((ushort)9).B(), "Do you ever feel?".B(), true.B() };
        int l = 0;
        for (int i = 0; i < items.Length; i++)
        {
            l += items[i].Length;
        }
        byte[] msg = new byte[l];
        int offset = 0;
        for (int i = 0; i < items.Length; i++)
        {
            System.Buffer.BlockCopy(items[i], 0, msg, offset, items[i].Length);
            offset += items[i].Length;
        }
        int refer = 0;
        MessageDelegator.DelegateMSG(10, msg, l);*/

    }

    public void Connect(string ip, int port)
    {
        server.Connect(ip, port);
    }
    public int GetConnectionId() {
        return server.ConId;
    }

    // Update is called once per frame
    void Update()
    {
        conManager.CheckIncomingMessages();
    }

}
