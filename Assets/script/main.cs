using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;
using System.Threading.Tasks;

public class main : MonoBehaviour
{
    [SerializeField] VOICEVOX voicevox;///VOICEVOXスクリプトアタッチしたオブジェクトを入れろ
    
    public Blackboard BB;
    public LessonGenerator LG;
    public bool flag;
    public int num;

    void Start()
    {
        BB = GameObject.Find("BlackBoardScript").GetComponent<Blackboard>();
        LG = GameObject.Find("LessonGeneratorScript").GetComponent<LessonGenerator>();
        flag = LG.flag;
        num = LG.textNum;
    }

    public async void play_lesson()
    {
        await WaitForFlag();
        while (flag)
        {
            ///flagがtrueになったら処理する
            BB.Text_Explain();
            await voicevox.Play(LG._voicelist[num]);
            num++;

            if (LG._voicelist[num] == null) 
            {
                flag = false;
            }

        }
    }

    public async Task WaitForFlag()
    {
        while (!flag)
        {
            await Task.Delay(100); // 100ミリ秒待機
        }
    }


}
