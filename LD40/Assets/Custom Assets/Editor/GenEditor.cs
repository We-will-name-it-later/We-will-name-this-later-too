using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class GenEditor : Editor {

    public override void OnInspectorGUI()
    {
        LevelGenerator gen = (LevelGenerator)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            gen.GenerateLevel();
        }
        if (GUILayout.Button("Destroy"))
        {
            gen.DestroyGrid();
        }

    }
}
