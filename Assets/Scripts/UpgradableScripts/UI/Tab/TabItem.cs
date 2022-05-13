using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TabItem : MonoBehaviour
{
    [SerializeField] private ETabCategory _tabCategory;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;
    
    [SerializeField] private float _scaleCoef;
    [SerializeField] private float _tweenDuration;
    
    public ETabCategory TabCategory => _tabCategory;

    private Button _tabButton;
    private Image _tabImage;

    private RectTransform _rectTransform;
    
    private Vector3 _defaultScale;
    
    public Action<ETabCategory> OnTabButtonClicked { get; set; }

    private void Awake()
    {
        
        _tabButton = GetComponent<Button>();
        _tabImage = GetComponent<Image>();
        
        
        _rectTransform = _tabButton.GetComponent<RectTransform>();

        _defaultScale = _rectTransform.localScale;
        
        _tabButton.onClick.AddListener(ListenTapButtonClick);
    }

    private void ListenTapButtonClick()
    {
        OnTabButtonClicked?.Invoke(_tabCategory);

        

        _rectTransform.DOScale(_defaultScale * _scaleCoef, _tweenDuration).OnComplete(() =>
        {
            _rectTransform.DOScale(_defaultScale, _tweenDuration);
        });
    }

    public void SetButtonActive(bool isActive)
    {
        _tabButton.interactable = isActive;
        
        if (isActive)
        {
            _tabImage.sprite = _activeSprite;
        }
        else
        {
            _tabImage.sprite = _inactiveSprite;
        }
    }


}
