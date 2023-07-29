using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public GameData gameData;
    void Start()
    {
        gameData = SaveSystem.LoadGame();
    }
    public void ReloadGameData()
    {
        gameData = SaveSystem.LoadGame();
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
