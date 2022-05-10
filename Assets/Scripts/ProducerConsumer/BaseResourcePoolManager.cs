using System;
using MMPoolingSystem;
using UnityEngine;

[RequireComponent(typeof(GOPoolController))]
public class BaseResourcePoolManager<TResource> : Singleton<BaseResourcePoolManager<TResource>>
    where TResource : BaseResource
{
    [SerializeField] private float _idleDuration = 0;
    [SerializeField] private int _minCount = 0;
    [SerializeField] private int _maxCount = Int32.MaxValue;
    [SerializeField] private TResource _resource;
    public float IdleDuration => _idleDuration;

    public int MinCount => _minCount;
    public int MaxCount => _maxCount;


    private GameObjectPool _currentGameObjectPool;
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
        _currentGameObjectPool = PoolController.RegisterPool(
            new GOPoolingInfo(
                maxCount: MaxCount,
                minCount: MinCount,
                idleDuration: IdleDuration,
                _resource.GetComponent<GOPoolObject>()));
    }


    public TResource LoadResource()
    {
        GOPoolObject goPoolObject = _currentGameObjectPool.TryPopPoolObject();
        return goPoolObject.GetComponent<TResource>();
    }
}