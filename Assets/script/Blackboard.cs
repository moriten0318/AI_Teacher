using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blackboard : MonoBehaviour
{

    public GameObject _TextNode;/// ��������textPrefab�̂��߂̕ϐ��錾
    public GameObject _Text_parent;///textnode�̐e�v�f�p�ϐ�

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create_TextNode(string text)
    {
        ///�e�L�X�g������ɕ\�����邽�߂̃��\�b�h


        GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab����

        Transform parent01Transform = instantiatedNode_Prefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
        Transform parent02Transform = parent01Transform.Find("ChatBoard");
        Transform childTransform = parent02Transform.Find("ChatText");

        TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();

        _blackboard_Node.text = text;


    }
}
