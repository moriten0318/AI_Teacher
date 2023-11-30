using UnityEngine;
using System.IO;

[System.Serializable]
public class Lesson
{
    public string 時間;
    public string 学習活動;
    public string 指導上の留意点;
    public string 評価の観点;
    public string 板書;
    public string 画像;
    public string 教師の発話;
}

public static class JsonHelper
{
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }

    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }
}