/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapData))]
public class GenerateMapButton : Editor {

    #region Public Variables

    #endregion

    #region Private Variables

    #endregion

    #region Editor
	/// <summary>
	/// Método que o Unity chama para gerar o botão no inspetor
	/// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapData script = (MapData)target;
    }
    #endregion

    #region Auxiliary Functions

    #endregion
}
