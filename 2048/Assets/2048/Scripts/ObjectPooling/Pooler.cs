using UnityEngine;
using System.Collections.Generic;

public class Pooler : Singleton<Pooler>
{
    private GameObject element;
    private Queue<GameObject> pool;

    protected override void Awake()
    {
        element = Resources.Load<GameObject>(Assets.numElement);
        pool = new Queue<GameObject>();

        Initialize();
    }

    private void Initialize()
    {
        int poolCount = GameSettings.GRID_HEIGHT * GameSettings.GRID_WIDTH;

        for (int i = 0; i < poolCount; i++)
        {
            GameObject _element = Instantiate(element);
            _element.transform.SetParent(this.transform, true);
            _element.SetActive(false);
            pool.Enqueue(_element);
        }
    }

    public GameObject GetBlock()
    {
        var element = pool.Dequeue();
        element.SetActive(true);
        return element;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    public void DestroyBlock(GameObject element)
    {
        element.SetActive(false);
        element.GetComponent<Element>().selected = false;
        pool.Enqueue(element);
    }
}