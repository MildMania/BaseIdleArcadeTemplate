using System.Collections.Generic;
using UnityEngine;

public abstract class BaseResourceProvider : MonoBehaviour
{
    [SerializeField] public Transform ResourceContainer;

    public abstract int GetResourceCount();
}

public abstract class BaseResourceProvider<TResource> : BaseResourceProvider where TResource : BaseResource
{
    public List<TResource> Resources { get; } = new List<TResource>();

    public override int GetResourceCount()
    {
        return Resources.Count;
    }
}