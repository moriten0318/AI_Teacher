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
    public UDPReceiver UDP;

    public bool Qflag;
    private bool mainflag;
    private int voicenum;
    private int textnum;
    private int questionnum;

    public AudioClip[] soundFiles;  // 音声ファイルを格納する配列
    public AudioClip instance01;
    public AudioClip instance02;
    public AudioClip instance03;
    public AudioSource audioSource;  // 音声再生用のAudioSourceコンポーネント(VOICEVOXを指定する)

    void Start()
    {
        BB = GameObject.Find("BlackBoardScript").GetComponent<Blackboard>();
        LG = GameObject.Find("LessonGeneratorScript").GetComponent<LessonGenerator>();
        audioSource = GetComponent<AudioSource>();
        soundFiles = new AudioClip[] { instance01, instance02, instance03 };

        mainflag = true;
        Qflag = false;
        voicenum = 0;
        textnum = 0;
        questionnum = 0;

    }

    public async void play_lesson()
    {
        await WaitForFlag();

        while (mainflag)
        {
            ///質問が来てQflagがtrueになったら処理する
            if (Qflag ==true)
            {
                Answer();
            }
            else
            {
                BB.Text_Explain(textnum);
                await voicevox.Play(LG._voicelist[voicenum]);
                textnum++;
                voicenum++;
                await Task.Delay(2500);///ちょっと待機

                if (LG._voicelist[voicenum] == null)
                {
                    mainflag = false;
                }
                if (textnum >= LG.splitText.Length)
                {
                    mainflag = false;
                }
            }


            

        }
    }

    public async Task WaitForFlag()
    {
        while (!mainflag)
        {
            await Task.Delay(100); // 100ミリ秒待機
        }
    }

    public async void Answer()
    {

        Qflag = false;
        // 受け取った時の音声ファイルをランダム再生
        int randomIndex = Random.Range(0, soundFiles.Length);
        AudioClip randomSound = soundFiles[randomIndex];
        audioSource.clip = randomSound;
        audioSource.Play();

        await Task.Delay(2000);///ちょっと待機
        BB.Create_QuestionNode(UDP.question[questionnum]);
        BB.Create_AnswerNode(UDP.answer[questionnum]);
        await voicevox.PlayOneShot(20,UDP.answer[questionnum]);
        questionnum++;

    }

}
