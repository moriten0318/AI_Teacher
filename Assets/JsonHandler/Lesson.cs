using UnityEngine;
using System.IO;

[System.Serializable]
public class Lesson
{
    public string 内容の説明;
    public string キーワード;
    public string 板書;
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