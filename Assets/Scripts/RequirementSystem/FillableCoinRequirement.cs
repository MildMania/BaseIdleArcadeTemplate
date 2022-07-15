using UnityEngine;
using WarHeroes.InventorySystem;

public class FillableCoinRequirement : RequirementCoin, IFillable
{
	const string DATA_FOLDER_PATH = "SaveData/UserData/";
	private CoinTracker _coinTracker;
	
	public FillableCoinRequirement(User user, RequirementDataCoin data) : base(user, data)
	{
	}
	
	private Fillable _fillable;
	public Fillable Fillable
	{
		get
		{
			if (_fillable == null)
				_fillable = new Fillable(0);

			return _fillable;
		}
	}
	
	public void TryFill(User user)
	{
		Coin trackable;
		
		user.GetUserData<UserCoinInventoryData>().Tracker.TryGetSingle(ECoin.Gold,out trackable);

		int goldAmount = trackable.TrackData.CurrentCount;
		
		Logger.Log("CURRENT GOLD COUNT: " + goldAmount);

		int fillAmount;
		
		Fillable.Fill(goldAmount,out fillAmount);
		
		Logger.Log("FİLL AMOUNT: " + fillAmount);
		
	}
}