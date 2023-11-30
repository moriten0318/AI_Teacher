using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;

public class BlackBoardManager : MonoBehaviour
{

    public MainManager _Main;

    public RawImage BlackBoard;
    private Texture2D[] textureArray;
    string LESSONDATA_PATH = "C://Users//moris//Desktop//Mypython//AITeacher_python//lessondata";

    // �e�N�X�`����z��ɒǉ����邽�߂̃��X�g���쐬
    List<Texture2D> textureList = new List<Texture2D>();


    // Start is called before the first frame update
    void Awake()
    {
        Texture2D[] textureArray; //�z����쐬
    }

    void Start()
    {

        // �t�H���_����PNG�t�@�C����ǂݍ���Ń��X�g�ɓ����
        int i = 0;
        while (true)
        {
            string fileName = "slide" + i + ".png"; // �t�@�C�����𐶐�
            string filePath = Path.Combine(LESSONDATA_PATH, fileName); // �t�@�C���̃t���p�X�𐶐�

            // �摜��ǂݍ��݁ATexture2D�I�u�W�F�N�g�Ƃ��Ď擾
            Texture2D imageTexture = LoadImage(filePath);

            if (imageTexture != null)
            {
                // ���������ꍇ�AimageTexture�ɂ͓ǂݍ��񂾉摜���i�[����Ă��܂��B
                // �����Ŏ擾�����e�N�X�`�����g���ĉ�������̏������s�����Ƃ��ł��܂��B
                // �Ⴆ�΁A�����\������RawImage��Material�ɐݒ肷�邱�Ƃ��ł��܂��B

                //�ȉ��擾�����e�N�X�`���ɂ��Ă̏���
                // ���X�g�ɒǉ�
                textureList.Add(imageTexture);
                i++;

            }
            else
            {
                // �t�@�C�������݂��Ȃ��ꍇ�A�ǂݍ��݂��~
                Debug.Log("�X���C�h�摜�ۑ�����");
                break;
            }
        }

        _Main.textureList = textureList;
    }


    public void UpdateBlackBoard(int boardindex)
    {
        // �C���f�b�N�X���L���ŁA�e�N�X�`���̐��ȉ��̏ꍇ�ɂ̂ݏ��������s
        if (boardindex >= 0 && boardindex < textureList.Count)
        {
            // RawImage�����݂��A�e�N�X�`�����L���ȏꍇ
            if (BlackBoard != null && textureList[boardindex] != null)
            {
                BlackBoard.texture = textureList[boardindex]; // RawImage�Ƀe�N�X�`����ݒ�
            }
        }

    }


    private static Texture2D LoadImage(string path)
    {
        // �t�@�C���̃o�C�i���f�[�^���i�[���邽�߂̃o�C�g�z��
        byte[] binary;
        try
        {
            // �w�肳�ꂽ�t�@�C�����o�C�i�����[�h�ŊJ��
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // �t�@�C���̒������擾
                var length = (int)fs.Length;
                // �o�C�i���f�[�^���i�[���邽�߂̃o�C�g�z����쐬
                binary = new byte[length];
                // �t�@�C������o�C�i���f�[�^��ǂݎ��A�o�C�g�z��Ɋi�[
                fs.Read(binary, 0, length);
                // �t�@�C�������
                fs.Close();
            }
        }
        catch (IOException exception)
        {
            // ��O�����������ꍇ�A�G���[���O��\������null��Ԃ�
            Debug.Log(exception);
            return null;
        }
        // �V����Texture2D�I�u�W�F�N�g���쐬
        var texture = new Texture2D(0, 0);
        // �o�C�i���f�[�^��Texture2D�ɓǂݍ���
        texture.LoadImage(binary);
        // �ǂݍ��񂾃e�N�X�`����Ԃ�
        return texture;
    }
}
