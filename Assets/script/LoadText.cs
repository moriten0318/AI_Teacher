using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LoadText : MonoBehaviour
{

	//�@�ǂݍ��񂾃e�L�X�g���o�͂���UI�e�L�X�g
	[SerializeField]
	private TextMeshProUGUI dataText;
	//�@�ǂލ��ރe�L�X�g���������܂�Ă���.txt�t�@�C��
	[SerializeField]
	private TextAsset textAsset;
	//�@�e�L�X�g�t�@�C������ǂݍ��񂾃f�[�^
	private string loadText1;
	//�@���s�ŕ������Ĕz��ɓ����
	private string[] splitText1;
	//�@���ݕ\�����e�L�X�g1�ԍ�
	private int textNum1;

	void Start()
	{
		loadText1 = textAsset.text;///�w�肵���e�L�X�g�A�Z�b�g��loadText1�ɓ����
		splitText1 = loadText1.Split(char.Parse("\n"));///���s�ŋ�؂��Ĕz��^splitText1�ɓ����
		textNum1 = 0;
		dataText.text = "�}�E�X�̍��N���b�N�Œʏ�̃e�L�X�g�t�@�C���̓ǂݍ��݁A�ǂݍ��݂����e�L�X�g���\������܂��B";
	}

	void Update()
	{
		//�@�ǂݍ��񂾃e�L�X�g�t�@�C���̓��e��\��
		if (Input.GetButtonDown("Fire1"))
		{
			if (splitText1[textNum1] != "")
			{
				dataText.text = splitText1[textNum1];
				textNum1++;
				if (textNum1 >= splitText1.Length)
				{
					textNum1 = 0;
				}
			}
			else
			{
				dataText.text = "";
				textNum1++;
			}
			//�@Resources�t�H���_�ɔz�u�����e�L�X�g�t�@�C���̓��e��\��
		}
	}
}
