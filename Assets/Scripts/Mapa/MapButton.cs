using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapButton : MonoBehaviour {
    public string SceneToLoad;
    
    private void OnMouseDown()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
}
