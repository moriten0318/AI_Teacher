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

    private LessonDataStorage _storage = new LessonDataStorage();

    string LESSONDATA_PATH = "C://Users//moris//Desktop//Mypython//AITeacher_python//lessondata";
    string JSON_PATH = "//honji_tenkai_1202.json";

    public Lesson[] jsondata { get; private set; }    /// �ǂݎ���p�̔z��Ƃ���lessons���`
    public string[] lessons { get; private set; }    /// �ǂݎ���p�̔z��Ƃ���lessons���`


    public Dictionary<int, List<string>> LoadSpeech()
    {
        string json = File.ReadAllText(LESSONDATA_PATH+JSON_PATH);
        jsondata = JsonHelper.FromJson<Lesson>(json);

        lessons = jsondata.Select(data => data.���t�̔��b).ToArray();///1�v�f��1�Z�N�V����������z��
        Dictionary<int, List<string>> returndic = new Dictionary<int, List<string>>();

        int i = 0;
        foreach (string lesson in lessons)
        {
            string[] splittext=lessons[i].Split(char.Parse("�B"));
            List<string> splitList = splittext.ToList();
            
            returndic[i] = splitList;

/*            foreach (string text in _storage.GetLessonData(i))
            {
                Debug.Log(text);
                Debug.Log(i);
            }*/

            i++;
        }
        Debug.Log("JSON�̃��[�h����");
        return returndic;
    }

    private void Start()
    {        

    }
}
