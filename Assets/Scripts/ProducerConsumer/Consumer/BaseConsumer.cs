using System;
using System.Collections;
using UnityEngine;

public abstract class BaseConsumer : MonoBehaviour
{
    [SerializeField] protected UpdatedFormationController _updatedFormationController;

    [SerializeField] protected AIInteraction _aiInteraction;
    public AIInteraction AiInteraction => _aiInteraction;
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
    }

    public abstract void ConsumeCustomActions(TResource resource);

    public bool IsAiInteractible()
    {
        return _aiInteraction != null;
    }
}