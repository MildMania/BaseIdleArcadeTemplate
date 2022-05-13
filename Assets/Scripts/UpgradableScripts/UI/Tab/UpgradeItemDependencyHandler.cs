using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemDependencyHandler : MonoBehaviour
{
    [Header("Images")] [SerializeField] private Image _upgradableImage;

    [Header("Buttons")] [SerializeField] private Button _upgradableButton;

    [Header("Sprites")] [SerializeField] private Sprite _readyToUpgradeSprite;
    [SerializeField] private Sprite _notReadyToUpgradeSprite;

    [SerializeField] private EAttributeCategory _attributeCategory;
    [SerializeField] private EUpgradable _upgradable;

    private UpgradeItemDisplayer _upgradeItemDisplayer;

    private Upgradable _dependentUpgradable;


    private void Awake()
    {
        _upgradeItemDisplayer = GetComponent<UpgradeItemDisplayer>();
        _upgradeItemDisplayer.OnUiElementsUpdated += OnUiElementsUpdated;
    }

    private void Start()
    {
        _dependentUpgradable = UpgradableManager.Instance.GetUpgradable(_attributeCategory, _upgradable);
        _dependentUpgradable.OnUpgraded += OnUpgradableUpgraded;
    }


    private void OnDestroy()
    {
        if (_dependentUpgradable != null)
        {
            _dependentUpgradable.OnUpgraded -= OnUpgradableUpgraded;
        }


        if (_upgradeItemDisplayer != null)
        {
            _upgradeItemDisplayer.OnUiElementsUpdated -= OnUiElementsUpdated;
        }
    }

    private void OnUiElementsUpdated()
    {
        if (_dependentUpgradable == null)
        {
            _dependentUpgradable = UpgradableManager.Instance.GetUpgradable(_attributeCategory, _upgradable);
        }

        int currentLevel = _dependentUpgradable.UpgradableTrackData.Level;

        if (currentLevel == 0)
        {
            SetNotReadyToUpgrade();
        }
        else
        {
            Foo();
        }
    }

    private void OnUpgradableUpgraded(UpgradableTrackData upgradableTrackData)
    {
        RequirementInfo nextRequirementInfo =
            GameConfigManager.Instance.GetNextRequirementInfo(_dependentUpgradable.AttributeCategory,
                upgradableTrackData);


        if (nextRequirementInfo.Level - 1 == 0)
        {
            SetNotReadyToUpgrade();
        }
        else
        {
            Foo();
        }
    }

    private void Foo()
    {
        RequirementInfo nextRequirementInfo =
            GameConfigManager.Instance.GetNextRequirementInfo(_upgradeItemDisplayer.Upgradable.AttributeCategory,
                _upgradeItemDisplayer.Upgradable.UpgradableTrackData);

        if (nextRequirementInfo.Level == -1)
        {
            SetNotReadyToUpgrade();
        }
        else
        {
            SetReadyToUpgrade();
        }
    }


    private void SetReadyToUpgrade()
    {
        _upgradableImage.sprite = _readyToUpgradeSprite;
        _upgradableButton.interactable = true;
    }

    private void SetNotReadyToUpgrade()
    {
        _upgradableImage.sprite = _notReadyToUpgradeSprite;
    }
}