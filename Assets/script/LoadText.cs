using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LoadText : MonoBehaviour
{///もう使わないスクリプトファイル

	//　読み込んだテキストを出力するUIテキスト
	///[SerializeField]
	///public TextMeshProUGUI dataText;
	//　読む込むテキストが書き込まれている.txtファイル
	[SerializeField]
	public TextAsset textAsset;
	//　テキストファイルから読み込んだデータ
	public string loadText1;
	//　改行で分割して配列に入れる
	public string[] splitText1;
	//　現在表示中テキスト1番号
	public int textNum1;

	void Start()
	{
		loadText1 = textAsset.text;////指定したテキストアセットをloadText1に入れる
		splitText1 = loadText1.Split(char.Parse("。"));///改行で区切って配列型splitText1に入れる
		textNum1 = 0;
		///dataText.text = "マウスの左クリックで改行ごとに分割した文章が表示されます。";
	}

/*	void Update()
	{
		//　読み込んだテキストファイルの内容を表示
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
			//　Resourcesフォルダに配置したテキストファイルの内容を表示
		}
	}*/
}
