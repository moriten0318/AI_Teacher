using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using VoicevoxBridge;
using System.Threading.Tasks;
using System.Threading;
using TMPro;
using System.Linq;


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
    [SerializeField] VOICEVOX voicevox;

    public List<int> IDsInSection = new List<int>();


    public GameObject _TextNode;/// �������鎚��Prefab�̂��߂̕ϐ��錾
    public GameObject _Text_parent;///textnode�̐e�v�f�p�ϐ�
    public Scrollbar _ScrollBar;

    Thread receiveThread;
    UdpClient client;
    int LOCAL_PORT = 50007;

    public MainManager _main;

    private Queue<Action> mainThreadActions = new Queue<Action>();

    // Start is called before the first frame update
    void Start()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        _ScrollBar = _ScrollBar.GetComponent<Scrollbar>();
        _ScrollBar.value = 1;
        StartTrackingSection();

        // RMStorage �� MessageAdded �C�x���g�Ƀn���h����o�^
        _main._RMStorage.MessageAdded += OnMessageAdded;///�C�x���g����������OnMessageAdded���Ă�

    }

    void Update()
    {
        while (mainThreadActions.Count > 0)
        {
            _ScrollBar.value = 0;
            Action action = mainThreadActions.Dequeue();
            action?.Invoke();
        }
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

                /*                Debug.Log("ID: " + messageData.id);
                                Debug.Log("Content: " + messageData.content);
                                Debug.Log("IsResponse: " + messageData.isResponse);*/

                // �����Ŏ�M�����f�[�^�����ƂɃC���^�[�t�F�[�X�̑���Ȃǂ��s��
                // ���b�Z�[�W��MessageStorage�ɕۑ�
                _main._RMStorage.AddMessage(messageData);

                // ��M�������b�Z�[�W�Ɋ�Â��Ď��s�������������L���[�ɒǉ�
                mainThreadActions.Enqueue(() => Create_QuestionNode(messageData.content, messageData.isResponse));

                if (messageData.isResponse)
                {
                    IDsInSection.Add(messageData.id);

/*                    Task.Run(async () =>
                    {
                        Debug.Log("�����������J�n: " + messageData.content);
                        try
                        {
                            var voice = await _Voice.CreateOneVoice(messageData.content);
                            UDPVStorage.StoreVoice(messageData.id, voice);
                            Debug.Log("��M�����������������܂���: " + messageData.content);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("���������G���[: " + ex.Message);
                        }
                    });*/
                }

            }
            catch (System.Exception err)
            {
                Debug.LogError(err.ToString());
            }
        }
    }

    public async void OnMessageAdded(MessageData message)
    {
        // ���b�Z�[�W���ǉ����ꂽ�Ƃ��ɌĂ΂��֐�
        mainThreadActions.Enqueue(async () =>
        {
            //Debug.Log("���b�Z�[�W�C�x���g�����I�I�I�I�I�I�I�I�I");
            try
            {
                string[] splittext = message.content.Split(char.Parse("�B"));
                List<string> splitList = splittext.ToList();

                if (message.isResponse)
                {
                    List<Voice> voice = await _main._Voice.CreateVoiceDate(message.id, splitList);
                    _main._RVStorage.StoreResponceVoice(message.id, voice);
                    Debug.Log($"ID={message.id}�̉������b�Z�[�W�ۑ������I�I�I�I�I�I�I�I�I");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("�����������ɃG���[���������܂���: " + ex.Message);
            }
        });
    }

    public void StartTrackingSection()
    {
        IDsInSection.Clear();
    }

    public int RandomID_ThisSection()
    {
        List<int> list = new List<int>(IDsInSection);
        
        int randomIndex = IDsInSection[UnityEngine.Random.Range(0, list.Count)];
        Debug.Log(randomIndex);
        return randomIndex;
    }

/*    ///Udp���瑗��ꂽ�e�L�X�g��ID����擾
    public string GetMessage(int messageId, bool isResponse)
    {
        List<MessageData> messages = _main._RMStorage.GetMessagesById(messageId);
        foreach (var message in messages)
        {
            if (message.isResponse == isResponse)
            {
                // �w�肳�ꂽ�^�C�v�i����܂��͉񓚁j�̃��b�Z�[�W�݂̂�\��
                //Debug.Log(message.content);
                return message.content;
            }
        }
        // �����Ɉ�v���郁�b�Z�[�W���Ȃ��ꍇ�͋�̕�����܂��̓f�t�H���g�l��Ԃ�
        return "";
    }*/

    private void Create_QuestionNode(string text,bool isResponse)
    {
        if (isResponse == false)
        {
            GameObject NodePrefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab����
            Transform parent01Transform = NodePrefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
            Transform parent02Transform = parent01Transform.Find("ChatBoard");
            Transform childTransform = parent02Transform.Find("ChatText");

            TextMeshProUGUI _Node = childTransform.GetComponent<TextMeshProUGUI>();

            string addtext = text.Replace("�u", "").Replace("�v", "").Replace("\\n", "");

            // addtext����łȂ����`�F�b�N���Ă���ǉ�
            if (!string.IsNullOrEmpty(addtext))
            {
                _Node.text = addtext;
            }
        }

    }


    private void Destroy_Node()
    {
        foreach (Transform child in _Text_parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void OnApplicationQuit()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        if (client != null)
            client.Close();
    }


}

