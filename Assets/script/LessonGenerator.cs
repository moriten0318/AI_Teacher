using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;

public class LessonGenerator : MonoBehaviour
{
    [SerializeField] public TextAsset textAsset;    //　読む込むテキストが書き込まれている.txtファイル
    public string loadText;    //　テキストファイルから読み込んだデータ
    public string[] splitText;    //　改行で分割して配列に入れる
    public int textNum;    ///　現在表示中テキスト1番号

    [SerializeField] VOICEVOX voicevox;///VOICEVOXスクリプトアタッチしたオブジェクトを入れろ
    int speaker = 20;　//もち子さん
    public List<Voice> _voicelist = new List<Voice>();

    bool waiting_flag = true;///trueだったら進んでよし

    void Start()
    {
        loadText = textAsset.text;////指定したテキストアセットをloadText1に入れる
        splitText = loadText.Split(char.Parse("。"));///改行で区切って配列型splitTextに入れる
		textNum = 0;
        CreateVoiceDate();

    }

    public async void CreateVoiceDate()
    {
        int num = 0;
        string text = "ずんだもんなのだ";
        Voice voice = await voicevox.CreateVoice(speaker, text);
            do
            {
                if (splitText[num] != "")
                {
                    _voicelist.Add(await voicevox.CreateVoice(speaker, splitText[num]));
                    Debug.Log("音声合成完了:" + splitText[num]);
                    num++;
                }
                else
                {
                    Debug.Log("エラー文があったため、スキップしました");
                    num++;
                }
            }
            while (num < splitText.Length);
        Debug.Log("全文の音声合成完了");
        await voicevox.Play(_voicelist[0]);
        await voicevox.Play(_voicelist[6]);///テスト

    }

}
