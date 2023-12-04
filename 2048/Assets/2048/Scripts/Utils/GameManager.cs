using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameData gameData = new GameData();

    private void Start()
    {
        LoadGameData();

        if(gameData.FirstLogin != 1)
        {
            StartCoroutine(FetchDataFromDatabase());
        }
    }
    IEnumerator FetchDataFromDatabase()
    {
        yield return new WaitForSeconds(.1f);
        //DatabaseRealtimeManager.Instance.RetrieveData(gameData.User.UserID);
    }
    public void LoadGameData()
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
