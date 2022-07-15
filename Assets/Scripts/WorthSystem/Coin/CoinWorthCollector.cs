using System;
using MMFramework.UserSystem;
using UnityEngine;

[RequireComponent(typeof(IUserProvider))]
public class CoinWorthCollector : WorthCollector<CoinWorth>
{
    [SerializeField]  private CoinController _coinController;

    private IUserProvider _userProvider;
    public IUserProvider UserProvider
    {
        get
        {
            if (_userProvider == null)
                _userProvider = GetComponent<IUserProvider>();

            return _userProvider;
        }
    }

    private CoinInventory _coinInventory;
    private CoinInventory _CoinInventory
    {
        get
        {
            if (_coinInventory == null)
            {
                UserProvider.User.InventoryController.TryGetInventory(out _coinInventory);
            }
   

            return _coinInventory;
        }
    }
    
    protected override void CollectWorthCustomActions(CoinWorth coinWorth)
    {
        Logger.Log("Coin Count : " + coinWorth.Count + " CoinType : " + coinWorth.Coin);
        Logger.Log("CollectWorthCustomActions");
        
        Coin trackable;
        _CoinInventory.Tracker.TryGetSingle(ECoin.Gold, out trackable);
        int currentCountCount = trackable.TrackData.CurrentCount + coinWorth.Count;
        
        CoinTrackData coinTrackData =
            new CoinTrackData(
                coinWorth.Coin,
                count: currentCountCount);
        
        _CoinInventory.Tracker.TryUpsert(
            coinTrackData);
        _coinController.Collect(currentCountCount);
        base.CollectWorthCustomActions(coinWorth);
        
    }
}