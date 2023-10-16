using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string optionsSceneName = "OptionsScene";

    // Start is called before the first frame update
    void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene(optionsSceneName);
    }

    public void QuitGame()
    {
        Application.Quit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
