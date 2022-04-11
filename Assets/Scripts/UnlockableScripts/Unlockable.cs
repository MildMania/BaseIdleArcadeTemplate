using MMFramework.TasksV2;
using System;
using System.Linq;
using Sirenix.Serialization;
using UnityEngine;
using WarHeroes.InventorySystem;
using Countable = MMFramework.InventorySystem.Countable;


public interface IUnlockable
{
	Unlockable Unlockable { get; }
}

[Serializable]
public class Unlockable
{
	public IRequirement[] Requirements = Array.Empty<IRequirement>();
	public bool IsLocked { get; private set; }
	public int Count { get; private set; }

	private UnlockableTrackData _unlockableTrackData;

	// [OdinSerialize] public IRequirementData[] RequirementData
	// 	= Array.Empty<IRequirementData>();

	[SerializeField] private MMTaskExecutor _lockedTaskExecutor
		= new MMTaskExecutor();

	[SerializeField] private MMTaskExecutor _unlockedTaskExecutor
		= new MMTaskExecutor();

	public Action<bool> OnLockedChanged { get; set; }
	public Action OnLockedValueChanged { get; set; }
	public void Init(UnlockableTrackData trackData)
	{
		_unlockableTrackData = trackData;
		IsLocked = trackData.IsUnlock;
		Count = trackData.CurrentCount;
	}

	public bool TryUnlock(User user,int amount)
	{
		var userCoinInventoryData = user.GetUserData<UserCoinInventoryData>();

		var userUnlockableData = user.GetUserData<UserUnlockableData>();
		
		Coin trackableCoin;
		userCoinInventoryData.Tracker.TryGetSingle(ECoin.Gold, out trackableCoin);
		
		int totalRequiredAmount = 0;
			
		foreach (var requirement in Requirements)
		{
			var requirementCoin = (RequirementCoin) requirement;

			totalRequiredAmount += requirementCoin.RequirementData.RequiredAmount;
		}

		totalRequiredAmount -= _unlockableTrackData.CurrentCount;

		if (trackableCoin.TrackData.CurrentCount == 0)
		{
			if (totalRequiredAmount == 0)
			{
				UnlockableTrackData unlockableTrackData = new UnlockableTrackData(_unlockableTrackData.TrackID, 
					_unlockableTrackData.CurrentCount,
					true);
				userUnlockableData.Tracker.TryUpsert(unlockableTrackData);
				return true;
			}
			return false;
		}
		
		if (totalRequiredAmount != 0 )
		{
			UnlockableTrackData unlockableTrackData = new UnlockableTrackData(_unlockableTrackData.TrackID, 
				_unlockableTrackData.CurrentCount + amount,false);
			userUnlockableData.Tracker.TryUpsert(unlockableTrackData);
			
			CoinTrackData coinTrackData =
				new CoinTrackData(
					ECoin.Gold,
					count: trackableCoin.TrackData.CurrentCount - amount);
		
			trackableCoin.UpdateData(coinTrackData);
			OnLockedValueChanged?.Invoke();
			
			return false;
		}
		else
		{
			UnlockableTrackData unlockableTrackData = new UnlockableTrackData(_unlockableTrackData.TrackID, 
				_unlockableTrackData.CurrentCount,
				true);
			userUnlockableData.Tracker.TryUpsert(unlockableTrackData);
			
			return true;
		}
	}
	
	public int GetRequirementCoin()
	{
		int requirementCoin = 0;
		foreach (var requirement in Requirements)
		{
			if (requirement is FillableCoinRequirement fillableCoinRequirement)
			{
				requirementCoin += fillableCoinRequirement.RequirementData.RequiredAmount;
			}
		}

		return requirementCoin;
	}
}