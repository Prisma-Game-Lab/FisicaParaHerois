using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Chain))]
public class ChainEditor : Editor {

    public override void OnInspectorGUI()
    {
        EditorUtility.SetDirty(target);
        Chain c = (Chain)target;



        PrefabUtility.DisconnectPrefabInstance(c);

        EditorGUILayout.LabelField("Link quantity: " + c.LinkQuantity.ToString());
        if (GUILayout.Button("+"))
        {
            c.AddLink();
        }
        if (!Application.isPlaying && GUILayout.Button("-"))
        {
            c.RemoveLink();
        }

        EditorGUILayout.LabelField("Force exerted: " + c.ForceExerted);
        c.BreakForce = EditorGUILayout.FloatField("Break force", c.BreakForce);

        DrawDefaultInspector();


    }

}
