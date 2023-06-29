using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DependencyManager : Singleton<DependencyManager>
{
    public GameManager      gameManager;
    public InputManager     inputManager;
    public GameController   gameController;
    public GridController   gridController;
    public Pooler           pooler;
    public Popup            popup;
    public GameObject       newBlockPopup;
}
