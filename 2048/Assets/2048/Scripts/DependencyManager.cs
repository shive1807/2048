using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependencyManager : MonoBehaviour
{
    public static DependencyManager Instance { get; private set; }

    public GameController gameController;
    public GridController gridController;
    public InputManager inputManager;

    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}