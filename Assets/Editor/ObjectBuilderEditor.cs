using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TreeRandomizer))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TreeRandomizer treeRandomizer = (TreeRandomizer)target;
        if (GUILayout.Button("Generate"))
        {
            treeRandomizer.Generate();
        }
        if (GUILayout.Button("A�a�lar� Sil"))
        {
            treeRandomizer.ClearList();
        }
    }
}