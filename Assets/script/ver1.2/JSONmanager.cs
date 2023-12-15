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

    private LessonDataStorage _storage = new LessonDataStorage();

    string LESSONDATA_PATH = "C://Users//moris//Desktop//Mypython//AITeacher_python//lessondata";
    //string JSON_PATH = "//honji_tenkai_1214.json";

    public Lesson[] jsondata { get; private set; }    /// �ǂݎ���p�̔z��Ƃ���lessons���`
    public string[] lessons { get; private set; }    /// �ǂݎ���p�̔z��Ƃ���lessons���`


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

        lessons = jsondata.Select(data => data.���t�̔��b).ToArray();///1�v�f��1�Z�N�V����������z��
        Dictionary<int, List<string>> returndic = new Dictionary<int, List<string>>();

        int i = 0;
        foreach (string lesson in lessons)
        {
            // �u�B�v�Ɓu�H�v����؂蕶���Ƃ��Ďg�p
            string[] splittext = lesson.Split(new char[] { '�B', '�H' }, StringSplitOptions.None);

            List<string> splitList = new List<string>();
            foreach (var text in splittext)
            {
                // ��ł͂Ȃ�������ɑ΂��Ă̂ݏ��������s
                if (!string.IsNullOrEmpty(text))
                {
                    // ���̃e�L�X�g�Ɂu�H�v��ǉ����ă��X�g�Ɋi�[
                    string newText = text.Contains("�H") ? text + "�H" : text;
                    splitList.Add(newText);
                }
            }

            returndic[i] = splitList;
            i++;
        }

        Debug.Log("JSON�̃��[�h����");
        return returndic;
    }

    private void Start()
    {        

    }
}
