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

        EditorGUILayout.LabelField("Values below are reproduced in every link in the chain");
        c.LinkScale = EditorGUILayout.Vector3Field("Scale", c.LinkScale);
        c.LinkMass = EditorGUILayout.FloatField("Mass", c.LinkMass);
        c.LinkBreakForce = EditorGUILayout.FloatField("Link break force", c.LinkBreakForce);


        DrawDefaultInspector();


    }

}
