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

    public string[] UDPQuestion;
    public string[] randomcomment;
    public List<Voice>randamvoice = new List<Voice>();

    public AudioSource audioSource;  // 音声再生用のAudioSourceコンポーネント(VOICEVOXを指定する)

    void Start()
    {
        BB = GameObject.Find("BlackBoardScript").GetComponent<Blackboard>();
        LG = GameObject.Find("LessonGeneratorScript").GetComponent<LessonGenerator>();
        audioSource = GetComponent<AudioSource>();
        randomcomment = new string[] { "あ、質問が届いていますね", "あ、生徒さんからメッセージが届いてますね", "メッセージが来てます！お答えします！" };

        mainflag = true;
        Qflag = false;
        voicenum = 0;
        textnum = 0;
        questionnum = 0;

        CreateRandamVoice();

    }

    public async void play_lesson()
    {

        while (mainflag)
        {

            if (UDPQuestion.Length < UDP.question.Count)
            {
                ///UDPQuestionが更新されている場合
                Debug.Log("更新を確認");
                UDPQuestion = UDP.question.ToArray();///UDPQuestionを更新

                int ranint = Random.Range(0, randomcomment.Length);
                await voicevox.Play(randamvoice[ranint]);
                BB.Create_TextNode(randomcomment[ranint]);

                int count = questionnum;
                while (count < UDPQuestion.Length)
                {
                    BB.Create_QuestionNode(UDPQuestion[count]);///生徒からの質問を表示する                    
                    if (UDP.question_voicelist.Count> count)
                    {
                        Debug.Log("成功1");
                        await voicevox.Play(UDP.question_voicelist[count]);
                    }
                    else
                    {
                        await voicevox.PlayOneShot(20, UDPQuestion[count]);///生成が間に合わなければ質問を再生成して読み上げるを流す
                    }
                    
                    BB.Create_TextNode(UDP.answer[count]);///解答を黒板に表示する
                    Debug.Log("count=" + count);
                    if (UDP.answer_voicelist.Count> count)
                    {
                        Debug.Log("成功2");
                        await voicevox.Play(UDP.answer_voicelist[count]);
                    }
                    else
                    {
                        await voicevox.PlayOneShot(20,UDP.answer[count]);///生成が間に合わなければ質問を再生成して読み上げるを流す
                    }                  
                    count++;
                }
                questionnum++;
            }

            BB.Text_Explain(textnum);///Splittextのtextnum番目を表示する
            await voicevox.Play(LG._voicelist[voicenum]);
            textnum++;
            voicenum++;
            await Task.Delay(1500);///ちょっと待機

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

    private async void CreateRandamVoice()
    {
        int num = 0;
        do
        {
            if (randomcomment[num] != "")
            {
                randamvoice.Add(await voicevox.CreateVoice(20, randomcomment[num]));
                //Debug.Log("音声合成完了:" + splitText[num]);
                num++;
            }
            else
            {
                Debug.Log("エラー文があったため、スキップしました");
                num++;
            }
        }
        while (num < randomcomment.Length);
    }




/*    public async void Answer()
    {
        // 受け取った時の音声ファイルをランダム再生
        int randomIndex = Random.Range(0, soundFiles.Length);
        AudioClip randomSound = soundFiles[randomIndex];
        audioSource.clip = randomSound;
        audioSource.Play();

        BB.Create_QuestionNode(UDP.question[questionnum]);
        BB.Create_AnswerNode(UDP.answer[questionnum]);
        await voicevox.PlayOneShot(20,UDP.answer[questionnum]);
        questionnum++;
    }*/

}
