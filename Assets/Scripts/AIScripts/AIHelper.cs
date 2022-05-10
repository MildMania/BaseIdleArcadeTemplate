using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class AIHelper : SerializedMonoBehaviour
{
    [OdinSerialize] private BaseResource _baseResource;
    public BaseResource BaseResource => _baseResource;

    [OdinSerialize] private Dictionary<BaseResource, BaseLoadBehaviour> _resourceToLoadBehaviour;
    [OdinSerialize] private Dictionary<BaseResource, BaseUnloadBehaviour> _resourceToUnloadBehaviour;

    private BaseLoadBehaviour _currentLoadBehaviour;
    public BaseLoadBehaviour CurrentLoadBehaviour => _currentLoadBehaviour;

    private BaseUnloadBehaviour _currentUnloadBehaviour;
    public BaseUnloadBehaviour CurrentUnloadBehaviour => _currentUnloadBehaviour;


    private void Start()
    {
        foreach (var item in _resourceToLoadBehaviour)
        {
            if (item.Key.GetType() != _baseResource.GetType())
            {
                item.Value.StopLoading();
            }
            else
            {
                _currentLoadBehaviour = item.Value;
                _currentLoadBehaviour.Deactivate();
            }
        }

        foreach (var item in _resourceToUnloadBehaviour)
        {
            if (item.Key.GetType() != _baseResource.GetType())
            {
                item.Value.StopUnloading();
            }
            else
            {
                _currentUnloadBehaviour = item.Value;
                _currentUnloadBehaviour.Deactivate();
            }
        }
    }

    public List<BaseConsumer> GetConsumers()
    {
        return ConsumerProvider.Instance.GetConsumers(_baseResource.GetType());
    }

    public List<BaseProducer> GetProducers()
    {
        return ProducerProvider.Instance.GetProducers(_baseResource.GetType());
    }

    public void ReserveProducer(BaseProducer producer)
    {
        ProducerProvider.Instance.ReserveProducer(_baseResource.GetType(), producer);
    }

    public void ReleaseProducer(BaseProducer producer)
    {
        ProducerProvider.Instance.ReleaseProducer(_baseResource.GetType(), producer);
    }

    public void ReserveConsumer(BaseConsumer consumer)
    {
        ConsumerProvider.Instance.ReserveConsumer(_baseResource.GetType(), consumer); 
    }

    public void ReleaseConsumer(BaseConsumer consumer)
    {
        ConsumerProvider.Instance.ReleaseConsumer(_baseResource.GetType(), consumer);
    }
}