using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TimedCircleAttack))]
public class TimedCircleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TimedCircleAttack attack = (TimedCircleAttack)target;
        if(GUILayout.Button("Test Attack"))
        {
            attack.StartAttack();
        }
    }
}
