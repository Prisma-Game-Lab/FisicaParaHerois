using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapButton : MonoBehaviour {
    public string SceneToLoad;
    public bool Completed = false;
    private Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();

        // Checar se a fase foi completa e atualizar variável "Completed"
        Anim.SetBool("Completed", Completed);

    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
}
