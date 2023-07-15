using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void OnHomePressed()
    {
        GameManager.Instance.LoadScene("MainMenu");
    }
    public void OnRestartPressed()
    {
        GameManager.Instance.ReloadScene();
        SaveSystem.ResetGrid();
    }
}
