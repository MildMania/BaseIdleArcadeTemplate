using System;
using UnityEngine;

public class CoinController : MonoBehaviour
{
	private int _currentCoinCount;

	[SerializeField] private CurrencyObserver _currencyObserver;

	public int CurrentCoinCount => _currentCoinCount;
	
	private void Awake()
	{
		LoadCoinCount();
	}

	private void Start()
	{
		_currencyObserver.OnCurrencyUpdated?.Invoke(_currentCoinCount);
	}

	public void Collect(int currentCoinCount)
	{
		_currentCoinCount = currentCoinCount;
		_currencyObserver.OnCurrencyUpdated?.Invoke(_currentCoinCount);
		
	}

	public void UpdateCoinCount()
	{
		LoadCoinCount();
		_currencyObserver.OnCurrencyUpdated?.Invoke(_currentCoinCount);
	}

	private void LoadCoinCount()
	{
		Coin trackable;
		UserManager.Instance.LocalUser.GetUserData<UserCoinInventoryData>().Tracker.TryGetSingle(ECoin.Gold,out trackable);

		_currentCoinCount = trackable.TrackData.CurrentCount;

	}
}