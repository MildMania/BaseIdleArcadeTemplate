using System;
using System.Collections;
using UnityEngine;

public class ProductionController<TProducer, TResource> : MonoBehaviour where TProducer : BaseProducer<TResource>
    where TResource : IResource
{
    [SerializeField] protected TProducer _producer;
    [SerializeField] protected TResource _resource;
    [SerializeField] private BaseProductionRequirement[] _productionRequirements;
    
    private float _productionDelay;

    public void UpdateProductionDelay(float value)
    {
        _productionDelay = 1 / value;
    }
    
    
    protected IEnumerator ProduceRoutine(TResource resource)
    {
        float currentTime = 0;

        while (true)
        {
            currentTime += Time.deltaTime;

            if (currentTime > _productionDelay)
            {
                if (IsAllRequirementsMet())
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
            if (!productionRequirement.IsProductionRequirementMet())
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
            productionRequirement.ConsumeRequirements(onConsumedCallback);
        }
    }
}