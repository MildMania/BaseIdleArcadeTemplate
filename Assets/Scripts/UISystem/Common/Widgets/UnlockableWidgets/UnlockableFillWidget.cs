using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using MMUtils = MMFramework.Utilities.Utilities;

[Binding]
public class UnlockableFillWidget : FillBarWidget
{
	[SerializeField] private UnlockableObject _unlockableObject;

	[SerializeField] private Image _image;
	public override float FillBarSize => 1;
	
	protected override void AwakeCustomActions()
	{
		_unlockableObject.OnUnlockableInit += OnUnlockableInit;
		_unlockableObject.OnTryUnlock += OnTryUnlock;
		base.AwakeCustomActions();
	}

	private void OnTryUnlock(int oldValue, UnlockableTrackData unlockableTrackData)
	{
		float requirementCoin = _unlockableObject.Unlockable.GetRequirementCoin();
		float unlockCount = unlockableTrackData.CurrentCount;
		float normVal =  MMUtils.Normalize(unlockCount, requirementCoin,
			0, 1, 0);

		TryUpdateBar(normVal,0);
	}

	private void OnUnlockableInit(UnlockableTrackData unlockableTrackData)
	{
		float requirementCoin = _unlockableObject.Unlockable.GetRequirementCoin();
		float unlockCount = unlockableTrackData.CurrentCount;
		float normVal =  MMUtils.Normalize(unlockCount, requirementCoin,
		0, 1, 0);

		TryUpdateBar(normVal,0);
	}
	

	private bool TryUpdateBar(float normVal,float delay)
	{
		DOTween.To(() => _image.fillAmount, x => _image.fillAmount = x, normVal, delay);
		
		return true;
	}
	
	protected override void OnDestroyCustomActions()
	{
		_unlockableObject.OnUnlockableInit -= OnUnlockableInit;
		_unlockableObject.OnTryUnlock -= OnTryUnlock;
		base.OnDestroyCustomActions();
	}
}