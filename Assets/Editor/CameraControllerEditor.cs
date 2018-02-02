using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor {

    private float _size = 1.0f;
    private Vector3 _snap = Vector3.one;



    public override void OnInspectorGUI()
    {
        EditorUtility.SetDirty(target);
        CameraController c = (CameraController)target;
        DrawDefaultInspector();

        EditorGUILayout.LabelField("Camera scroll bounds");
        c.Limit1 = EditorGUILayout.Vector3Field("Limit 1:", c.Limit1);
        c.Limit2 = EditorGUILayout.Vector3Field("Limit 2:", c.Limit2);

        


        //EditorGUI.BeginChangeCheck();
        //Vector3 newL1 = Handles.PositionHandle(c.Limit1, Quaternion.identity);
        //if (EditorGUI.EndChangeCheck())
        //{
        //    Undo.RecordObject(c, "Change bounds");
        //    c.Limit1 = newL1;
        //    c.Update();
        //}

    }

    public void OnSceneGUI()
    {
        CameraController c = (CameraController)target;
        if (c.showBoundingBox)
        {
            //draws rectangle
            Rect drawRect = new Rect(Mathf.Min(c.Limit1.x, c.Limit2.x), Mathf.Min(c.Limit1.y, c.Limit2.y), Mathf.Abs(c.Limit1.x - c.Limit2.x), Mathf.Abs(c.Limit1.y - c.Limit2.y));
            Handles.DrawSolidRectangleWithOutline(drawRect, new Color(0.5f, 0.5f, 0.5f, 0.2f), Color.black);

            //draw handles
            EditorGUI.BeginChangeCheck();
            c.Limit1 = Handles.FreeMoveHandle(c.Limit1, Quaternion.identity, _size, _snap, Handles.RectangleHandleCap);
            c.Limit2 = Handles.FreeMoveHandle(c.Limit2, Quaternion.identity, _size, _snap, Handles.RectangleHandleCap);
            EditorGUI.EndChangeCheck();
        }
    }
}
