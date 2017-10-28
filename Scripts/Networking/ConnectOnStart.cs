using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BaseServerFront))]
public class ConnectOnStart : MonoBehaviour {
    [SerializeField]
    string ip;
    [SerializeField]
    int port;
	// Use this for initialization
	void Start () {
        GetComponent<BaseServerFront>().Connect(ip, port);
        MessageDelegator.Send(GetComponent<BaseServerFront>().GetConnectionId(), UnityEngine.Networking.QosType.Reliable, 9, 1900.B(), "Like a plastic bag?".B(), false.B()); //ošetřit aby první parametr byl vždy ushort
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
