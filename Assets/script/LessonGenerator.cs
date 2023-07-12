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

    bool waiting_flag = true;///true��������i��ł悵

    void Start()
    {
        loadText = textAsset.text;////�w�肵���e�L�X�g�A�Z�b�g��loadText1�ɓ����
        splitText = loadText.Split(char.Parse("�B"));///���s�ŋ�؂��Ĕz��^splitText1�ɓ����
		textNum = 0;
    }

}
