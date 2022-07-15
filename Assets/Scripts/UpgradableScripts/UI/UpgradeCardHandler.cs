using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UpgradeCardHandler : MonoBehaviour
{
    [Header("Upgradable")]
    [SerializeField] private Upgradable _upgradable;
    
    [Header("Buttons")]
    [SerializeField] private Button _upgradeWithCurrencyButton;
    [SerializeField] private Button _upgradeWithAdButton;
    
    [Header("Texts")]
    [SerializeField] private TMP_Text _valueText;

    [Header("Transform References")]
    [SerializeField] private Transform _levelsParent;

    
    [Header("Object References")]
    [SerializeField] private GameObject _maxLevelTextObject;
    [SerializeField] private GameObject _interactablesObject;

    private void Awake()
    {
        _upgradable.OnUpgraded += OnUpgradableUpgraded;
        
        _upgradeWithCurrencyButton.onClick.AddListener(OnUpgradeWithCurrencyButtonClicked);
        _upgradeWithAdButton.onClick.AddListener(OnUpgradeWithAdButtonClicked);
    }
    
    private void Start()
    {
        UpdateUiElements(_upgradable.UpgradableTrackData);
    }
    
    private void OnDestroy()
    {
        _upgradable.OnUpgraded -= OnUpgradableUpgraded;
    }

    private void OnUpgradeWithCurrencyButtonClicked()
    {
        _upgradable.TryUpgrade();
    }

    private void OnUpgradeWithAdButtonClicked()
    {
        //Reward ad call logic comes here!
    }

    private void OnUpgradableUpgraded(UpgradableTrackData upgradableTrackData)
    {
        UpdateUiElements(upgradableTrackData);
    }

    private void UpdateUiElements(UpgradableTrackData upgradableTrackData)
    {
        RequirementInfo nextRequirementInfo = GameConfigManager.Instance.GetNextRequirementInfo(_upgradable.AttributeCategory, upgradableTrackData);
        
        _maxLevelTextObject.SetActive(nextRequirementInfo.Level == -1);
        _interactablesObject.SetActive(nextRequirementInfo.Level >= 0);
        
        if (nextRequirementInfo.Level == -1)
        {

            UnlockAllLevels();
            return;
        }

        Logger.Log("UPGRADED LEVEL: " + nextRequirementInfo.Level + " VALUE: " +nextRequirementInfo.Value);
        _valueText.text = nextRequirementInfo.Value.ToString();
        
        UnlockLevelsUntil(nextRequirementInfo.Level);
    }

    private void UnlockLevelsUntil(int unlockedLevelCount)
    {

        for(int i = 0; i < _levelsParent.childCount; i++)
        {
            var upgradeLevelBarHandler = _levelsParent.GetChild(i).GetComponent<UpgradeLevelBarHandler>();

            if (i < unlockedLevelCount - 1)
            {
                upgradeLevelBarHandler.UnlockLevelBar();
            }
            else
            {
                upgradeLevelBarHandler.LockLevelBar();
            }

        }
    }

    private void UnlockAllLevels()
    {
        for(int i = 0; i < _levelsParent.childCount; i++)
        {
            var upgradeLevelBarHandler = _levelsParent.GetChild(i).GetComponent<UpgradeLevelBarHandler>();
            upgradeLevelBarHandler.UnlockLevelBar();
        }
    }
    
}
