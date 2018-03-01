using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManaging : MonoBehaviour {
	public float StartButtonAnimationTime;
	public Animator StartButtonAnimation;
	public AudioClip Inicio;

	public void LoadScene(string s)
    {
        SceneManager.LoadScene(s);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Usada no botão de iniciar
    /// </summary>
    public void StartGameButton(string customizationScene, string mapScene)
    {
		Debug.Log("Iniciou");
		AudioSource.PlayClipAtPoint (Inicio, new Vector3 (5, 1, 2));
		StartCoroutine ("StartButtonDelay", new string[] {customizationScene, mapScene});

        return;
    }

    /// <summary>
    /// Usada no botão de iniciar
    /// </summary>
    /// <param name="scenes">Nome das cenas separado por vírgula</param>
    public void StartGameButton(string scenes)
    {
        //Separa a string passada em 2 (o inspector não aceita métodos com 2 variáveis)
        string customizationScene = scenes.Substring(0, scenes.IndexOf(','));
        string mapScene = scenes.Substring(scenes.IndexOf(',') + 1);

        StartGameButton(customizationScene, mapScene);
    }

	public IEnumerator StartButtonDelay(string[] scenes){
		//Lê de um arquivo gerado pelo Unity se o personagem já foi criado
		int characterCreated = PlayerPrefs.GetInt("CharacterCreated", 0);
		StartButtonAnimation.Play ("scaleAnim");

		yield return new WaitForSeconds (StartButtonAnimationTime);

		//Se não, vai para a cena de customização
		if(characterCreated == 0)
		{
			LoadScene(scenes[0]);
		}

		//Se foi, vai para a cena do mapa
		else
		{
			LoadScene(scenes[1]);
		}
	}

    /// <summary>
    /// Usada no botão da cena de customização
    /// </summary>
    public void CustomizationSceneOkButton(string cutsceneScene, string mapScene)
    {
        if (PlayerPrefs.GetInt("CharacterCreated", 0) == 0) //Personagem ainda não foi setado
        {
            //Salva em um arquivo gerado pelo Unity que o personagem já foi criado
            PlayerPrefs.SetInt("CharacterCreated", 1);
            LoadScene(cutsceneScene);
        }

        else
        {
            LoadScene(mapScene);
        }
    }

    /// <summary>
    /// Usada no botão da cena de customização
    /// </summary>
    /// <param name="scenes">Nome das cenas separado por vírgula</param>
    public void CustomizationSceneOkButton(string scenes)
    {
        //Separa a string passada em 2 (o inspector não aceita métodos com 2 variáveis)
        string cutsceneScene = scenes.Substring(0, scenes.IndexOf(','));
        string mapScene = scenes.Substring(scenes.IndexOf(',') + 1);

        CustomizationSceneOkButton(cutsceneScene, mapScene);
    }

	public static string GetScene(){
		return SceneManager.GetActiveScene().name;
	}
}
