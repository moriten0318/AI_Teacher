using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;
using System;
using System.Text.RegularExpressions;
using System.IO;

using System.Linq;

/// <summary>
/// JSONファイルから「教師の発話」及びパワポのスライドを画像として取得する
/// </summary>

[System.Serializable]
public class LessonSection
{
    public string 内容の説明;
    public string キーワード;
    public string 板書;
    public string 教師の発話;
}

public class JSONmanager : MonoBehaviour
{

    private LessonDataStorage _storage = new LessonDataStorage();

    string LESSONDATA_PATH = "C://Users//moris//Desktop//Mypython//AITeacher_python//lessondata";
    //string JSON_PATH = "//honji_tenkai_1214.json";

    public Lesson[] jsondata { get; private set; }    /// 読み取り専用の配列としてlessonsを定義
    public string[] lessons { get; private set; }    /// 読み取り専用の配列としてlessonsを定義


    public Dictionary<int, List<string>> LoadSpeech()
    {
        string[] jsonFiles = Directory.GetFiles(LESSONDATA_PATH, "*.json");

        if (jsonFiles.Length == 0)
        {
            Debug.LogError("No JSON files found in the specified directory.");
            return null;
        }

        // Assuming you want to load the first JSON file found in the directory.
        string json = File.ReadAllText(jsonFiles[0]);
        jsondata = JsonHelper.FromJson<Lesson>(json);

        lessons = jsondata.Select(data => data.教師の発話).ToArray();///1要素に1セクションがある配列
        Dictionary<int, List<string>> returndic = new Dictionary<int, List<string>>();

        int i = 0;
        foreach (string lesson in lessons)
        {
            // 「。」と「？」を区切り文字として使用
            string[] splittext = lesson.Split(new char[] { '。', '？' }, StringSplitOptions.None);

            List<string> splitList = new List<string>();
            foreach (var text in splittext)
            {
                // 空ではない文字列に対してのみ処理を実行
                if (!string.IsNullOrEmpty(text))
                {
                    // 元のテキストに「？」を追加してリストに格納
                    string newText = text.Contains("？") ? text + "？" : text;
                    splitList.Add(newText);
                }
            }

            returndic[i] = splitList;
            i++;
        }

        Debug.Log("JSONのロード完了");
        return returndic;
    }

    private void Start()
    {        

    }
}
