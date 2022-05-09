using System;
using System.Collections.Generic;
using UnityEngine;

public class AutomatCapacityRequirement : BaseRequirement
{

    [SerializeField] private EAttributeCategory _attributeCategory;
    [SerializeField] private EUpgradable _capacityUpgradableType;
    
    [SerializeField] private BaseResourceProvider[] _resourceProviders;

    private Upgradable _automatCapacityUpgradable;
    
    private int _producedCount;
    [SerializeField] private int _produceCapacity;


    private void Start()
    {
        _automatCapacityUpgradable = UpgradableManager.Instance.GetUpgradable(_attributeCategory, _capacityUpgradableType);

        _produceCapacity = (int) GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, _automatCapacityUpgradable.UpgradableTrackData);
        
        _automatCapacityUpgradable.OnUpgraded += OnAutomatCapacityUpgraded;
    }

    private void OnAutomatCapacityUpgraded(UpgradableTrackData upgradableTrackData)
    {
        float value = GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, upgradableTrackData);

        _produceCapacity = (int) value;
    }

    private void OnDestroy()
    {
        _automatCapacityUpgradable.OnUpgraded -= OnAutomatCapacityUpgraded;
    }

    public override bool IsRequirementMet()
    {
        _producedCount = 0;
        _resourceProviders.ForEach(provider =>
            _producedCount += provider.GetResourceCount());

        return _producedCount < _produceCapacity;
    }

}