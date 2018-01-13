using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Chain))]
public class ChainEditor : Editor {

    public override void OnInspectorGUI()
    {
        Chain c = (Chain)target;

        EditorGUILayout.LabelField("Link quantity: " + c.LinkQuantity.ToString());
        if (GUILayout.Button("+"))
        {
            c.AddLink();
        }
        if (!Application.isPlaying && GUILayout.Button("-"))
        {
            c.RemoveLink();
        }


        DrawDefaultInspector();


    }

}
