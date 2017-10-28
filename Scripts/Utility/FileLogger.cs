    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class FileLogger {
    static System.Action<string> listener;
    public static void Log(string s) {
        ForceSave(s);
        if (listener != null)
            listener(s);
    }

    static void ForceSave(string s) {
        if (!Directory.Exists(Application.streamingAssetsPath + "/logs"))
            Directory.CreateDirectory(Application.streamingAssetsPath + "/logs");
        File.AppendAllText(Application.streamingAssetsPath + "/logs/" + "log " + GetFuckingTime() + ".txt", s + System.Environment.NewLine);
    }

    static string GetFuckingTime() {
        return System.DateTime.Today.Year + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Today.Day;
    }

    public static void SetLogListener(System.Action<string> method) {
        listener = method;
    }
}
