using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PhysicsObject))]
public class PhysicsObjectEditor : Editor {
    private float _size = 1.0f;
    private Vector3 _snap = Vector3.one;

    public override void OnInspectorGUI()
    {
        EditorUtility.SetDirty(target);
        PhysicsObject obj = (PhysicsObject)target;
        DrawDefaultInspector();

        obj.Limit1 = EditorGUILayout.Vector3Field("Limit 1:", obj.Limit1);
        obj.Limit2 = EditorGUILayout.Vector3Field("Limit 2:", obj.Limit2);
	}

    public void OnSceneGUI()
    {
        PhysicsObject obj = (PhysicsObject)target;
        if (obj.ShowBoundingBox)
        {
            //draws rectangle
            Rect drawRect = new Rect(Mathf.Min(obj.Limit1.x, obj.Limit2.x), Mathf.Min(obj.Limit1.y, obj.Limit2.y), Mathf.Abs(obj.Limit1.x - obj.Limit2.x), Mathf.Abs(obj.Limit1.y - obj.Limit2.y));
            Handles.DrawSolidRectangleWithOutline(drawRect, new Color(0.5f, 0.5f, 0.5f, 0.2f), Color.black);

            //draw handles
            EditorGUI.BeginChangeCheck();
            obj.Limit1 = Handles.FreeMoveHandle(obj.Limit1, Quaternion.identity, _size, _snap, Handles.RectangleHandleCap);
            obj.Limit2 = Handles.FreeMoveHandle(obj.Limit2, Quaternion.identity, _size, _snap, Handles.RectangleHandleCap);
            EditorGUI.EndChangeCheck();
        }
    }
}
