using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutomatUpgradeUiHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private CoinWorthDefiner _coinWorthDefiner;
    
    [SerializeField] private EAttributeCategory _attributeCategory;
    [SerializeField] protected EUpgradable  _automatUpgradableType;

    private Upgradable _automatCapacityUpgradable;
    
    private void Start()
    {
        _automatCapacityUpgradable = UpgradableManager.Instance.GetUpgradable(_attributeCategory, _automatUpgradableType);

        int capacity = (int) GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, _automatCapacityUpgradable.UpgradableTrackData);

        int money = capacity * _coinWorthDefiner.Count;
        _moneyText.text = "$ " + money;
        
        _automatCapacityUpgradable.OnUpgraded += OnSpeedUpgraded;
    }

    private void OnSpeedUpgraded(UpgradableTrackData upgradableTrackData)
    {
        int value = (int)GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, upgradableTrackData);

        int money = value * _coinWorthDefiner.Count;
        
        _moneyText.text = "$ " + money;
    }

    private void OnDestroy()
    {
        _automatCapacityUpgradable.OnUpgraded -= OnSpeedUpgraded;
    }
}
