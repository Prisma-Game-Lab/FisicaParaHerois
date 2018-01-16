using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
//used only for the bezier function
using UnityEditor;


public enum CurveType
{
    Straight, 
    Bezier
}

public enum RepetitionType
{
    Cycle,
    Ping_Pong,
    One_way
}

public enum FollowDirection
{
    Forward, 
    Backward
}

//auxiliary struct to encapsulate communication with WaypointFollower
public struct PathPoint
{
    public readonly int _index;
    public readonly Vector3 position;

    public PathPoint(int i, Vector3 pos)
    {
        _index = i;
        position = pos;
    }

}

[System.Serializable]
public class PathInfo
{

    public string name;

    //tells the editor whether or not to display this path on screen
    public bool display;

    
    public CurveType ctype;


    [SerializeField]
    private List<Vector3> _controlPoints;

    
    [SerializeField] // o tempo de espera para cada control point(opcional)
    private List<float> _waitTime;


    [SerializeField]
    [Tooltip("Amount of points per curve segment, when using bezier")]
    [Range(1, 1000)]
    private int _samplingAmount;

    //valores para os pontos de bezier são armazenados como deslocamentos em relação ao ponto de controle
    [SerializeField]    [HideInInspector]
    private List<Vector3> _bezier_forw;
    [SerializeField]    [HideInInspector]
    private List<Vector3> _bezier_back;

    private ReadOnlyCollection<Vector3> _readControlPoints;
    private ReadOnlyCollection<Vector3> _readBezier_forw;
    private ReadOnlyCollection<Vector3> _readBezier_back;

    private List<Vector3> _sampledPoints;

    //access properties
    public ReadOnlyCollection<Vector3> ControlPoints
    {
        get
        {
            if (_readControlPoints == null) _readControlPoints = new ReadOnlyCollection<Vector3>(_controlPoints);
            return _readControlPoints;
        }
    }

    public ReadOnlyCollection<Vector3> ForwardBezierPoints
    {
        get
        {
            if (_readBezier_forw == null) _readBezier_forw = new ReadOnlyCollection<Vector3>(_bezier_forw);
            if (_bezier_forw.Count < _controlPoints.Count)
            {
                int i;
                for (i = 0; i < _bezier_forw.Count; i++)
                {

                }
                for(; i < _controlPoints.Count; i++)
                {
                    _bezier_forw.Add(Vector3.right * 3);
                }
            }
            
            return _readBezier_forw;
        }
    }

    public ReadOnlyCollection<Vector3> BackwardBezierPoints
    {
        get
        {
            if (_readBezier_back == null) _readBezier_back = new ReadOnlyCollection<Vector3>(_bezier_back);
            if (_bezier_back.Count < _controlPoints.Count)
            {
                int i;
                for (i = 0; i < _bezier_back.Count; i++)
                {

                }
                for (; i < _controlPoints.Count; i++)
                {
                    _bezier_back.Add(Vector3.left * 3);
                }
            }

            return _readBezier_back;
        }
    }

    public PathPoint FirstPathPoint { get; private set; }
    public PathPoint LastPathPoint  { get; private set; }


    public void OnEnable()
    {
        _controlPoints = new List<Vector3>();
        _bezier_forw = new List<Vector3>(_controlPoints.Count);
        _bezier_back = new List<Vector3>(_controlPoints.Count);
        _readControlPoints = new ReadOnlyCollection<Vector3>(_controlPoints);
        _sampledPoints = new List<Vector3>();
    }

    //constructor
    public PathInfo(string name)
    {
        this.name = name;        
    }    

    //altering functionality
    public void AddControlPoint(Vector3 p)
    {
        _controlPoints.Add(p);
        CalculateSampledPoints();
    }

    public void AlterControlPoint(Vector3 np, int index)
    {
        _controlPoints[index] = np;
        CalculateSampledPoints();
    }
    

    public void AlterForwardBezierPoint(Vector3 np, int index)
    {
        _bezier_forw[index] = np;
        CalculateSampledPoints();
    }

    public void AlterBackwardBezierPoint(Vector3 np, int index)
    {
        _bezier_back[index] = np;
        CalculateSampledPoints();
    }

    //sampled points functionality, uses PathPoint struct


