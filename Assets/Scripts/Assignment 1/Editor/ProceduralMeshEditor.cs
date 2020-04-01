using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralMesh))]
public class ProceduralMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProceduralMesh script = (ProceduralMesh)target;
        if (GUILayout.Button("Generate Mesh")) {
            script.GenerateNewMesh();
        }
    }
}
