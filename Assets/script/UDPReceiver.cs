using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using VoicevoxBridge;
using System.Threading.Tasks;

public class UDPReceiver : MonoBehaviour
{

    private static UdpClient udp;
    private IPEndPoint remoteEP;
    public main main;
    [SerializeField] VOICEVOX voicevox;

    public List<string> question = new List<string>();
    public List<string> answer = new List<string>();
    public List<Voice> question_voicelist = new List<Voice>();
    public List<Voice> answer_voicelist = new List<Voice>();



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

            string[] list = text.Split(char.Parse("@"));
            question.Add(list[0]);
            answer.Add(list[1]);
            question_voicelist.Add(await voicevox.CreateVoice(20,list[0]));
            answer_voicelist.Add(await voicevox.CreateVoice(20, list[1]));
            Debug.Log("UDPéÛêMÉAÉä");

        }

    }
}
