using UnityEngine;

public abstract class BaseProducer : MonoBehaviour
{
    [SerializeField] protected AIInteraction _aiInteraction;
    public AIInteraction AiInteraction => _aiInteraction;
}

public abstract class BaseProducer<TResource> : BaseProducer, IProducer<TResource>
    where TResource : BaseResource
{
    [SerializeField] protected BaseResourceProvider<TResource> _resourceProvider;

    public void Produce(TResource resource)
    {
        _resourceProvider.Resources.Add(ProduceCustomActions(resource));
    }

    public abstract TResource ProduceCustomActions(TResource folder);


    public bool TryRemoveAndGetLastResource(ref TResource lastResource)
    {
        if (_resourceProvider.Resources.Count == 0)
        {
            return false;
        }

        lastResource = _resourceProvider.Resources[_resourceProvider.Resources.Count - 1];
        _resourceProvider.Resources.Remove(lastResource);
        TryRemoveAndGetLastProducibleCustomActions();
        return true;
    }


    protected abstract void TryRemoveAndGetLastProducibleCustomActions();

    public bool IsAiInteractible()
    {
        return _aiInteraction != null;
    }
}