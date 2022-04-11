using DG.Tweening;
using MMFramework.MMUI;
using TMPro;
using UnityEngine;

public class UnlockableTextWidget : WidgetBase
{
	[SerializeField] private UnlockableObject _unlockableObject;
	
	[SerializeField] private TMP_Text _targetText;
	
	[SerializeField] private string _preFix = "$";
	
	protected override void AwakeCustomActions()
	{
		_unlockableObject.OnUnlockableInit += OnUnlockableInit;
		_unlockableObject.OnTryUnlock += OnTryUnlock;

		base.AwakeCustomActions();
	}

	protected override void OnDestroyCustomActions()
	{
		_unlockableObject.OnUnlockableInit -= OnUnlockableInit;
		_unlockableObject.OnTryUnlock -= OnTryUnlock;
		
		base.OnDestroyCustomActions();
	}

	private void OnTryUnlock(int oldValue, UnlockableTrackData unlockableTrackData)
	{
		int totalRequirement = _unlockableObject.Unlockable.GetRequirementCoin();
		TryUpdateText(oldValue, unlockableTrackData.CurrentCount,totalRequirement,0);
	}

	private bool TryUpdateText(int oldValue, int currentValue,int totalRequirement,float delay)
	{
		int target = totalRequirement - currentValue;
		if (oldValue == target)
		{
			return false;
		}
		
		DOTween.To(() => oldValue, x => oldValue = x, target, delay)
			.OnUpdate(() =>
			{
				_targetText.text = _preFix + oldValue;
			});
		
		return true;
	}
	
	private void OnUnlockableInit(UnlockableTrackData unlockableTrackData)
	{
		int totalRequirement = _unlockableObject.Unlockable.GetRequirementCoin();
		_targetText.text = _preFix + (totalRequirement - unlockableTrackData.CurrentCount);
	}
}