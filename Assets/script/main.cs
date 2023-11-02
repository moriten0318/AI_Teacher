using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;
using System.Threading.Tasks;

public class main : MonoBehaviour
{
    [SerializeField] VOICEVOX voicevox;///VOICEVOX�X�N���v�g�A�^�b�`�����I�u�W�F�N�g������

    public Blackboard BB;
    public LessonGenerator LG;
    public UDPReceiver UDP;

    public bool Qflag;
    private bool mainflag;
    private int voicenum;
    private int textnum;
    private int questionnum;

    public AudioClip[] soundFiles;  // �����t�@�C�����i�[����z��
    public AudioClip instance01;
    public AudioClip instance02;
    public AudioClip instance03;
    public AudioSource audioSource;  // �����Đ��p��AudioSource�R���|�[�l���g(VOICEVOX���w�肷��)

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
            ///���₪����Qflag��true�ɂȂ����珈������
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
                await Task.Delay(2500);///������Ƒҋ@

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
            await Task.Delay(100); // 100�~���b�ҋ@
        }
    }

    public async void Answer()
    {

        Qflag = false;
        // �󂯎�������̉����t�@�C���������_���Đ�
        int randomIndex = Random.Range(0, soundFiles.Length);
        AudioClip randomSound = soundFiles[randomIndex];
        audioSource.clip = randomSound;
        audioSource.Play();

        await Task.Delay(2000);///������Ƒҋ@
        BB.Create_QuestionNode(UDP.question[questionnum]);
        BB.Create_AnswerNode(UDP.answer[questionnum]);
        await voicevox.PlayOneShot(20,UDP.answer[questionnum]);
        questionnum++;

    }

}
