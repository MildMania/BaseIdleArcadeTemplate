using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseResourceProvider : MonoBehaviour
{
    [SerializeField] public Transform ResourceContainer;
    
    [SerializeField] protected bool _canLoadUnlimited;
    [SerializeField] protected int _loadLimit;


    
    public Action OnResourceCountUpdated { get; set; }

    public int LoadLimit => _loadLimit;



    public abstract int GetResourceCount();

    public bool CanLoad()
    {
        if (_canLoadUnlimited)
        {
            return true;
        }

        return GetResourceCount() < _loadLimit;

    }


}

public abstract class BaseResourceProvider<TResource> : BaseResourceProvider where TResource : BaseResource
{
    public List<TResource> Resources { get; } = new List<TResource>();

    public override int GetResourceCount()
    {
        return Resources.Count;
    }
}