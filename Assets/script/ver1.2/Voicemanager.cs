using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Voicemanager : MonoBehaviour
{
    [SerializeField] GameObject bottun;

    [SerializeField] VOICEVOX voicevox;///VOICEVOXスクリプトアタッチしたオブジェクトを入れろ
    int speaker = 20;　//もち子さん
    
    void Start()
    {

    }

    public async Task<List<Voice>> CreateVoiceDate(int num)
    {///voicevoxで音声合成をする(リストを返す)

/*        Debug.Log("音声合成開始");*/
        List<Voice> voicelist = new List<Voice>();

        JSONmanager _jsonManager = GameObject.FindObjectOfType<JSONmanager>();



        // _jsonManager.lessons が null でなくなるまで待機
        while (_jsonManager.lessons == null)
        {
            await Task.Delay(100); // 100ミリ秒待機して再度チェック
/*            Debug.Log("音声合成待機中");*/
        }

        // 他のスクリプトから JSONmanager インスタンスの Lessons プロパティにアクセス
        string[] lessons = _jsonManager.lessons;


        string lessonText = lessons[num]; // 代入する対象の文字列
     
        if (lessonText != "")
        {
            int i = 0;
            string[] splitText = lessonText.Split(char.Parse("。"));///句読点で句切ってsplitTextに入れる
            do
            {                
                string addtext= splitText[i].Replace("「", "").Replace("」", "").Replace("\\n", "");

                // addtextが空でないかチェックしてから追加
                if (!string.IsNullOrEmpty(addtext))
                {
                    voicelist.Add(await voicevox.CreateVoice(speaker, addtext));
/*                    Debug.Log(addtext);*/
                }
                i++;
            }
            while (i < splitText.Length);
        }
        else
        {
            Debug.Log("エラー文があったため、スキップしました");
            num++;
        }
        /*        Debug.Log("セクションの音声合成完了");*/

        if (num == 0)
        {///最初の音声合成時にボタンをtrueにする
            bottun.SetActive(true);
        }

        return voicelist;
    }
}
