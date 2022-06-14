using UnityEngine;

[RequireComponent(typeof(SingleMoneyFovController))]
public class FovBasedSingleMoneyDetector : BaseSingleMoneyDetector
{
    [SerializeField] private SingleMoneyFovController _singleMoneyFOVController;

    private void Awake()
    {
        SubscribeToFovController();
        _singleMoneyFOVController.SetActive(true);
    }

    private void OnDestroy()
    {
        _singleMoneyFOVController.SetActive(false);
        UnsubscribeFromFovController();
    }

    private void SubscribeToFovController()
    {
        _singleMoneyFOVController.OnTargetEnteredFieldOfView += OnTargetEnteredFieldOfView;
        _singleMoneyFOVController.OnTargetExitedFieldOfView += OnTargetExitedFieldOfView;
    }

    private void UnsubscribeFromFovController()
    {
        _singleMoneyFOVController.OnTargetEnteredFieldOfView -= OnTargetEnteredFieldOfView;
        _singleMoneyFOVController.OnTargetExitedFieldOfView -= OnTargetExitedFieldOfView;
    }

    private void OnTargetEnteredFieldOfView(SingleMoney singleMoney)
    {
        LastDetected = singleMoney;
        OnDetected?.Invoke(singleMoney);
    }

    private void OnTargetExitedFieldOfView(SingleMoney singleMoney)
    {
        OnEnded?.Invoke(singleMoney);
    }
}