    //return the path point closest to desired position
    public PathPoint GetClosestPathPoint(Vector3 pos)
    {
        //how to handle no points?

        int index = -1;
        float minSqrDistance = float.MaxValue;
        if (_sampledPoints == null) CalculateSampledPoints();
        for(int i = 0; i < _sampledPoints.Count; i++)
        {
            Vector3 dist = pos - _sampledPoints[i];
            if (minSqrDistance > dist.sqrMagnitude)
            {
                minSqrDistance = dist.sqrMagnitude;
                index = i;
            }
        }
        PathPoint p = new PathPoint(index, _sampledPoints[index]);

        return new PathPoint(index, _sampledPoints[index]);
    }

    //gets next path point to follow according to wf's specifications. If needed, stops wf and/or changes its follow direction
    public PathPoint GetNextPathPoint(PathPoint pathpoint, WaypointFollower wf)
    {
        if (_sampledPoints == null) CalculateSampledPoints();

       //se for necessário esperar, espera-se
       MaybeWait(pathpoint._index, wf);

        switch(wf.followdirection)
        {
            case FollowDirection.Forward:
                if (pathpoint._index + 1 >= _sampledPoints.Count)
                {
                    switch(wf.repetitionType)
                    {
                        case RepetitionType.Cycle:
                            
                            return FirstPathPoint;
                            
                        case RepetitionType.One_way:
                            wf.stopped = true;
                            return FirstPathPoint;

                        case RepetitionType.Ping_Pong:
                            wf.followdirection = FollowDirection.Backward;
                            return new PathPoint(_sampledPoints.Count - 2, _sampledPoints[_sampledPoints.Count - 2]);
                    }
                }
                else
                {
                    return new PathPoint(pathpoint._index + 1, _sampledPoints[pathpoint._index + 1]);
                }
                break;
            case FollowDirection.Backward:
                if (pathpoint._index - 1 < 0)
                {
                    switch (wf.repetitionType)
                    {
                        case RepetitionType.Cycle:
                            return LastPathPoint;

                        case RepetitionType.One_way:
                            wf.stopped = true;
                            return LastPathPoint;

                        case RepetitionType.Ping_Pong:
                            wf.followdirection = FollowDirection.Forward;
                            return new PathPoint(1, _sampledPoints[1]);
                    }
                }
                else
                {
                    return new PathPoint(pathpoint._index - 1, _sampledPoints[pathpoint._index - 1]);
                }
                break;

                

        }        
        return pathpoint;
    }

    private void MaybeWait(int PathPointIndex, WaypointFollower wf)
    {
        //checa se o índice é exatamente um control point
        //meio roubado e estranho, mas: se é reta, todo ponto é control point
        // se é bezier, todo ponto múltiplo de _samplingAmount é control point
        if (ctype == CurveType.Straight)
        {
            //todo ponto é control point: checa se há alguma espera para este point
            if (_waitTime != null && _waitTime.Count - 1 >= PathPointIndex)
            {
                //espera
                wf.StartCoroutine(wf.Wait(_waitTime[PathPointIndex]));
                
            }
        }

        return;
    }

    public PathPoint getPreviousPathPoint(PathPoint pathpoint)
    {

        if (_sampledPoints == null) CalculateSampledPoints();
        if (pathpoint._index - 1 < 0)
        {
            return new PathPoint(_sampledPoints.Count - 1, _sampledPoints[_sampledPoints.Count - 1]);
        }
        else
        {
            return new PathPoint(pathpoint._index - 1, _sampledPoints[pathpoint._index - 1]);
        }
    }

    public void CalculateSampledPoints()
    {
        switch(ctype)
        {
            case CurveType.Straight:
                _sampledPoints = new List<Vector3>(_controlPoints);
                break;
            case CurveType.Bezier:
                _sampledPoints = new List<Vector3>();
                if ((_bezier_back != null && _bezier_back.Count == _controlPoints.Count) && (_bezier_forw != null && _bezier_forw.Count == _controlPoints.Count))
                {
                    for (int i = 0; i < _controlPoints.Count -1; i++)
                    {
                        _sampledPoints.AddRange(Handles.MakeBezierPoints(_controlPoints[i], _controlPoints[i + 1], _controlPoints[i] + _bezier_forw[i], _controlPoints[i + 1] + _bezier_back[i + 1], _samplingAmount));
                    } 
                
                }
                else _sampledPoints = new List<Vector3>(_controlPoints);
                break;
        }

        FirstPathPoint = new PathPoint(0, _sampledPoints[0]);
        LastPathPoint = new PathPoint(_sampledPoints.Count - 1, _sampledPoints[_sampledPoints.Count - 1]);

    }

    

}





