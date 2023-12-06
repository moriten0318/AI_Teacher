using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Diagnostics;

public class PythonRunner : MonoBehaviour
{
    void Start()
    {
        RunPythonScript();
    }

    void RunPythonScript()
    {
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "C://Users//moris//AppData//Local//Programs//Python//Python310//python.exe"; // Pythonのパス（例: "C:/Python39/python.exe"）
        start.Arguments = "C://Users//moris//Desktop//Mypython//AITeacher_python//UDP_Teacher.py"; // スクリプトのパス
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        using (Process process = Process.Start(start))
        {
            using (StreamReader reader = new StreamReader(process.StandardOutput.BaseStream, Encoding.UTF8))
            {
                string result = reader.ReadToEnd();
                UnityEngine.Debug.Log(result);
            }
        }
    }
}
