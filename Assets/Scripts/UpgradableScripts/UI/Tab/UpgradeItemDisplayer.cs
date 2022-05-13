using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemDisplayer : MonoBehaviour
{
    private const string MAX_TEXT = "MAX";
    
    [Header("Upgradables")]
    [SerializeField] private Upgradable _upgradable;

    [Header("Images")] 
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;

    [Header("Buttons")]
    [SerializeField] private Button _upgradeButton;
    
    [Header("Texts")]
    [SerializeField] private TMP_Text _valueText;

    [Header("Transform References")]
    [SerializeField] private Transform _levelsParent;
    
    [SerializeField] private float _scaleCoef;
    [SerializeField] private float _tweenDuration;

    private RectTransform _rectTransform;
    private Vector3 _defaultScale;
    public Action OnUiElementsUpdated { get; set; }

    private Image _buttonImage;

    public Upgradable Upgradable => _upgradable;
    
    private void Awake()
    {
        _upgradable.OnUpgraded += OnUpgradableUpgraded;
        
        _upgradeButton.onClick.AddListener(OnUpgradeWithCurrencyButtonClicked);

        _rectTransform = GetComponent<RectTransform>();
        _defaultScale = _rectTransform.localScale;

        _buttonImage = _upgradeButton.GetComponent<Image>();
    }

    private void Start()
    {
        UpdateUiElements(_upgradable.UpgradableTrackData);
        OnUiElementsUpdated?.Invoke();
    }

    private void OnUpgradeWithCurrencyButtonClicked()
    {
        if (!_upgradable.TryUpgrade())
        {
            _rectTransform.DOShakePosition(1f, 30f, 100, 45,fadeOut:true);
            return;
        };
        
        _rectTransform.DOScale(_defaultScale * _scaleCoef, _tweenDuration).OnComplete(() =>
        {
            _rectTransform.DOScale(_defaultScale, _tweenDuration);
        });
    }
    
    private void OnUpgradableUpgraded(UpgradableTrackData upgradableTrackData)
    {
        UpdateUiElements(upgradableTrackData);
    }
    
    private void UpdateUiElements(UpgradableTrackData upgradableTrackData)
    {
        RequirementInfo nextRequirementInfo = GameConfigManager.Instance.GetNextRequirementInfo(_upgradable.AttributeCategory, upgradableTrackData);
        
        if (nextRequirementInfo.Level >= 0)
        {
            //NOT MAX LEVEL
            UnlockLevelsUntil(nextRequirementInfo.Level);
            _valueText.text = nextRequirementInfo.Value.ToString();
            _buttonImage.sprite = _enabledSprite;


        }
        else
        {
            //MAX LEVEL
            UnlockAllLevels();
            _valueText.text = MAX_TEXT;
            _valueText.color = Color.black;
            _buttonImage.sprite = _disabledSprite;
        }
        

    }


    
    private void UnlockLevelsUntil(int unlockedLevelCount)
    {

        int unlockCount = unlockedLevelCount - 1;
        for(int i = _levelsParent.childCount - 1; i >= 0; i--)
        {
            var upgradeLevelBarHandler = _levelsParent.GetChild(i).GetComponent<UpgradeLevelBarHandler>();


            if (unlockCount > 0)
            {
                upgradeLevelBarHandler.UnlockLevelBar();
                unlockCount--;
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

    private void OnDestroy()
    {
        _upgradable.OnUpgraded -= OnUpgradableUpgraded;
    }
}
