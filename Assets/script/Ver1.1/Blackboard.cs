using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class Blackboard : MonoBehaviour
{
    /// <summary>
    /// 黒板上に表示される要素を管理するクラス
    /// </summary>

    public GameObject _TextNode;/// 生成するtextPrefabのための変数宣言
    public GameObject _Text_parent;///textnodeの親要素用変数
    public GameObject _QuestionNode;/// 生成するquestionPrefabのための変数宣言
    public GameObject _Question_parent;///questiontnodeの親要素用変数
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

        ///LoadTextスクリプトにアクセスする用
        //_generator = GameObject.Find("LessonGeneratorScript").GetComponent<LessonGenerator>();



        ///ImageFileのSpriteをListに入れる
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
        ///numのテキストを黒板上にノードとして表示する
        if (_generator.splitText[num] != "")
        {
            Create_TextNode(_generator.splitText[num]);

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

        Transform imageObject = _Text_parent.transform.Find("ImagePrefab(Clone)");
        ///黒板上にImageを表示した次は削除する
        if (imageObject != null)
        {
            Destroy_TextNode();
        }

        if (_Text_parent.transform.childCount >= maxChildCount)///黒板上に３つ以上Nodeが溜まったら削除する
        {
            Destroy_TextNode();
        }

        ///Img表示メッセージがあるかチェック
        string n = IMGnum.ToString();
        string checktext = string.Format("image{0}", n);
        bool containsIMGnum = text.Contains(checktext);


        if (containsIMGnum)
        {///画像表示あり
            GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab生成
            
            string pattern = checktext;
            text = Regex.Replace(text, pattern, "");

            Transform parent01Transform = instantiatedNode_Prefab.transform;///生成したPrefabはTextの親要素なので、親のTransformとして取得
            Transform parent02Transform = parent01Transform.Find("Board");
            Transform childTransform = parent02Transform.Find("Text");

            TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();
            _blackboard_Node.text = text;

            if (_Text_parent.transform.childCount >= maxChildCount)///黒板上に３つ以上Nodeが溜まったら一番上を削除する
            {
                Transform topChild = _Text_parent.transform.GetChild(0);
                Destroy(topChild.gameObject);
            }
            Create_Image();
        }
        else
        {///画像表示無し
            GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab生成
            Transform parent01Transform = instantiatedNode_Prefab.transform;///生成したPrefabはTextの親要素なので、親のTransformとして取得
            Transform parent02Transform = parent01Transform.Find("Board");
            Transform childTransform = parent02Transform.Find("Text");

            TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();
            _blackboard_Node.text = text;

        }

    }

    public void Create_Image()
    {
        GameObject instantiatedImage_Prefab = Instantiate(_ImageNode, _Text_parent.transform);///Prefab生成
        Image _IMGcompornent = instantiatedImage_Prefab.GetComponent<Image>();
        if (spriteList[IMGnum-1] != null)
        {
            _IMGcompornent.sprite = spriteList[IMGnum-1];
        }
        IMGnum++;
    }

    public void Create_QuestionNode(string text)
    {
        GameObject instantiatedNode_Prefab = Instantiate(_QuestionNode, _Question_parent.transform);///Prefab生成
        Transform parent01Transform = instantiatedNode_Prefab.transform;///生成したPrefabはTextの親要素なので、親のTransformとして取得
        Transform parent02Transform = parent01Transform.Find("ChatBoard");
        Transform childTransform = parent02Transform.Find("ChatText");

        TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();
        _blackboard_Node.text = text;

        Invoke("Destroy_QuestionNode", 10.0f);
    }

    public void Create_AnswerNode(string text)
    {
        Destroy_TextNode();
        GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab生成
        Transform parent01Transform = instantiatedNode_Prefab.transform;///生成したPrefabはTextの親要素なので、親のTransformとして取得
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
        Debug.Log("質問ノード削除");
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
        // 音声の再生
        audioSource.PlayOneShot(boardsound);
    }*/

}
