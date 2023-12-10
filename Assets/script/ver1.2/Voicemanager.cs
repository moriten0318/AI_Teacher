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
    private List<string> SplitText;
    [SerializeField] GameObject bottun;
    [SerializeField] VOICEVOX voicevox;///VOICEVOXスクリプトアタッチしたオブジェクトを入れろ
    public int speaker = 20;　//もち子さん


    
    void Start()
    {

    }

    public async Task<List<Voice>> CreateVoiceDate(int index,List<string> SplitText)
    {///voicevoxで音声合成をする(リストを返す)

        List<Voice> voicelist = new List<Voice>();
        int i = 0;
        do
        {
            string addtext = SplitText[i].Replace("「", "").Replace("」", "").Replace("\\n", "");

            // addtextが空でないかチェックしてから追加
            if (!string.IsNullOrEmpty(addtext))
            {
                voicelist.Add(await voicevox.CreateVoice(speaker, addtext));
                //Debug.Log(addtext);
            }
            i++;
        }
        while (i < SplitText.Count);

        if (index == 0)
        {///最初の音声合成時にボタンをtrueにする
            bottun.SetActive(true);
        }

        return voicelist;
    }

    public async Task<Voice> CreateOneVoice(string Text)
    {///voicevoxで音声合成をする

        Voice voice = null; ;
        string addtext = Text.Replace("「", "").Replace("」", "").Replace("\\n", "");

        // addtextが空でないかチェックしてから追加
        if (!string.IsNullOrEmpty(addtext))
        {
            voice = await voicevox.CreateVoice(speaker, addtext);
            Debug.Log($"音声合成結果: {voice != null}");
        }

        else
        {
            Debug.Log("addtextが空");
        }

        return voice;
    }
}
