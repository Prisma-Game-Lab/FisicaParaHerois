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

        obj.LimitMin = EditorGUILayout.Vector3Field("Limit Min:", obj.LimitMin);
        obj.LimitMax = EditorGUILayout.Vector3Field("Limit Max:", obj.LimitMax);
	}

    public void OnSceneGUI()
    {
        PhysicsObject obj = (PhysicsObject)target;
        if (obj.ShowBoundingBox)
        {
            //draws rectangle
            Rect drawRect = new Rect(Mathf.Min(obj.LimitMin.x, obj.LimitMax.x), Mathf.Min(obj.LimitMin.y, obj.LimitMax.y), Mathf.Abs(obj.LimitMin.x - obj.LimitMax.x), Mathf.Abs(obj.LimitMin.y - obj.LimitMax.y));
            Handles.DrawSolidRectangleWithOutline(drawRect, new Color(0.5f, 0.5f, 0.5f, 0.2f), Color.black);

            //draw handles
            EditorGUI.BeginChangeCheck();
            obj.LimitMin = Handles.FreeMoveHandle(obj.LimitMin, Quaternion.identity, _size, _snap, Handles.RectangleHandleCap);
            obj.LimitMax = Handles.FreeMoveHandle(obj.LimitMax, Quaternion.identity, _size, _snap, Handles.RectangleHandleCap);
            EditorGUI.EndChangeCheck();
        }
    }
}
