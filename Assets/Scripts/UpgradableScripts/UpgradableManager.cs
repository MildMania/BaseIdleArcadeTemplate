using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradableManager : Singleton<UpgradableManager>
{
    [SerializeField] private CoinController _coinController;

    public CoinController CoinController => _coinController;
    
    private Upgradable[] _upgradables;

    private void Awake()
    {
        _upgradables = GetComponentsInChildren<Upgradable>();
    }

    public Upgradable GetUpgradable(EAttributeCategory attributeCategory, EUpgradable upgradableType)
    {
        Upgradable upgradable = null;

        foreach (var u in _upgradables)
        {
            if (u.AttributeCategory.Equals(attributeCategory) &&
                u.UpgradableType.Equals(upgradableType))
            {
                upgradable = u;
                break;
            }
        }

        return upgradable;
    }
}