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


    public GameObject _TextNode;/// 生成する字幕Prefabのための変数宣言
    public GameObject _Text_parent;///textnodeの親要素用変数
    public Scrollbar _ScrollBar;

    Thread receiveThread;
    UdpClient client;
    int LOCAL_PORT = 50007;

    public Voicemanager _Voice;
    public ResponceMessageStorage RMStorage = new ResponceMessageStorage();
    private QuestionSceneVoiceStorage _QVstorage = new QuestionSceneVoiceStorage();

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

        // RMStorage の MessageAdded イベントにハンドラを登録
        RMStorage.MessageAdded += OnMessageAdded;///イベント発生したらOnMessageAddedを呼ぶ

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

                // JSONデータを解析
                MessageData messageData = JsonUtility.FromJson<MessageData>(json);

/*                Debug.Log("ID: " + messageData.id);
                Debug.Log("Content: " + messageData.content);
                Debug.Log("IsResponse: " + messageData.isResponse);*/

                // ここで受信したデータをもとにインターフェースの操作などを行う
                // メッセージをMessageStorageに保存
                RMStorage.AddMessage(messageData);

                // 受信したメッセージに基づいて実行したい処理をキューに追加
                mainThreadActions.Enqueue(() => Create_QuestionNode(messageData.content, messageData.isResponse));

                if (messageData.isResponse)
                {
                    IDsInSection.Add(messageData.id);

/*                    Task.Run(async () =>
                    {
                        Debug.Log("音声合成を開始: " + messageData.content);
                        try
                        {
                            var voice = await _Voice.CreateOneVoice(messageData.content);
                            UDPVStorage.StoreVoice(messageData.id, voice);
                            Debug.Log("受信した音声を合成しました: " + messageData.content);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("音声合成エラー: " + ex.Message);
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

    private async void OnMessageAdded(MessageData message)
    {
        // メッセージが追加されたときに呼ばれる関数
        Debug.Log("メッセージイベント発生！！！！！！！！！");
        try
        {
            Voice voice = await _Voice.CreateOneVoice(message.content);
            _QVstorage.StoreResponceVoice(message.id, voice);
            Debug.Log("メッセージイベントから保存完了！！！！！！！！！");
        }
        catch (Exception ex)
        {
            Debug.LogError("音声生成中にエラーが発生しました: " + ex.Message);
        }
    }

    public void StartTrackingSection()
    {
        IDsInSection.Clear();
    }

    public int RandomID_ThisSection()
    {
        List<int> list = new List<int>(IDsInSection);
        // ここで currentSectionMessages には、現在のセクション中に受け取ったメッセージが含まれています
        Debug.Log(IDsInSection);
        int randomIndex = IDsInSection[UnityEngine.Random.Range(0, list.Count)];
        return randomIndex;
    }

    ///Udpから送られたテキストをIDから取得
    public string GetMessage(int messageId, bool isResponse)
    {
        List<MessageData> messages = RMStorage.GetMessagesById(messageId);
        foreach (var message in messages)
        {
            if (message.isResponse == isResponse)
            {
                // 指定されたタイプ（質問または回答）のメッセージのみを表示
                //Debug.Log(message.content);
                return message.content;
            }
        }
        // 条件に一致するメッセージがない場合は空の文字列またはデフォルト値を返す
        return "";
    }

    private void Create_QuestionNode(string text,bool isResponse)
    {
        if (isResponse == false)
        {
            GameObject NodePrefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab生成
            Transform parent01Transform = NodePrefab.transform;///生成したPrefabはTextの親要素なので、親のTransformとして取得
            Transform parent02Transform = parent01Transform.Find("ChatBoard");
            Transform childTransform = parent02Transform.Find("ChatText");

            TextMeshProUGUI _Node = childTransform.GetComponent<TextMeshProUGUI>();

            string addtext = text.Replace("「", "").Replace("」", "").Replace("\\n", "");

            // addtextが空でないかチェックしてから追加
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

