using System;
using MMFramework.TasksV2;
using UnityEngine;

//public class ConsumptionController : MonoBehaviour
//{

//}

public class ConsumptionController<TConsumer, TResource> : MonoBehaviour where TConsumer : BaseConsumer<TResource>
    where TResource : IResource
{
    [SerializeField] protected TConsumer _consumer;
    [SerializeField] private BaseResourceProvider<TResource> _resourceProvider;
    public bool IsAvailable { get; set; } = true;

    private int _consumptionCount;
    private Action _onConsumptionFinished;

    [SerializeField] private MMTaskExecutor _onConsumptionStartedTasks;
    [SerializeField] private MMTaskExecutor _onConsumptionStoppedTasks;

    public void StartConsumption(int amount, Action onConsumedCallback)
    {
        if (IsAvailable)
        {
            IsAvailable = false;
            _consumptionCount = amount;
            _onConsumptionFinished = onConsumedCallback;

            if (_onConsumptionStartedTasks!= null && amount != 0)
                _onConsumptionStartedTasks.Execute(this);

            for (int i = 0; i < amount; i++)
            {
                TResource resource = _resourceProvider.Resources[_resourceProvider.Resources.Count - 1];
                //TODO: Think about if it is the best way to remove resource here!
                _consumer.Consume(resource);
                _consumer.OnConsumeFinished += OnConsumeFinished;
            }
        }
    }

    private void OnConsumeFinished(BaseConsumer<TResource> consumer, TResource resource)
    {
        consumer.OnConsumeFinished -= OnConsumeFinished;
        if (--_consumptionCount == 0)
        {
            _onConsumptionFinished?.Invoke();
            IsAvailable = true;

            if (_resourceProvider.ResourceContainer.childCount == 0 && _onConsumptionStoppedTasks != null)
                _onConsumptionStoppedTasks.Execute(this);

        }
    }
}