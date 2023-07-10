using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blackboard : MonoBehaviour
{

    public GameObject _TextNode;/// 生成するtextPrefabのための変数宣言
    public GameObject _Text_parent;///textnodeの親要素用変数

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
        ///テキストを黒板上に表示するためのメソッド


        GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab生成

        Transform parent01Transform = instantiatedNode_Prefab.transform;///生成したPrefabはTextの親要素なので、親のTransformとして取得
        Transform parent02Transform = parent01Transform.Find("ChatBoard");
        Transform childTransform = parent02Transform.Find("ChatText");

        TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();

        _blackboard_Node.text = text;


    }
}
