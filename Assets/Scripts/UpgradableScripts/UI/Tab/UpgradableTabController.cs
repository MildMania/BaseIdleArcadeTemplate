using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class UpgradableTabController : SerializedMonoBehaviour
{
    private TabItem[] _tabItems;
    private ContentItem[] _contentItems;
    private BackGroundItem[] _backGroundItems;
    private void Start()
    {
        _tabItems = GetComponentsInChildren<TabItem>();
        _contentItems = GetComponentsInChildren<ContentItem>();
        _backGroundItems = GetComponentsInChildren<BackGroundItem>();
        
        foreach (var tabItem in _tabItems)
        {
            tabItem.OnTabButtonClicked += OnTabButtonClicked;
        }
        
        InitFirstTab();
    }

    private void OnEnable()
    {
        InitFirstTab();
    }

    private void InitFirstTab()
    {
        OnTabButtonClicked(ETabCategory.TAB_1);
    }

    private void OnTabButtonClicked(ETabCategory tabCategory)
    {
        if (_contentItems == null || _tabItems == null)
        {
            return;
        }

        
        foreach (var contentItem in _contentItems)
        {
            contentItem.gameObject.SetActive(contentItem.TabCategory == tabCategory);
        }

        foreach (var backGroundItem in _backGroundItems)
        {
            backGroundItem.gameObject.SetActive(backGroundItem.TabCategory == tabCategory);
        }
        
        foreach (var tabItem in _tabItems)
        {
            tabItem.SetButtonActive(tabItem.TabCategory != tabCategory);
        }
    }

    private void OnDestroy()
    {
        foreach (var tabItem in _tabItems)
        {
            tabItem.OnTabButtonClicked -= OnTabButtonClicked;
        }
    }
}
