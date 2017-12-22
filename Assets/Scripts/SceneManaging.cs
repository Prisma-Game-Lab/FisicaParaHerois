using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManaging : MonoBehaviour {

	public void LoadScene(string s)
    {
        SceneManager.LoadScene(s);
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
        //Lê de um arquivo gerado pelo Unity se o personagem já foi criado
        int characterCreated = PlayerPrefs.GetInt("CharacterCreated", 0);

        //Se não, vai para a cena de customização
        if(characterCreated == 0)
        {
            LoadScene(customizationScene);
        }

        //Se foi, vai para a cena do mapa
        else
        {
            LoadScene(mapScene);
        }

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
}
