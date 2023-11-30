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

    [SerializeField] VOICEVOX voicevox;///VOICEVOXスクリプトアタッチしたオブジェクトを入れろ

    public GameObject _TextNode;/// 生成する字幕Prefabのための変数宣言
    public GameObject _Text_parent;///textnodeの親要素用変数

    public List<Texture2D> textureList;///スライド画像リスト

    public string[] lessons;///セクションごとの教師の発話の配列(1要素が1セクション)
    private string[] SplitText;///句読点で分割したもの
    private string[] tempSplitText;///句読点で分割したもの

    private List<Voice> voicelist = new List<Voice>();///再生する音声リスト
    private List<Voice> tempVoices = new List<Voice>();///次のセクションの音声リスト

    private int CurrentSectionIndex=1;

    public Scrollbar _ScrollBar;

    void Start()
    {
        prepare_speech(0);
        _ScrollBar = _ScrollBar.GetComponent<Scrollbar>();
        _ScrollBar.value = 1;
    }

    void Update()
    {
        _ScrollBar.value = 0;
    }

    async void prepare_speech(int index)
    {
        tempVoices = await _Voice.CreateVoiceDate(index);
        tempSplitText = lessons[index].Split(char.Parse("。"));///句読点で句切ってsplitTextに入れる
    }

    public async void play_section(int CurrentSectionIndex)
    {///１セクションを再生するループ
        int num = 0;
        SplitText = tempSplitText;
        voicelist = tempVoices;

        prepare_speech(CurrentSectionIndex);

        while (num < voicelist.Count)
        {
            _BB.UpdateBlackBoard(CurrentSectionIndex+1);///スライドの更新
            Create_captions(SplitText[num]);///字幕生成
            await voicevox.Play(voicelist[num]);///音声再生
            num++;
        }

    }


    private void Create_captions(string text)
    {

        foreach (string lesson in lessons)
        {
            Debug.Log(lesson);
        }

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
}


