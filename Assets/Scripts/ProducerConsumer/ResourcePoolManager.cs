using System;
using System.Collections.Generic;
using MMPoolingSystem;
using UnityEngine;

[RequireComponent(typeof(GOPoolController))]
public class ResourcePoolManager : Singleton<ResourcePoolManager>
{
    [SerializeField] private float _idleDuration = 0;
    [SerializeField] private int _minCount = 0;
    [SerializeField] private int _maxCount = Int32.MaxValue;
    [SerializeField] private List<BaseResource> _resources;

    public Dictionary<Type, GameObjectPool> ResourceToGameObjectPool = new Dictionary<Type, GameObjectPool>();
    public float IdleDuration => _idleDuration;

    public int MinCount => _minCount;
    public int MaxCount => _maxCount;

    private GOPoolController _poolController;


    public GOPoolController PoolController
    {
        get
        {
            if (_poolController == null)
                _poolController = GetComponent<GOPoolController>();

            return _poolController;
        }
    }


    private void Awake()
    {
        TryCreatePools();
    }

    private void TryCreatePools()
    {

        foreach (var resource in _resources)
        {
            GameObjectPool currentGameObjectPool = PoolController.RegisterPool(
                new GOPoolingInfo(
                    maxCount: MaxCount,
                    minCount: MinCount,
                    idleDuration: IdleDuration,
                    resource.GetComponent<GOPoolObject>()));

            ResourceToGameObjectPool[resource.GetType()] = currentGameObjectPool;


        }

    }


    public BaseResource LoadResource(BaseResource resource)
    {
        GOPoolObject goPoolObject = ResourceToGameObjectPool[resource.GetType()].TryPopPoolObject();
        return goPoolObject.GetComponent<BaseResource>();
    }
}