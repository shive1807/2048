using UnityEngine;
using System.Collections.Generic;

public class Pooler : Singleton<Pooler>
{
    private GameObject element;
    private Queue<GameObject> pool;

    public int BlockCount = 100;
    public int BufferBlockCount = 20;

    protected override void Awake()
    {
        element = Resources.Load<GameObject>(Assets.numElement);
        pool = new Queue<GameObject>();

        BlockCount = GameSettings.GRID_HEIGHT * GameSettings.GRID_WIDTH;
        Initialize();
    }

    private void Initialize()
    {
        int poolCount = BlockCount + BufferBlockCount;

        for (int i = 0; i < poolCount + 20; i++)
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