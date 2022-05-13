using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Boss), true)]
public class BossEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Boss boss = (Boss)target;

        var style = GUI.skin.GetStyle("label");
        style.fontSize = 24;

        GUILayout.Label("Current state: " + boss.GetCurrentState(), style);
    }
}
