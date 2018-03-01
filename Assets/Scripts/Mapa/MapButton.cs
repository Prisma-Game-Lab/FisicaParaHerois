using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapButton : MonoBehaviour {
	public static Dictionary<int,bool> CompletedLevels;
    public string SceneToLoad;
    public bool Completed = false;
	public int index;
    private Animator Anim;

	private void Awake(){
		if (CompletedLevels == null) {
			CompletedLevels = new Dictionary<int,bool>();
			CompletedLevels.Add (index, LevelStatus.ReadCompleted(index));
		} 

		else if (!CompletedLevels.ContainsKey (index)) {
			CompletedLevels.Add (index, LevelStatus.ReadCompleted(index));
		}

		Completed = CompletedLevels[index];
	}

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
