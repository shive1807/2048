using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    public List<GameObject> pool;
    public GameObject element;
    private int poolCount = 0;

    void Start()
    {
        poolCount = DependencyManager.Instance.gridController.rows * DependencyManager.Instance.gridController.cols;
        pool = new List<GameObject>();
        Initialize();
    }
    private void Initialize()
    {
        for(int i = 0; i < poolCount; i++)
        {
            GameObject _element = Instantiate(element);
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
