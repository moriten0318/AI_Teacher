using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using VoicevoxBridge;
using System.Threading.Tasks;
using System.Threading;


[System.Serializable]
public class MessageData
{
    public int id;
    public string content;
    public bool isResponse;
}

public class Receiver : MonoBehaviour
{

    private static UdpClient udp;
    private IPEndPoint remoteEP;
    public main main;
    [SerializeField] VOICEVOX voicevox;

    public List<string> question = new List<string>();
    public List<string> answer = new List<string>();
    public List<Voice> question_voicelist = new List<Voice>();
    public List<Voice> answer_voicelist = new List<Voice>();


    Thread receiveThread;
    UdpClient client;
    int LOCAL_PORT = 50007;

    private MessageStorage messageStorage = new MessageStorage();

    // Start is called before the first frame update
    void Start()
    {

        /*        udp = new UdpClient(LOCAL_PORT);
                ListenForUDPMessage();*/

        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    private void ReceiveData()
    {
        client = new UdpClient(LOCAL_PORT);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string json = Encoding.UTF8.GetString(data);

                // JSON�f�[�^�����
                MessageData messageData = JsonUtility.FromJson<MessageData>(json);
                Debug.Log("ID: " + messageData.id);
                Debug.Log("Content: " + messageData.content);
                Debug.Log("IsResponse: " + messageData.isResponse);

                // �����Ŏ�M�����f�[�^�����ƂɃC���^�[�t�F�[�X�̑���Ȃǂ��s��
                // ���b�Z�[�W��MessageStorage�ɕۑ�
                messageStorage.AddMessage(messageData);
            }
            catch (System.Exception err)
            {
                Debug.LogError(err.ToString());
            }
        }
    }

    void OnApplicationQuit()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        if (client != null)
            client.Close();
    }

    public string GetMessage(int messageId, bool isResponse)
    {
        List<MessageData> messages = messageStorage.GetMessagesById(messageId);
        foreach (var message in messages)
        {
            if (message.isResponse == isResponse)
            {
                // �w�肳�ꂽ�^�C�v�i����܂��͉񓚁j�̃��b�Z�[�W�݂̂�\��
                Debug.Log(message.content);
                return message.content;
            }
        }
        // �����Ɉ�v���郁�b�Z�[�W���Ȃ��ꍇ�͋�̕�����܂��̓f�t�H���g�l��Ԃ�
        return "";
    }


    /*    async void ListenForUDPMessage()
        {
            while (true)
            {
                UdpReceiveResult result = await udp.ReceiveAsync();
                byte[] data = result.Buffer;
                string text = Encoding.UTF8.GetString(data);
                Debug.Log(text);

                string[] list = text.Split(char.Parse("@"));
                question.Add(list[0]);
                answer.Add(list[1]);
                question_voicelist.Add(await voicevox.CreateVoice(20, list[0]));
                answer_voicelist.Add(await voicevox.CreateVoice(20, list[1]));
                Debug.Log("UDP��M�A��");

            }
        }*/
}

