using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LoadText : MonoBehaviour
{

	//　読み込んだテキストを出力するUIテキスト
	[SerializeField]
	private TextMeshProUGUI dataText;
	//　読む込むテキストが書き込まれている.txtファイル
	[SerializeField]
	private TextAsset textAsset;
	//　テキストファイルから読み込んだデータ
	private string loadText1;
	//　改行で分割して配列に入れる
	private string[] splitText1;
	//　現在表示中テキスト1番号
	private int textNum1;

	void Start()
	{
		loadText1 = textAsset.text;///指定したテキストアセットをloadText1に入れる
		splitText1 = loadText1.Split(char.Parse("\n"));///改行で区切って配列型splitText1に入れる
		textNum1 = 0;
		dataText.text = "マウスの左クリックで通常のテキストファイルの読み込み、読み込みしたテキストが表示されます。";
	}

	void Update()
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
	}
}
