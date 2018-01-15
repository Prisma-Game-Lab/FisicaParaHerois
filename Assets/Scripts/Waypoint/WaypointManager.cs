using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaypointManager : MonoBehaviour
{
    
    public List<PathInfo> paths;

    public void Start()
    {
        
        foreach (PathInfo path in paths)
        {
            path.CalculateSampledPoints();
            
        }
    }

    public void Update()
    {
        
    }

    public PathInfo GetPath(string name)
    {
        foreach(PathInfo path in paths)
        {
            if (path.name == name) return path;
        }
        return null;
    }


}




public static class WaypointExtensions
{
    //adiciona ao objeto associado ao transform um componente que o faz seguir o path
    public static void FollowPath(this Transform trans, string pathname)
    {
        WaypointFollower.CreateWaypointFollowerIn(trans.gameObject, pathname);
    }
}
