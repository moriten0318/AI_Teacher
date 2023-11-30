using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonLoader : MonoBehaviour
{
    // パブリックなフィールドとしてパスを定義
    public string path = "Assets/JsonHandler/Resources/updated_honji_tenkai_1121.json";

    void Start()
    {
        string json = File.ReadAllText(path);
        Lesson[] lessons = JsonHelper.FromJson<Lesson>(json);

        foreach (Lesson lesson in lessons)
        {
            Debug.Log(lesson.教師の発話);
        }
    }
}