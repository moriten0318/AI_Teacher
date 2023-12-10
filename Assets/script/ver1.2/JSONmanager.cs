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
    public string 内容の説明;
    public string キーワード;
    public string 板書;
    public string 教師の発話;
}

public class JSONmanager : MonoBehaviour
{

    private LessonDataStorage _storage = new LessonDataStorage();

    string LESSONDATA_PATH = "C://Users//moris//Desktop//Mypython//AITeacher_python//lessondata";
    string JSON_PATH = "//honji_tenkai_1202.json";

    public Lesson[] jsondata { get; private set; }    /// 読み取り専用の配列としてlessonsを定義
    public string[] lessons { get; private set; }    /// 読み取り専用の配列としてlessonsを定義


    public Dictionary<int, List<string>> LoadSpeech()
    {
        string json = File.ReadAllText(LESSONDATA_PATH+JSON_PATH);
        jsondata = JsonHelper.FromJson<Lesson>(json);

        lessons = jsondata.Select(data => data.教師の発話).ToArray();///1要素に1セクションがある配列
        Dictionary<int, List<string>> returndic = new Dictionary<int, List<string>>();

        int i = 0;
        foreach (string lesson in lessons)
        {
            string[] splittext=lessons[i].Split(char.Parse("。"));
            List<string> splitList = splittext.ToList();
            
            returndic[i] = splitList;

/*            foreach (string text in _storage.GetLessonData(i))
            {
                Debug.Log(text);
                Debug.Log(i);
            }*/

            i++;
        }
        Debug.Log("JSONのロード完了");
        return returndic;
    }

    private void Start()
    {        

    }
}
