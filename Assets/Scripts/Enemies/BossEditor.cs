using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Boss))]
public class BossEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Boss boss = (Boss)target;

       // if (GUILayout.Button("Add State"))
       // {
       //
       // }
    }
}
