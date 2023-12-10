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
    [SerializeField] VOICEVOX voicevox;///VOICEVOX�X�N���v�g�A�^�b�`�����I�u�W�F�N�g������
    public int speaker = 20;�@//�����q����


    
    void Start()
    {

    }

    public async Task<List<Voice>> CreateVoiceDate(int index,List<string> SplitText)
    {///voicevox�ŉ�������������(���X�g��Ԃ�)

        List<Voice> voicelist = new List<Voice>();
        int i = 0;
        do
        {
            string addtext = SplitText[i].Replace("�u", "").Replace("�v", "").Replace("\\n", "");

            // addtext����łȂ����`�F�b�N���Ă���ǉ�
            if (!string.IsNullOrEmpty(addtext))
            {
                voicelist.Add(await voicevox.CreateVoice(speaker, addtext));
                //Debug.Log(addtext);
            }
            i++;
        }
        while (i < SplitText.Count);

        if (index == 0)
        {///�ŏ��̉����������Ƀ{�^����true�ɂ���
            bottun.SetActive(true);
        }

        return voicelist;
    }

    public async Task<Voice> CreateOneVoice(string Text)
    {///voicevox�ŉ�������������

        Voice voice = null; ;
        string addtext = Text.Replace("�u", "").Replace("�v", "").Replace("\\n", "");

        // addtext����łȂ����`�F�b�N���Ă���ǉ�
        if (!string.IsNullOrEmpty(addtext))
        {
            voice = await voicevox.CreateVoice(speaker, addtext);
            Debug.Log($"������������: {voice != null}");
        }

        else
        {
            Debug.Log("addtext����");
        }

        return voice;
    }
}
