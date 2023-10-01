using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class UDPReceiver : MonoBehaviour
{

    private static UdpClient udp;
    private IPEndPoint remoteEP;

    // Start is called before the first frame update
    void Start()
    {
        int LOCAL_PORT = 50007;
        udp = new UdpClient(LOCAL_PORT);
        udp.Client.ReceiveTimeout = 100000;
        ListenForUDPMessage();
    }

    async void ListenForUDPMessage()
    {
        while (true)
        {
            UdpReceiveResult result = await udp.ReceiveAsync();
            byte[] data = result.Buffer;
            string text = Encoding.UTF8.GetString(data);
            Debug.Log(text);
        }

    }
}
