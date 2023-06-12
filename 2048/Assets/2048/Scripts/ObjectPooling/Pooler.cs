using UnityEngine;
using System.Collections.Generic;

public class Pooler : MonoBehaviour
{
    private int poolCount = 0;

    private GameObject element;
    private List<GameObject> pool;

    private void Start()
    {
        pool        = new List<GameObject>();
        element     = Resources.Load<GameObject>(Assets.numElement);
        poolCount   = GameSettings.GRID_HEIGHT * GameSettings.GRID_WIDTH;

        Initialize();
    }

    private void Initialize()
    {
        for(int i = 0; i < poolCount; i++)
        {
            GameObject _element = Instantiate(element);
            _element.transform.SetParent(this.transform, false);
            _element.SetActive(false);
            pool.Add(_element);
        }
    }

    public GameObject SpawnfromPool(Vector2 Pos = default)
    {
        foreach(GameObject element in pool)
        {
            if (!element.activeInHierarchy)
            {
                element.SetActive(true);
                element.transform.position = Pos;
                return element;
            }
        }
        return null;
    }

    public void Deactivate(GameObject element)
    {
        element.SetActive(false);
        element.transform.position = Vector2.zero;
    }
}
