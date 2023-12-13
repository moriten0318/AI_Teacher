using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;
using System.Threading.Tasks;
using System.Linq;

public class MainManager : MonoBehaviour
{

    public JSONmanager _JSON;
    public Voicemanager _Voice;
    public BlackBoardManager _BB;
    public Receiver _receiver;
    [SerializeField] private ButtonClick bottun;
    [SerializeField] private Blackboard _header;

    private LessonDataStorage _storage = new LessonDataStorage();
    private VoiceDataStorage _Vstorage = new VoiceDataStorage();
    public ResponceMessageStorage _RMStorage = new ResponceMessageStorage();
    public ResponceVoiceStorage _RVStorage = new ResponceVoiceStorage();

    [SerializeField] VOICEVOX voicevox;///VOICEVOXスクリプトアタッチしたオブジェクトを入れろ

    public GameObject _TextNode;/// 生成する字幕Prefabのための変数宣言
    public GameObject _Text_parent;///textnodeの親要素用変数
    public Scrollbar _ScrollBar;

    private List<Texture2D> textureList;///スライド画像リスト

    private int CurrentSectionIndex=0;///mainループを規定するインデックス
    //bool isPlayingSection = false;

   

    void Start()
    {
        textureList=_BB.GetTextureList();
        _storage.StoreSpeech(_JSON.LoadSpeech());

        Generate_AllSpeechVoice();///全発話の音声合成開始
        _ScrollBar = _ScrollBar.GetComponent<Scrollbar>();
        _ScrollBar.value = 1;
    }

    void Update()
    {
        _ScrollBar.value = 0;
    }

    async Task Generate_AllSpeechVoice()
    {
        // QuestionIntrotext のすべての要素に対して処理を行う
        for (int i = 0; i < _RVStorage.IntroText.Count; i++)
        {
            string txt = _RVStorage.IntroText[i];
            // 音声を生成し、保存
            Voice voice = await _Voice.CreateOneVoice(txt);
            _RVStorage.StoreIntroVoice(i,voice);
        }
        Debug.Log("質問コーナー導入音声合成完了");

        int index = 0;
        while (true)
        {
            // _storage.GetLessonData(index) が null ならループ終了
            var lessonData = _storage.GetLessonData(index);
            if (lessonData == null)
            {
                break;
            }

            // 音声を生成し、保存
            List<Voice> voices = await _Voice.CreateVoiceDate(index, lessonData);
            _Vstorage.StoreVoicelist(index, voices);
            // インデックスを増やす

            if (index == 0)
            {///最初の音声合成時にボタンをtrueにする
                bottun.BottunToActive();
            }

            index++;
        }

        Debug.Log("音声全合成完了");
    }




    public async Task play_section(int index)
    {///１セクションを再生するループ

        _header.Destroy_TextNode();
        List<string> SplitText = _storage.GetLessonData(index);
        List<Voice> CurrentVoices = _Vstorage.GetSectionVoiceList(index);

        ///セクションの発話内容をべらべら喋るよ
        int num = 0;
        while (num < CurrentVoices.Count)
        {
            _BB.UpdateBlackBoard(index+1, textureList);///スライドの更新(スライドは１枚分数がずれてるので注意!)
            Create_captions(SplitText[num]);///字幕生成
            await voicevox.Play(CurrentVoices[num]);///音声再生
            await Task.Delay(500);
            num++;


         ///ここまで
        }
        ///質問コーナー
        await QuestionScene();

        await Task.Delay(2500); // 5000ミリ秒待機
        Debug.Log("セクション終了");

        _receiver.StartTrackingSection();
    }


    private async Task QuestionScene()
    {
        _BB.UpdateBlackBoard(0, textureList);
        for (int i = 0; i < _RVStorage.IntroText.Count; i++)
        {
            Create_captions(_RVStorage.IntroText[i]);
            Voice v = _RVStorage.GetIntroVoice(i);
            await voicevox.Play(v, autoReleaseVoice: false); // 音声再生
        }

        for (int i = 0; i < 3; i++)
        {
            try
            {
                Debug.Log($"i={i}のレスポンスを返します");
                int n = _receiver.RandomID_ThisSection();
                Debug.Log($"RandomID_ThisSectionから返されたID: {n}");
                MessageData responceMessagedata = _RMStorage.GetMessageData(n, true);
                if (responceMessagedata == null)
                {
                    Debug.Log($"ID {n} のMessageDataが見つかりませんでした。");
                    continue;
                }

                string[] splittext = responceMessagedata.content.Split(char.Parse("。"));
                List<string> txtlist = splittext.ToList();
                Debug.Log($"分割されたテキストリストのサイズ: {txtlist.Count}");

                List<Voice> voicelist = _RVStorage.GetResponceVoice(n);
                if (voicelist == null || voicelist.Count == 0)
                {
                    Debug.Log($"ID {n} のVoiceリストが不完全、または見つかりませんでした。");
                    continue;
                }
                Debug.Log($"取得されたVoiceリストのサイズ: {voicelist.Count}");
                // テキストリストと音声リストの要素数が異なる場合は調整する
                int minCount = Math.Min(txtlist.Count, voicelist.Count);

                ///質問文を表示する
                MessageData Question = _RMStorage.GetMessageData(n, false);
                _header.Create_TextNode(Question.content);
                ///質問への解答を表示、再生する
                for (int j = 0; j < minCount; j++)
                {
                    Debug.Log($"テキスト: {txtlist[j]}, Voiceインデックス: {j}");
                    Create_captions(txtlist[j]); // 字幕生成
                    await voicevox.Play(voicelist[j], autoReleaseVoice: false); // 音声再生
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"QuestionSceneでエラーが発生したため強制移行します: {ex.Message}");
                // ここで例外が発生した場合、ループを終了して次のセクションに移行
                return;
            }
        }
    }

    private void Create_captions(string text)
    {
        Destroy_Node();
        GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab生成
        Transform parent01Transform = instantiatedNode_Prefab.transform;///生成したPrefabはTextの親要素なので、親のTransformとして取得
        Transform parent02Transform = parent01Transform.Find("Board");
        Transform childTransform = parent02Transform.Find("Text");

        TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();

        string addtext = text.Replace("「", "").Replace("」", "").Replace("\\n", "");

        // addtextが空でないかチェックしてから追加
        if (!string.IsNullOrEmpty(addtext))
        {
            _blackboard_Node.text = addtext;
        }       
    }
    private void Destroy_Node()
    {
        foreach (Transform child in _Text_parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Main()
    {
        Mainroop();
    }


    public async Task Mainroop()
    {
        while (CurrentSectionIndex < _JSON.jsondata.Length)
        {
            await play_section(CurrentSectionIndex);
            CurrentSectionIndex++;
        }
    }
}


