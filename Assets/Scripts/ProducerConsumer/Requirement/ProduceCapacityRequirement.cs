using System;
using System.Collections.Generic;
using UnityEngine;

public class ProduceCapacityRequirement : BaseRequirement
{
    [SerializeField] private int _produceCapacity;
    [SerializeField] private BaseResourceProvider[] _resourceProviders;
    private int _producedCount;
    
    
    public override bool IsRequirementMet()
    {
        _producedCount = 0;
        _resourceProviders.ForEach(provider =>
            _producedCount += provider.GetResourceCount());

        return _producedCount < _produceCapacity;
    }

    public override void ExecuteRequirement(Action onRequirementExecuted)
    {
        onRequirementExecuted();
    }
}