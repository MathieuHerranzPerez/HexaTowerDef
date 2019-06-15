using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapCreator))]
public class MapEditor : Editor
{
    public MapCreator current
    {
        get
        {
            return (MapCreator)target;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Clear tiles"))
            current.Clear();
        if (GUILayout.Button("Build tiles"))
            current.InitGrid();
    }
}
