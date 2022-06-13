using MMFramework.TasksV2;
using System;
using UnityEngine;

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
		
		Coin trackableCoin;
		userCoinInventoryData.Tracker.TryGetSingle(ECoin.Gold, out trackableCoin);
		
		int totalRequiredAmount = 0;
			
		foreach (var requirement in Requirements)
		{
			var requirementCoin = (RequirementCoin) requirement;

			totalRequiredAmount += requirementCoin.RequirementData.RequiredAmount;
		}

		int maxRequiredAmount = totalRequiredAmount;
		totalRequiredAmount -= _unlockableTrackData.CurrentCount;
		
		if (trackableCoin.TrackData.CurrentCount == 0)
		{
			if (totalRequiredAmount == 0)
			{
				user.UnlockableUpdater.UpdateUnlockable(_unlockableTrackData.TrackID,_unlockableTrackData.CurrentCount,true);
				return true;
			}
			return false;
		}
		
		if (totalRequiredAmount != 0 )
		{
			CoinTrackData coinTrackData;
			if (amount >= totalRequiredAmount)
			{
				user.UnlockableUpdater.UpdateUnlockable(_unlockableTrackData.TrackID,maxRequiredAmount,true);
				coinTrackData =
					new CoinTrackData(
						ECoin.Gold,
						count: trackableCoin.TrackData.CurrentCount - totalRequiredAmount);
		
				trackableCoin.UpdateData(coinTrackData);
				OnLockedValueChanged?.Invoke();
				
				return true;
			}
			
			user.UnlockableUpdater.UpdateUnlockable(_unlockableTrackData.TrackID,_unlockableTrackData.CurrentCount + amount,false);

			coinTrackData =
				new CoinTrackData(
					ECoin.Gold,
					count: trackableCoin.TrackData.CurrentCount - amount);
		
			trackableCoin.UpdateData(coinTrackData);
			OnLockedValueChanged?.Invoke();
			
			return false;
		}
		
		user.UnlockableUpdater.UpdateUnlockable(_unlockableTrackData.TrackID,maxRequiredAmount,true);
		return true;
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