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
    public GameObject _ImageNode;
	public int maxChildCount = 3;
    private LessonGenerator _generator;
    public int IMGnum = 1;
    public List<Sprite> spriteList = new List<Sprite>();


    void Start()
    {
        ///LoadText�X�N���v�g�ɃA�N�Z�X����p
        _generator = GameObject.Find("LessonGeneratorScript").GetComponent<LessonGenerator>();
        ///ImageFile��Sprite��List�ɓ����
        for (int i = 1; i <= 3; i++)
        {
            string spritePath = "Imagefile1/Image" + i;
            Sprite sprite = Resources.Load<Sprite>(spritePath);
            spriteList.Add(sprite);
        }
    }

	public void Text_Explain()
	{
		//�@�ǂݍ��񂾃e�L�X�g�t�@�C���̓��e��\��
			if (_generator.splitText[_generator.textNum] != "")
			{
				Create_TextNode(_generator.splitText[_generator.textNum]);
                _generator.textNum++;
				if (_generator.textNum >= _generator.splitText.Length)
				{
                    _generator.textNum = 0;
				}
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

        Transform imageObject = _Text_parent.transform.Find("ImagePrefab");
        ///�����Image��\���������͍폜����
        if (imageObject != null)
        {
            Destroy_TextNode();
            Debug.Log("�摜���������̂ŏ����܂���");
        }

        if (_Text_parent.transform.childCount >= maxChildCount)///����ɂR�ȏ�Node�����܂�����폜����
        {
            Destroy_TextNode();
        }

        string n = IMGnum.ToString();
        bool containsIMGnum = text.Contains("image"+n);///
        if (containsIMGnum)
        {
            Debug.Log("�摜�\���}�[�J�[�m�F");
            GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab����
            string pattern = "image" + n;
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
        {
            Debug.Log("�摜�}�[�J�[�Ȃ�");
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
        _IMGcompornent.sprite = spriteList[IMGnum-1];
        IMGnum++;
    }


    private void Destroy_TextNode()
    {
        foreach (Transform child in _Text_parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
