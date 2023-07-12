using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blackboard : MonoBehaviour
{
    /// <summary>
    /// 黒板上に表示される要素を管理するクラス
    /// </summary>

    public GameObject _TextNode;/// 生成するtextPrefabのための変数宣言
    public GameObject _Text_parent;///textnodeの親要素用変数
	public int maxChildCount = 3;
    private LessonGenerator _generator;


    void Start()
    {
        ///LoadTextスクリプトにアクセスする用
    _generator = GameObject.Find("LessonGeneratorScript").GetComponent<LessonGenerator>();
    }

	public void Text_Explain()
	{
		//　読み込んだテキストファイルの内容を表示
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
			    Create_TextNode("これはエラーテキストです");
                _generator.textNum++;
			}
	}


    public void Create_TextNode(string text)
    {
        ///テキストを黒板上に表示するためのメソッド///

        if (_Text_parent.transform.childCount >= maxChildCount)///黒板上に３つ以上Nodeが溜まったら削除する
        {
            Destroy_TextNode();
        }

        GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab生成

        Transform parent01Transform = instantiatedNode_Prefab.transform;///生成したPrefabはTextの親要素なので、親のTransformとして取得
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
