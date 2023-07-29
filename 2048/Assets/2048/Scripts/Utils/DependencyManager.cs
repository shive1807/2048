using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DependencyManager : MonoBehaviour
{
    public static DependencyManager Instance;

    public GameManager      gameManager;
    public InputManager     inputManager;
    public GameController   gameController;
    public GridController   gridController;
    public Pooler           pooler;
    public Popup            popup;
    public GameObject       newBlockPopup;
    public GemsManager      gemsManager;
    public VFX              vfx;
    public PauseMenu        pauseMenu;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this as DependencyManager;
        }
    }
}
