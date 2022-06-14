using UnityEngine;

public class SingleMoneyCollectBehaviour : MonoBehaviour
{
	[SerializeField] private CoinWorthCollector _coinWorthCollector;

	[SerializeField] private SingleMoneyFovController _moneyFovController;

	[SerializeField] private SingleMoneyTweenBehaviour _singleMoneyTweenBehaviour;

	[SerializeField]
	private iOSHapticFeedback.iOSFeedbackType _hapticType = iOSHapticFeedback.iOSFeedbackType.ImpactMedium;

	private OnHapticRequestedEventRaiser _onHapticRequestedEventRaiser = new OnHapticRequestedEventRaiser();

	private void Awake()
	{
		_moneyFovController.OnTargetEnteredFieldOfView += OnTargetEnteredFieldOfView;
		_moneyFovController.OnTargetExitedFieldOfView += OnTargetExitedFieldOfView;
	}

	private void OnTargetExitedFieldOfView(SingleMoney singleMoney)
	{
	}


	private void OnTargetEnteredFieldOfView(SingleMoney singleMoney)
	{
		_onHapticRequestedEventRaiser.Raise(new OnHapticRequestedEventArgs(_hapticType));
		var worthDefiner = singleMoney.GetComponent<CoinWorthDefiner>();
		_coinWorthCollector.CollectWorth(new CoinWorth(worthDefiner.Coin, worthDefiner.Count));


		Vector3 singleMoneyScreenPosition =
			GameUtilities.GetWorldToScreenSpace(singleMoney.transform.position,
				CameraManager.Instance.MainCamera, _singleMoneyTweenBehaviour.TweenArea);

		singleMoney.DestroyItself();
		_singleMoneyTweenBehaviour.TweenMoney(singleMoneyScreenPosition);
	}


	private void OnDestroy()
	{
		_moneyFovController.OnTargetEnteredFieldOfView -= OnTargetEnteredFieldOfView;
		_moneyFovController.OnTargetExitedFieldOfView -= OnTargetExitedFieldOfView;
	}
}