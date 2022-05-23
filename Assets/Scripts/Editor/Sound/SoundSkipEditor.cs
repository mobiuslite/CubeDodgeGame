using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundSkip))]
public class SoundSkipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Skip 5 seconds"))
        {
            SoundSkip skipScript = (SoundSkip)target;

            skipScript.StartSkipTime(5.0f);
        }
    }
}
