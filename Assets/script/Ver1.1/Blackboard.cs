using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class Blackboard : MonoBehaviour
{
    /// <summary>
    /// ����ɕ\�������v�f���Ǘ�����N���X
    /// </summary>

    public GameObject _TextNode;/// ��������textPrefab�̂��߂̕ϐ��錾
    public GameObject _Text_parent;///textnode�̐e�v�f�p�ϐ�
    public GameObject _QuestionNode;/// ��������questionPrefab�̂��߂̕ϐ��錾
    public GameObject _Question_parent;///questiontnode�̐e�v�f�p�ϐ�
    public GameObject _ImageNode;

    public int maxChildCount;
    private LessonGenerator _generator;
    private int IMGnum = 1;
    public List<Sprite> spriteList = new List<Sprite>();

    AudioSource audioSource;
    ///public AudioClip boardsound;



    void Start()
    {
        //audioSource = GetComponent<AudioSource>();

        ///LoadText�X�N���v�g�ɃA�N�Z�X����p
        //_generator = GameObject.Find("LessonGeneratorScript").GetComponent<LessonGenerator>();



        ///ImageFile��Sprite��List�ɓ����
/*        for (int i = 1; ; i++)
        {
            string spritePath = "Imagefile2/Image" + i;
            Sprite sprite = Resources.Load<Sprite>(spritePath);
            if (sprite == null)
            {
                break;
            }
            spriteList.Add(sprite);
        }*/
    }

    public void Text_Explain(int num)
    {
        ///num�̃e�L�X�g������Ƀm�[�h�Ƃ��ĕ\������
        if (_generator.splitText[num] != "")
        {
            Create_TextNode(_generator.splitText[num]);

        }
        else
        {
            Create_TextNode("����̓G���[�e�L�X�g�ł�");
            _generator.textNum++;
        }
    }


    public void Create_TextNode(string text)
    {
        ///�e�L�X�g������ɕ\�����邽�߂̃��\�b�h///

        Transform imageObject = _Text_parent.transform.Find("ImagePrefab(Clone)");
        ///�����Image��\���������͍폜����
        if (imageObject != null)
        {
            Destroy_TextNode();
        }

        if (_Text_parent.transform.childCount >= maxChildCount)///����ɂR�ȏ�Node�����܂�����폜����
        {
            Destroy_TextNode();
        }

        ///Img�\�����b�Z�[�W�����邩�`�F�b�N
        string n = IMGnum.ToString();
        string checktext = string.Format("image{0}", n);
        bool containsIMGnum = text.Contains(checktext);


        if (containsIMGnum)
        {///�摜�\������
            GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab����
            
            string pattern = checktext;
            text = Regex.Replace(text, pattern, "");

            Transform parent01Transform = instantiatedNode_Prefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
            Transform parent02Transform = parent01Transform.Find("Board");
            Transform childTransform = parent02Transform.Find("Text");

            TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();
            _blackboard_Node.text = text;

            if (_Text_parent.transform.childCount >= maxChildCount)///����ɂR�ȏ�Node�����܂������ԏ���폜����
            {
                Transform topChild = _Text_parent.transform.GetChild(0);
                Destroy(topChild.gameObject);
            }
            Create_Image();
        }
        else
        {///�摜�\������
            GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab����
            Transform parent01Transform = instantiatedNode_Prefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
            Transform parent02Transform = parent01Transform.Find("Board");
            Transform childTransform = parent02Transform.Find("Text");

            TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();
            _blackboard_Node.text = text;

        }

    }

    public void Create_Image()
    {
        GameObject instantiatedImage_Prefab = Instantiate(_ImageNode, _Text_parent.transform);///Prefab����
        Image _IMGcompornent = instantiatedImage_Prefab.GetComponent<Image>();
        if (spriteList[IMGnum-1] != null)
        {
            _IMGcompornent.sprite = spriteList[IMGnum-1];
        }
        IMGnum++;
    }

    public void Create_QuestionNode(string text)
    {
        GameObject instantiatedNode_Prefab = Instantiate(_QuestionNode, _Question_parent.transform);///Prefab����
        Transform parent01Transform = instantiatedNode_Prefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
        Transform parent02Transform = parent01Transform.Find("ChatBoard");
        Transform childTransform = parent02Transform.Find("ChatText");

        TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();
        _blackboard_Node.text = text;

        Invoke("Destroy_QuestionNode", 10.0f);
    }

    public void Create_AnswerNode(string text)
    {
        Destroy_TextNode();
        GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab����
        Transform parent01Transform = instantiatedNode_Prefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
        Transform parent02Transform = parent01Transform.Find("Board");
        Transform childTransform = parent02Transform.Find("Text");

        TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();
        _blackboard_Node.text = text;

        

    }

    private void Destroy_QuestionNode()
    {
        foreach (Transform child in _Question_parent.transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("����m�[�h�폜");
    }


    public void Destroy_TextNode()
    {
        foreach (Transform child in _Text_parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

/*    public void BoardSound()
    {
        // �����̍Đ�
        audioSource.PlayOneShot(boardsound);
    }*/

}
