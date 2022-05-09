using System;
using System.Collections;
using UnityEngine;

public class ProductionController<TProducer, TResource> : MonoBehaviour where TProducer : BaseProducer<TResource>
    where TResource : IResource
{
    [SerializeField] protected TProducer _producer;
    [SerializeField] protected TResource _resource;
    [SerializeField] private BaseRequirement[] _productionRequirements;
    
    [SerializeField] private float _productionDelay;
    protected bool IsAllRequirementMet;
    public void UpdateProductionDelay(float value)
    {
        _productionDelay = 1 / value;
    }
    
    
    protected IEnumerator ProduceRoutine(TResource resource)
    {
        if (_producer.IsAiInteractible())
        {
            ProducerProvider.Instance.AddProducer(_producer, _resource.GetType());
        }

        float currentTime = 0;

        while (true)
        {
            currentTime += Time.deltaTime;

            if (currentTime > _productionDelay)
            {
                IsAllRequirementMet = IsAllRequirementsMet();
                if (IsAllRequirementMet)
                {
                    int callbackCount = 0;
                    ConsumeAllRequirements(onConsumedCallback);

                    //Consider waiting all resources to be consumed by listening onConsumed Event!
                    void onConsumedCallback()
                    {
                        if (++callbackCount >= _productionRequirements.Length)
                        {
                            _producer.Produce(resource);
                        }
                    }
                }

                currentTime = 0;
            }


            yield return null;
        }
    }

    private bool IsAllRequirementsMet()
    {
        foreach (var productionRequirement in _productionRequirements)
        {
            if (!productionRequirement.IsRequirementMet())
            {
                return false;
            }
        }

        return true;
    }

    private void ConsumeAllRequirements(Action onConsumedCallback)
    {
        if (_productionRequirements.Length == 0)
        {
            onConsumedCallback?.Invoke();
        }

        foreach (var productionRequirement in _productionRequirements)
        {
            productionRequirement.ExecuteRequirement(onConsumedCallback);
        }
    }
}