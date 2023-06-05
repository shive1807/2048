using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DependencyManager : Singleton<DependencyManager>
{
    //public static DependencyManager Instance { get; private set; }
    //public GameController gameController;
    //public GridController gridController;
    //public InputManager inputManager;
    //private void Awake()
    //{
    //    Singleton();
    //}
    //private void Singleton()
    //{
    //    if(Instance != null && Instance != this)
    //    {
    //        Destroy(this);
    //    }
    //    else
    //    {
    //        Instance = this;
    //    }
    //}
    public InputManager     inputManager;
    public GameController   gameController;
    public GridController   gridController;
    //public Pooler           pooler;

}
