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

    public string[] UDPQuestion;
    public string[] randomcomment;
    public List<Voice>randamvoice = new List<Voice>();

    public AudioSource audioSource;  // �����Đ��p��AudioSource�R���|�[�l���g(VOICEVOX���w�肷��)

    void Start()
    {
        BB = GameObject.Find("BlackBoardScript").GetComponent<Blackboard>();
        LG = GameObject.Find("LessonGeneratorScript").GetComponent<LessonGenerator>();
        audioSource = GetComponent<AudioSource>();
        randomcomment = new string[] { "���A���₪�͂��Ă��܂���", "���A���k���񂩂烁�b�Z�[�W���͂��Ă܂���", "���b�Z�[�W�����Ă܂��I���������܂��I" };

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
                ///UDPQuestion���X�V����Ă���ꍇ
                Debug.Log("�X�V���m�F");
                UDPQuestion = UDP.question.ToArray();///UDPQuestion���X�V

                int ranint = Random.Range(0, randomcomment.Length);
                await voicevox.Play(randamvoice[ranint]);
                BB.Create_TextNode(randomcomment[ranint]);

                int count = questionnum;
                while (count < UDPQuestion.Length)
                {
                    BB.Create_QuestionNode(UDPQuestion[count]);///���k����̎����\������                    
                    if (UDP.question_voicelist.Count> count)
                    {
                        Debug.Log("����1");
                        await voicevox.Play(UDP.question_voicelist[count]);
                    }
                    else
                    {
                        await voicevox.PlayOneShot(20, UDPQuestion[count]);///�������Ԃɍ���Ȃ���Ύ�����Đ������ēǂݏグ��𗬂�
                    }
                    
                    BB.Create_TextNode(UDP.answer[count]);///�𓚂����ɕ\������
                    Debug.Log("count=" + count);
                    if (UDP.answer_voicelist.Count> count)
                    {
                        Debug.Log("����2");
                        await voicevox.Play(UDP.answer_voicelist[count]);
                    }
                    else
                    {
                        await voicevox.PlayOneShot(20,UDP.answer[count]);///�������Ԃɍ���Ȃ���Ύ�����Đ������ēǂݏグ��𗬂�
                    }                  
                    count++;
                }
                questionnum++;
            }

            BB.Text_Explain(textnum);///Splittext��textnum�Ԗڂ�\������
            await voicevox.Play(LG._voicelist[voicenum]);
            textnum++;
            voicenum++;
            await Task.Delay(1500);///������Ƒҋ@

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
                //Debug.Log("������������:" + splitText[num]);
                num++;
            }
            else
            {
                Debug.Log("�G���[�������������߁A�X�L�b�v���܂���");
                num++;
            }
        }
        while (num < randomcomment.Length);
    }




/*    public async void Answer()
    {
        // �󂯎�������̉����t�@�C���������_���Đ�
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
