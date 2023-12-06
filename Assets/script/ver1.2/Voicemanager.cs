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

    [SerializeField] VOICEVOX voicevox;///VOICEVOX�X�N���v�g�A�^�b�`�����I�u�W�F�N�g������
    int speaker = 20;�@//�����q����
    
    void Start()
    {

    }

    public async Task<List<Voice>> CreateVoiceDate(int num)
    {///voicevox�ŉ�������������(���X�g��Ԃ�)

/*        Debug.Log("���������J�n");*/
        List<Voice> voicelist = new List<Voice>();

        JSONmanager _jsonManager = GameObject.FindObjectOfType<JSONmanager>();



        // _jsonManager.lessons �� null �łȂ��Ȃ�܂őҋ@
        while (_jsonManager.lessons == null)
        {
            await Task.Delay(100); // 100�~���b�ҋ@���čēx�`�F�b�N
/*            Debug.Log("���������ҋ@��");*/
        }

        // ���̃X�N���v�g���� JSONmanager �C���X�^���X�� Lessons �v���p�e�B�ɃA�N�Z�X
        string[] lessons = _jsonManager.lessons;


        string lessonText = lessons[num]; // �������Ώۂ̕�����
     
        if (lessonText != "")
        {
            int i = 0;
            string[] splitText = lessonText.Split(char.Parse("�B"));///��Ǔ_�ŋ�؂���splitText�ɓ����
            do
            {                
                string addtext= splitText[i].Replace("�u", "").Replace("�v", "").Replace("\\n", "");

                // addtext����łȂ����`�F�b�N���Ă���ǉ�
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
            Debug.Log("�G���[�������������߁A�X�L�b�v���܂���");
            num++;
        }
        /*        Debug.Log("�Z�N�V�����̉�����������");*/

        if (num == 0)
        {///�ŏ��̉����������Ƀ{�^����true�ɂ���
            bottun.SetActive(true);
        }

        return voicelist;
    }
}
