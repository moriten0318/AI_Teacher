using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;
using System.Text.RegularExpressions;
using System.IO;

using System.Linq;

/// <summary>
/// JSONファイルから「教師の発話」及びパワポのスライドを画像として取得する
/// </summary>

[System.Serializable]
public class LessonSection
{
    public string 時間;
    public string 学習活動;
    public string 指導上の留意点;
    public string 評価の観点;
    public string 板書;
    public string 画像;
    public string 教師の発話;
}

public class JSONmanager : MonoBehaviour
{

    public MainManager _Main;

    string LESSONDATA_PATH = "C://Users//moris//Desktop//Mypython//AITeacher_python//lessondata";
    string JSON_PATH = "//updated_honji_tenkai_1121.json";

    public Lesson[] jsondata { get; private set; }    /// 読み取り専用の配列としてlessonsを定義
    public string[] lessons { get; private set; }    /// 読み取り専用の配列としてlessonsを定義


    private void LoadSpeech()
    {
        string json = File.ReadAllText(LESSONDATA_PATH+JSON_PATH);
        jsondata = JsonHelper.FromJson<Lesson>(json);

        lessons = jsondata.Select(data => data.教師の発話).ToArray();

        /*        foreach (string lesson in lessons)
                {
                    Debug.Log(lesson);
                }*/

        _Main.lessons = lessons;

        Debug.Log("JSONのロード完了");
    }


    private void Start()
    {
        LoadSpeech();
    }
}
