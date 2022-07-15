using System.Collections.Generic;
using System;
using WarHeroes.InventorySystem;

public class User
{
    public enum EUser
    {
        Default,
        Fake,
    }

    public bool IsLocalUser { get; private set; }

    public bool IsDataLoaded { get; private set; }

    private Dictionary<Type, UserData> _userDataMap;

    public InventoryController<EInventory> InventoryController { get; private set; }

    public UnlockableUpdater UnlockableUpdater { get; private set; }

    private Action _onDataLoaded;
    
    public User(
        bool isLocalUser,
        InventoryController<EInventory> inventoryController,
        UnlockableUpdater unlockableUpdater,
        UpgradableUpdater upgradableUpdater,
        params UserData[] userDataParams)
    {
        IsLocalUser = isLocalUser;
        
        InventoryController = inventoryController;

        UnlockableUpdater = unlockableUpdater;

        InitUserDataMap(userDataParams);
    }

    private void InitUserDataMap(UserData[] userDatas)
    {
        _userDataMap = new Dictionary<Type, UserData>();

        foreach (UserData userData in userDatas)
        {
            userData.InitUser(this);
            _userDataMap.Add(userData.GetType(), userData);
        }
    }

    public T GetUserData<T>()
        where T :UserData
    {
        return (T)_userDataMap[typeof(T)];
    }

    public void WaitUntilDataLoad(Action onLoadedCallback)
    {
        if (IsDataLoaded)
            onLoadedCallback?.Invoke();

        if (onLoadedCallback != null)
            _onDataLoaded += onLoadedCallback;
    }

    public void LoadData(Action onLoadedCallback)
    {
        int subDataCount = _userDataMap.Count;
        int loadedSubDataCount = 0;

        void onSubDataLoaded()
        {
            loadedSubDataCount++;

            if (loadedSubDataCount < subDataCount)
                return;

            IsDataLoaded = true;

            _onDataLoaded?.Invoke();
            _onDataLoaded = null;

            onLoadedCallback?.Invoke();
        }

        //Logger.Log("Load User Data");
        foreach (var kvp in _userDataMap)
            kvp.Value.LoadData(onSubDataLoaded);
    }

    public void SaveData(Action onSavedCallback)
    {
        int subDataCount = _userDataMap.Count;
        int savedSubDataCount = 0;

        void onSubDataSaved()
        {
            savedSubDataCount++;

            if (savedSubDataCount < subDataCount)
                return;

            onSavedCallback?.Invoke();
        }

        foreach (var kvp in _userDataMap)
            kvp.Value.SaveData(onSubDataSaved);
    }
}
