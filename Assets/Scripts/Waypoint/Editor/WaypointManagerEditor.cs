using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointManager))]
public class NewBehaviourScript : Editor
{

    Handles handle;
    Vector3 newPos;

    
    private List<bool> isSelected;

    float size = 0.2f;
    Vector3 snap = Vector3.one * 1.0f;

    public void OnEnable()
    {
        WaypointManager wm = (WaypointManager)target;

        if (wm.paths == null) wm.paths = new List<PathInfo>();
        

        isSelected = new List<bool>();

        for (int i = 0; i < wm.paths.Count; i++)
        {
            isSelected.Add(false);
        }
        
    }

    public override void OnInspectorGUI()
    {
        WaypointManager wm = (WaypointManager)target;

        DrawDefaultInspector();

        //for (int i = 0; i < wm.paths.Count; i++)
        //{
        //    isSelected[i] = GUILayout.Toggle(isSelected[i], wm.paths[i].name);

        //    if (isSelected[i])
        //    {
        //        //display/edit path attributes
        //        //wm.paths[i].name = EditorGUILayout.TextField(wm.paths[i].name);
        //        //wm.paths[i].ctype = (CurveType)EditorGUILayout.EnumPopup("Curve Type:", wm.paths[i].ctype);

                

        //        //SerializedObject so = new SerializedObject(wm.paths[i]);
        //        //var prop = so.FindProperty("_controlPoints");
        //        //if (prop == null)
        //        //{
        //        //    Debug.Log("null");
        //        //}
        //        ////display/edit paths properties, and include the children
        //        //EditorGUILayout.PropertyField(prop, true);
        //        //so.ApplyModifiedProperties();


        //        if (GUILayout.Button("Add Point"))
        //        {
        //            wm.paths[i].AddControlPoint(Vector3.zero);
        //            return;
        //        }
        //    }

        //}

        if (GUILayout.Button("Add new Path"))
        {
            wm.paths.Add(new PathInfo("New Path"));
            isSelected.Add(false);
        }


    }



    protected virtual void OnSceneGUI()
    {

        WaypointManager waypointmanager = (WaypointManager)target;

        for (int i = 0; i < waypointmanager.paths.Count; i++)
        {
            PathInfo path = waypointmanager.paths[i];

            //if path is not selected, dont draw it on screen
            if (!path.display) continue;

            //draw control point handles and update them, acording to input
            for (int j = 0; j < path.ControlPoints.Count; j++)
            {
                Handles.color = Color.blue;
                EditorGUI.BeginChangeCheck();
                Vector3 newPos = Handles.FreeMoveHandle(path.ControlPoints[j], Quaternion.identity, size * HandleUtility.GetHandleSize(path.ControlPoints[j]), snap, Handles.SphereHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(waypointmanager, "Change Control Point Position");
                    path.AlterControlPoint(newPos, j);
                    waypointmanager.Update();
                }

                if (path.ctype == CurveType.Bezier)
                {
                    //draws and updates bezier handles
                    
                    
                    //o primeiro ponto de controle não tem um ponto de bezier "para trás"
                    if (j != 0)
                    {
                        Handles.color = Color.gray;
                        Handles.DrawLine(path.BackwardBezierPoints[j] + path.ControlPoints[j], path.ControlPoints[j]);
                        Handles.color = Color.red;
                        EditorGUI.BeginChangeCheck();
                        Vector3 pos3 = Handles.FreeMoveHandle(path.BackwardBezierPoints[j] + path.ControlPoints[j], Quaternion.identity, 0.5f * size * HandleUtility.GetHandleSize(path.BackwardBezierPoints[j] + path.ControlPoints[j]), snap, Handles.SphereHandleCap);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(waypointmanager, "Change Bezier Point Position");
                            Vector3 newBezier = pos3 - path.ControlPoints[j];
                            //rotação entre o ponto antigo e o novo. Ponto "oposto" deve ser rotacionado junto
                            Quaternion rot = Quaternion.FromToRotation(path.BackwardBezierPoints[j], newBezier);
                            //rotaciona ponto "oposto"(o forward bezier point do mesmo controlPoint)
                            //o último ponto tem um desses?
                            path.AlterForwardBezierPoint(rot * path.ForwardBezierPoints[j], j);

                            path.AlterBackwardBezierPoint(newBezier, j);
                            waypointmanager.Update();
                        }

                        

                    }
                    //o último ponto de controle não tem um ponto de bezier forward
                    if (j != path.ControlPoints.Count - 1)
                    {
                        Handles.color = Color.gray;
                        Handles.DrawLine(path.ForwardBezierPoints[j] + path.ControlPoints[j], path.ControlPoints[j]);
                        Handles.color = Color.green;
                        EditorGUI.BeginChangeCheck();
                        Vector3 pos3 = Handles.FreeMoveHandle(path.ForwardBezierPoints[j] + path.ControlPoints[j], Quaternion.identity, 0.5f * size * HandleUtility.GetHandleSize(path.ForwardBezierPoints[j] + path.ControlPoints[j]), snap, Handles.SphereHandleCap);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(waypointmanager, "Change Bezier Point Position");
                            Vector3 newBezier = pos3 - path.ControlPoints[j];
                            //rotação entre o ponto antigo e o novo. Ponto "oposto" deve ser rotacionado junto
                            Quaternion rot = Quaternion.FromToRotation(path.ForwardBezierPoints[j], newBezier);
                            //rotaciona ponto "oposto"(o forward bezier point do mesmo controlPoint)
                            //o primeiro ponto tem um desses?
                            path.AlterBackwardBezierPoint(rot * path.BackwardBezierPoints[j], j);

                            path.AlterForwardBezierPoint(newBezier, j);
                            waypointmanager.Update();
                        }
                    }

                    //draws bezier curve between all points
                    for(int k = 0; k < path.ControlPoints.Count-1; k++)
                    {
                        Handles.DrawBezier(path.ControlPoints[k], path.ControlPoints[k + 1], path.ControlPoints[k] + path.ForwardBezierPoints[k], path.ControlPoints[k + 1]+ path.BackwardBezierPoints[k + 1], Color.cyan, null, 2.0f);
                    }

                    
                }
            }

            if(path.ctype == CurveType.Straight)
            {
                //draw line between all points
                Handles.color = Color.cyan;
                Vector3[] array = new Vector3[path.ControlPoints.Count];
                path.ControlPoints.CopyTo(array, 0);
                Handles.DrawPolyLine(array);
            }
            
            
        }


    }

}



[CustomPropertyDrawer(typeof(PathInfo))]
public class PathInfoPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);
        
        //List<Vector3> points = property.FindPropertyRelative("_controlPoints");
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }

}