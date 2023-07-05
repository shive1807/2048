using UnityEngine;
using System.Collections.Generic;

public class Pooler : Singleton<Pooler>
{
    private int poolCount = 0;

    private GameObject element;
    private Queue<GameObject> pool;

    protected override void Awake()
    {
        element     = Resources.Load<GameObject>(Assets.numElement);
        pool        = new Queue<GameObject>();
        poolCount   = GameSettings.GRID_HEIGHT * GameSettings.GRID_WIDTH;

        Initialize();
    }

    private void Initialize()
    {
        for(int i = 0; i < poolCount; i++)
        {
            GameObject _element = Instantiate(element);
            _element.transform.SetParent(this.transform, true);
            _element.SetActive(false);
            pool.Enqueue(_element);
        }
    }

    public GameObject SpawnfromPool()
    {
        var element = pool.Dequeue();
        element.SetActive(true);
        return element;
    }

    public void Deactivate(GameObject element)
    {
        element.SetActive(false);
        pool.Enqueue(element);
    }
}
