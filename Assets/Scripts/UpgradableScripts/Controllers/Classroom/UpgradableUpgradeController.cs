using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class UpgradableUpgradeController : MonoBehaviour
{
    [SerializeField] private EAttributeCategory _attributeCategory;
    [SerializeField] private EUpgradable _upgradableType;
    
    private Upgradable _upgradable;
    

    private void Start()
    {
        _upgradable = UpgradableManager.Instance.GetUpgradable(_attributeCategory, _upgradableType);
        
        UpgradeClassroom(_upgradable.UpgradableTrackData);
        
        _upgradable.OnUpgraded += OnClassroomUpgraded;
    }

    private void UpgradeClassroom(UpgradableTrackData upgradableTrackData)
    {
        var classroomUpgradeHandlers = GetComponentsInChildren<UpgradableUpgradeHandler>();

        foreach (var classroomUpgradeHandler in classroomUpgradeHandlers)
        {
            classroomUpgradeHandler.UpgradeByLevel(upgradableTrackData.Level);
        }
        
        float value = GameConfigManager.Instance.GetAttributeUpgradeValue( _upgradable.AttributeCategory, upgradableTrackData);

        var productionControllers = GetComponentsInChildren<ProductionController<MoneyProducer, Money>>();

        foreach (var productionController in productionControllers)
        {
            productionController.UpdateProductionDelay(value);
        }

    }

    private void OnClassroomUpgraded(UpgradableTrackData upgradableTrackData)
    {
       UpgradeClassroom(upgradableTrackData);
    }

    private void OnDestroy()
    {
        _upgradable.OnUpgraded -= OnClassroomUpgraded;
    }
}
