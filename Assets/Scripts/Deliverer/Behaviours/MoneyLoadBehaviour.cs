using UnityEngine;

public class MoneyLoadBehaviour : BaseLoadBehaviour<MoneyProducer, Money>
{
    [SerializeField] private CoinWorthCollector _coinWorthCollector;
    [SerializeField] private MoneyProducerFovController _moneyProducerFovController;

    protected override void OnAwakeCustomActions()
    {
        base.OnAwakeCustomActions();

        _moneyProducerFovController.OnTargetEnteredFieldOfView += OnProducerEnteredFieldOfView;
        _moneyProducerFovController.OnTargetExitedFieldOfView += OnProducerExitedFieldOfView;
        _isUpgradableActive = false;
        _loadDelay = 0.001f;
    }

    protected override void OnDestroyCustomActions()
    {
        base.OnDestroyCustomActions();

        _moneyProducerFovController.OnTargetEnteredFieldOfView -= OnProducerEnteredFieldOfView;
        _moneyProducerFovController.OnTargetExitedFieldOfView -= OnProducerExitedFieldOfView;
    }

    public override void LoadCustomActions(Money money)
    {
        money.OnMovementFinished += OnMoveRoutineFinished;
        money.Move(_deliverer.transform, null, Instantiate(_baseResourceMovementBehaviour));
    }

    private void OnMoveRoutineFinished(BaseResource resource)
    {
        Money money = (Money) resource;
        money.gameObject.SetActive(false);
        money.OnMovementFinished -= OnMoveRoutineFinished;
        var worthDefiner = money.GetComponent<CoinWorthDefiner>();
        _coinWorthCollector.CollectWorth(new CoinWorth(worthDefiner.Coin, worthDefiner.Count));
    }
}