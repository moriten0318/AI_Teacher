using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;
using System.Threading.Tasks;

public class MainManager : MonoBehaviour
{

    public JSONmanager _JSON;
    public Voicemanager _Voice;
    public BlackBoardManager _BB;
    public Receiver _receiver;

    private LessonDataStorage _storage = new LessonDataStorage();
    private VoiceDataStorage _Vstorage = new VoiceDataStorage();
    private QuestionSceneVoiceStorage _QVstorage = new QuestionSceneVoiceStorage();

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
        // fQuestionIntrotext のすべての要素に対して処理を行う
        for (int i = 0; i < _QVstorage.IntroText.Length; i++)
        {
            string txt = _QVstorage.IntroText[i];
            // 音声を生成し、保存
            Voice voice = await _Voice.CreateOneVoice(txt);
            _QVstorage.StoreIntroVoice(voice);
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
            index++;
        }

        Debug.Log("音声全合成完了");
    }




    public async Task play_section(int index)
    {///１セクションを再生するループ

       
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


    /// <summary>
    /// 回答文をSplitする　序文を再生する
    /// </summary>
    /// <returns></returns>
    private async Task QuestionScene()
    {

        for (int i = 0; i < 3; i++)
        {
            int n = _receiver.RandomID_ThisSection();
            string responstxt = _receiver.GetMessage(n, true);
            Debug.Log(responstxt);

            if (!string.IsNullOrEmpty(responstxt))
            {
                Create_captions(responstxt);
                await voicevox.PlayOneShot(_Voice.speaker, responstxt);
            }
            else
            {
                Debug.LogError("Voice is null for ID: " + n);
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


