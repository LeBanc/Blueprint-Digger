using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataManager))]
[CanEditMultipleObjects]
public class DataManager_editor : Editor
{
    private DataManager _data;

    public void OnEnable()
    {
        _data = (DataManager)target;
    }

    public override void OnInspectorGUI()
    {
        if( GUILayout.Button("Reset Levels"))
        {
            _data.ResetLevels();
        }
    }
}
