using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeLevelBarHandler : MonoBehaviour
{
    [SerializeField] private Sprite _levelLockedSprite;
    [SerializeField] private Sprite _levelUnlockedSprite;

    private Image _levelBarImage;
    
    private void Awake()
    {
        _levelBarImage = GetComponent<Image>();
    }

    public void LockLevelBar()
    {
        _levelBarImage.sprite = _levelLockedSprite;
    }

    public void UnlockLevelBar()
    {
        _levelBarImage.sprite = _levelUnlockedSprite;
    }
}
