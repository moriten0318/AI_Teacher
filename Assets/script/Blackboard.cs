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

    public void Create_TextNode(string text)
    {
        ///�e�L�X�g������ɕ\�����邽�߂̃��\�b�h

        GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab����

        Transform parent01Transform = instantiatedNode_Prefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
        Transform parent02Transform = parent01Transform.Find("Board");
        Transform childTransform = parent02Transform.Find("Text");

        TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();

        _blackboard_Node.text = text;


    }
}
