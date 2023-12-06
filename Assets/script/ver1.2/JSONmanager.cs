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
/// JSON�t�@�C������u���t�̔��b�v�y�уp���|�̃X���C�h���摜�Ƃ��Ď擾����
/// </summary>

[System.Serializable]
public class LessonSection
{
    public string ���e�̐���;
    public string �L�[���[�h;
    public string ��;
    public string ���t�̔��b;
}

public class JSONmanager : MonoBehaviour
{

    public MainManager _Main;

    string LESSONDATA_PATH = "C://Users//moris//Desktop//Mypython//AITeacher_python//lessondata";
    string JSON_PATH = "//honji_tenkai_1202.json";

    public Lesson[] jsondata { get; private set; }    /// �ǂݎ���p�̔z��Ƃ���lessons���`
    public string[] lessons { get; private set; }    /// �ǂݎ���p�̔z��Ƃ���lessons���`


    private void LoadSpeech()
    {
        string json = File.ReadAllText(LESSONDATA_PATH+JSON_PATH);
        jsondata = JsonHelper.FromJson<Lesson>(json);

        lessons = jsondata.Select(data => data.���t�̔��b).ToArray();

        /*        foreach (string lesson in lessons)
                {
                    Debug.Log(lesson);
                }*/

        _Main.lessons = lessons;

        Debug.Log("JSON�̃��[�h����");
    }


    private void Start()
    {
        LoadSpeech();
    }
}
