using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blackboard : MonoBehaviour
{
    /// <summary>
    /// ����ɕ\�������v�f���Ǘ�����N���X
    /// </summary>

    public GameObject _TextNode;/// ��������textPrefab�̂��߂̕ϐ��錾
    public GameObject _Text_parent;///textnode�̐e�v�f�p�ϐ�
	public int maxChildCount = 3;
    private LessonGenerator _generator;


    void Start()
    {
        ///LoadText�X�N���v�g�ɃA�N�Z�X����p
    _generator = GameObject.Find("LessonGeneratorScript").GetComponent<LessonGenerator>();
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

        if (_Text_parent.transform.childCount >= maxChildCount)///����ɂR�ȏ�Node�����܂�����폜����
        {
            Destroy_TextNode();
        }

        GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab����

        Transform parent01Transform = instantiatedNode_Prefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
        Transform parent02Transform = parent01Transform.Find("Board");
        Transform childTransform = parent02Transform.Find("Text");

        TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();

        _blackboard_Node.text = text;


    }

    private void Destroy_TextNode()
    {
        foreach (Transform child in _Text_parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
