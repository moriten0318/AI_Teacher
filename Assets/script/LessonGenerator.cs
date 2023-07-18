using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;

public class LessonGenerator : MonoBehaviour
{
    [SerializeField] public TextAsset textAsset;    //�@�ǂލ��ރe�L�X�g���������܂�Ă���.txt�t�@�C��
    public string loadText;    //�@�e�L�X�g�t�@�C������ǂݍ��񂾃f�[�^
    public string[] splitText;    //�@���s�ŕ������Ĕz��ɓ����
    public int textNum;    ///�@���ݕ\�����e�L�X�g1�ԍ�

    [SerializeField] VOICEVOX voicevox;///VOICEVOX�X�N���v�g�A�^�b�`�����I�u�W�F�N�g������
    int speaker = 20;�@//�����q����
    public List<Voice> _voicelist = new List<Voice>();

    public bool flag = false;///true��������i��ł悵
    [SerializeField] GameObject bottun;
    [SerializeField] GameObject loadpanel;

    void Start()
    {
        loadText = textAsset.text;////�w�肵���e�L�X�g�A�Z�b�g��loadText1�ɓ����
        splitText = loadText.Split(char.Parse("�B"));///���s�ŋ�؂��Ĕz��^splitText�ɓ����
		textNum = 0;

        CreateVoiceDate();

    }

    public async void CreateVoiceDate()
    {
        int num = 0;
            do
            {
                if (splitText[num] != "")
                {
                    _voicelist.Add(await voicevox.CreateVoice(speaker, splitText[num]));
                    Debug.Log("������������:" + splitText[num]);
                    num++;
                    if (num == 1)
                    {///�ŏ��̉�����������true�ɂ���
                    bottun.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log("�G���[�������������߁A�X�L�b�v���܂���");
                    num++;
                }
            }
            while (num < splitText.Length);
        Debug.Log("�S�Ẳ�����������");
    }


}
