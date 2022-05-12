using System;
using System.Collections;
using UnityEngine;

public abstract class BaseConsumer : MonoBehaviour
{
    [SerializeField] protected UpdatedFormationController _updatedFormationController;
    [SerializeField] protected AIInteraction _aiInteraction;
    public AIInteraction AiInteraction => _aiInteraction;
    public Action<int> OnResourcesUpdated { get; set; }
    public abstract bool IsFull();
}

public abstract class BaseConsumer<TResource> : BaseConsumer, IConsumer<TResource>
    where TResource : BaseResource
{
    [SerializeField] protected BaseResourceProvider<TResource> _baseResourceProvider;

    public Action<BaseConsumer<TResource>, TResource> OnConsumeFinished;


    public BaseResourceProvider<TResource> ResourceProvider
    {
        get => _baseResourceProvider;
        set => _baseResourceProvider = value;
    }

    public void Consume(TResource resource)
    {
        ResourceProvider.Resources.Remove(resource);
        ConsumeCustomActions(resource);
        _updatedFormationController.RemoveLastTransform();
        OnResourcesUpdated?.Invoke(ResourceProvider.GetResourceCount());
    }

    public abstract void ConsumeCustomActions(TResource resource);

    public bool IsAiInteractible()
    {
        return _aiInteraction != null;
    }

    public override bool IsFull()
    {
        return !_baseResourceProvider.CanLoad();
    }

    public void AddToResourceProvider(TResource resource)
    {
        ResourceProvider.Resources.Add(resource);
        OnResourcesUpdated?.Invoke(ResourceProvider.GetResourceCount());
    }
}