using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{


    public override void OnInspectorGUI()
    {
		DrawDefaultInspector();
		MapGenerator myScript = (MapGenerator)target;
        
        if (GUILayout.Button("Load Map from File"))
        {
            myScript.LoadMap();
        }
        
        if (GUILayout.Button("Generate Map"))
        {
            myScript.GenerateMap();
        }
        if (GUILayout.Button("Delete Old Map"))
        { 
          myScript.DeleteMap();
        }
    }
}
