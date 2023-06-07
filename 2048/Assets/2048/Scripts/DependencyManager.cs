using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DependencyManager : Singleton<DependencyManager>
{
    public InputManager     inputManager;
    public GameController   gameController;
    public GridController   gridController;
    public Pooler pooler;
}
