using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

    public static bool EnableEditorLog = true;

    public static void EditorLog(string s) {
#if UNITY_EDITOR
        if(EnableEditorLog)
            Debug.Log(s);
#endif
    }
    public static void EditorLog(object s)
    {
#if UNITY_EDITOR
        if (EnableEditorLog)
            Debug.Log(s.ToString());
#endif
    }
}
