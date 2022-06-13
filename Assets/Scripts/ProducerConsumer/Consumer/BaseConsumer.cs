using System;
using MMFramework.TasksV2;
using UnityEngine;

public abstract class BaseConsumer : MonoBehaviour
{
    [SerializeField] protected MMTaskExecutor _onMoveFinishedTaskExecutor;
    [SerializeField] protected UpdatedFormationController _updatedFormationController;
    [SerializeField] protected AIInteraction _aiInteraction;
    public AIInteraction AiInteraction => _aiInteraction;
    public Action<int> OnResourcesRemoved { get; set; }
    public Action<int> OnResourcesAdded { get; set; }
    
    public Action OnConsumed;
    public abstract bool IsFull();
    public abstract bool IsEmpty();
    public abstract BaseResourceProvider GetResourceProvider();
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
        OnResourcesRemoved?.Invoke(ResourceProvider.GetResourceCount());
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

    public override bool IsEmpty()
    {
        return _baseResourceProvider.GetResourceCount() == 0;
    }

    public override BaseResourceProvider GetResourceProvider()
    {
        return ResourceProvider;
    }
    public void AddToResourceProvider(TResource resource)
    {
        ResourceProvider.Resources.Add(resource);
        OnResourcesAdded?.Invoke(ResourceProvider.GetResourceCount());
    }
}