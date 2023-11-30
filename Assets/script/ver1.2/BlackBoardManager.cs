using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;

public class BlackBoardManager : MonoBehaviour
{

    public MainManager _Main;

    public RawImage BlackBoard;
    private Texture2D[] textureArray;
    string LESSONDATA_PATH = "C://Users//moris//Desktop//Mypython//AITeacher_python//lessondata";

    // テクスチャを配列に追加するためのリストを作成
    List<Texture2D> textureList = new List<Texture2D>();


    // Start is called before the first frame update
    void Awake()
    {
        Texture2D[] textureArray; //配列を作成
    }

    void Start()
    {

        // フォルダ内のPNGファイルを読み込んでリストに入れる
        int i = 0;
        while (true)
        {
            string fileName = "slide" + i + ".png"; // ファイル名を生成
            string filePath = Path.Combine(LESSONDATA_PATH, fileName); // ファイルのフルパスを生成

            // 画像を読み込み、Texture2Dオブジェクトとして取得
            Texture2D imageTexture = LoadImage(filePath);

            if (imageTexture != null)
            {
                // 成功した場合、imageTextureには読み込んだ画像が格納されています。
                // ここで取得したテクスチャを使って何かしらの処理を行うことができます。
                // 例えば、これを表示するRawImageやMaterialに設定することができます。

                //以下取得したテクスチャについての処理
                // リストに追加
                textureList.Add(imageTexture);
                i++;

            }
            else
            {
                // ファイルが存在しない場合、読み込みを停止
                Debug.Log("スライド画像保存完了");
                break;
            }
        }

        _Main.textureList = textureList;
    }


    public void UpdateBlackBoard(int boardindex)
    {
        // インデックスが有効で、テクスチャの数以下の場合にのみ処理を実行
        if (boardindex >= 0 && boardindex < textureList.Count)
        {
            // RawImageが存在し、テクスチャが有効な場合
            if (BlackBoard != null && textureList[boardindex] != null)
            {
                BlackBoard.texture = textureList[boardindex]; // RawImageにテクスチャを設定
            }
        }

    }


    private static Texture2D LoadImage(string path)
    {
        // ファイルのバイナリデータを格納するためのバイト配列
        byte[] binary;
        try
        {
            // 指定されたファイルをバイナリモードで開く
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // ファイルの長さを取得
                var length = (int)fs.Length;
                // バイナリデータを格納するためのバイト配列を作成
                binary = new byte[length];
                // ファイルからバイナリデータを読み取り、バイト配列に格納
                fs.Read(binary, 0, length);
                // ファイルを閉じる
                fs.Close();
            }
        }
        catch (IOException exception)
        {
            // 例外が発生した場合、エラーログを表示してnullを返す
            Debug.Log(exception);
            return null;
        }
        // 新しいTexture2Dオブジェクトを作成
        var texture = new Texture2D(0, 0);
        // バイナリデータをTexture2Dに読み込む
        texture.LoadImage(binary);
        // 読み込んだテクスチャを返す
        return texture;
    }
}
