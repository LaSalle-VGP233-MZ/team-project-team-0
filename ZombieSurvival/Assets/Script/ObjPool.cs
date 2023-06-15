using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _objPool;
    [SerializeField]
    private int _initialPoolSize;

    private Queue<GameObject> _pool;

    private void Awake()
    {
        _pool = new Queue<GameObject>();
    }

    void Start()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreateObjInPool();
        }
    }

    public GameObject GiveMeAnObjAt(Vector3 pos)
    {
        if(_pool.Count != 0)
        {
            GameObject obj = _pool.Dequeue();
            obj.transform.position = pos;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(_objPool);
            obj.transform.position = pos;
            return obj;
        }
    }

    public void ReturnToQueue(GameObject obj)
    {
        _pool.Enqueue(obj);
    }

    private void CreateObjInPool()
    {
        GameObject obj = Instantiate(_objPool);
        obj.SetActive(false);
        obj.transform.parent = this.transform;
        _pool.Enqueue(obj);
    }
}
