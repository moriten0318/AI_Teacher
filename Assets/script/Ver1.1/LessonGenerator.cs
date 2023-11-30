using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;
using System.Text.RegularExpressions;

public class LessonGenerator : MonoBehaviour
{
    [SerializeField] public TextAsset textAsset;    //　読む込むテキストが書き込まれている.txtファイル

    public string loadText;    //　テキストファイルから読み込んだデータ
    public string[] splitText;    //　改行で分割して配列に入れる
    public int textNum;    ///　現在表示中テキスト番号(他のScriptでも使うから変にいじらないこと！)
    private int imgnum;
    public string addtext;

    [SerializeField] VOICEVOX voicevox;///VOICEVOXスクリプトアタッチしたオブジェクトを入れろ
    int speaker = 20;　//もち子さん
    public List<Voice> _voicelist = new List<Voice>();
    [SerializeField] GameObject bottun;
    [SerializeField] GameObject _loading;

    

    void Start()
    {


    }

    public void LessonGenerate(TextAsset textfile)
    {
        _loading.gameObject.SetActive(true);///Loading表示する

        loadText = textfile.text;////指定したテキストアセットをloadTextに入れる
        splitText = loadText.Split(char.Parse("。"));///句読点で区切って配列型splitTextに入れる
        textNum = 0;
        imgnum = 1;

        CreateVoiceDate();
    }



    private async void CreateVoiceDate()
    {///voicevoxで音声合成をする
        int num = 0;
            do
            {
                string n = imgnum.ToString();
                string checktext = string.Format("image{0}", n);
                bool contains_imgnum = splitText[num].Contains(checktext);

                if (splitText[num] != "")
                {
                    addtext = splitText[num];
                    if (contains_imgnum)
                    {///image～がテキスト内に含まれていればその部分を削除してVOICEVOXに渡す              
                    addtext = Regex.Replace(splitText[num], checktext, "");
                    imgnum++;
                    }

                    _voicelist.Add(await voicevox.CreateVoice(speaker,addtext));
                    //Debug.Log("音声合成完了:" + splitText[num]);
                    num++;
                    if (num == 1)
                    {///最初の音声合成時にボタンをtrueにする
                    bottun.SetActive(true);
                    Destroy(_loading);
                    }
                }
                else
                {
                    Debug.Log("エラー文があったため、スキップしました");
                    num++;
                }
            }
            while (num < splitText.Length);
        Debug.Log("全ての音声合成完了");
    }


}
